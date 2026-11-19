using System.Windows;

namespace WeiDi.Game;

/// <summary>
/// 空地的边界：一条闭合折线。开局就是正方形的四条边。
/// 每次围地成功，环上「从起笔点到回框点的较短那段」被这次轨迹替换 ——
/// 轨迹线因此成为新的边框，圆点以后可以沿它走、也可以从它再切入空地。
/// 圆点在边框上的位置用「沿环的弧长 s」表示。
/// </summary>
internal sealed class Boundary
{
    private const double Eps = 1e-8;
    private const double SameEps = 1e-6;

    /// <summary>判定「这一侧是空地」时，从线段往外采样多远。</summary>
    private const double InwardProbe = 4.0;

    private List<Point> _pts = new();
    private List<Point> _extPts = new();          // 顶点铺两遍，沿环走不用处理回绕
    private double[] _extCum = Array.Empty<double>();
    private int[] _normalSign = Array.Empty<int>();   // 每段「空地法向」的符号缓存，0=未算
    private double _length;
    private int _version;

    public Boundary() => Reset();

    public IReadOnlyList<Point> Points => _pts;

    public double Length => _length;

    /// <summary>边框每变一次就 +1，渲染层据此重建几何。</summary>
    public int Version => _version;

    public void Reset()
    {
        _pts = new List<Point>
        {
            new(0, 0),
            new(Arena.S, 0),
            new(Arena.S, Arena.S),
            new(0, Arena.S),
        };
        Rebuild();
    }

    // ── 弧长 → 点 / 方向 ──────────────────────────────────────

    public double Wrap(double s)
    {
        if (_length <= Eps) return 0;
        double m = s % _length;
        return m < 0 ? m + _length : m;
    }

    /// <summary>s 落在第几段（0..n-1）。</summary>
    public int SegmentAt(double s)
    {
        int n = _pts.Count;
        if (n == 0) return 0;
        s = Wrap(s);
        int lo = 0;
        int hi = n - 1;
        while (lo < hi)
        {
            int mid = (lo + hi + 1) >> 1;
            if (_extCum[mid] <= s) lo = mid;
            else hi = mid - 1;
        }
        return lo;
    }

    public double SegmentStart(int i) => _extCum[Math.Max(0, Math.Min(i, _pts.Count))];

    public Point PointAt(double s)
    {
        if (_pts.Count == 0) return default;
        s = Wrap(s);
        int i = SegmentAt(s);
        Point a = _extPts[i];
        Point b = _extPts[i + 1];
        double seg = _extCum[i + 1] - _extCum[i];
        if (seg <= Eps) return a;
        double t = (s - _extCum[i]) / seg;
        return new Point(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);
    }

    /// <summary>s 处、朝 s 增大方向的单位切向。</summary>
    public Point TangentAt(double s)
    {
        if (_pts.Count == 0) return new Point(1, 0);
        int i = SegmentAt(s);
        Point a = _extPts[i];
        Point b = _extPts[i + 1];
        double dx = b.X - a.X;
        double dy = b.Y - a.Y;
        double len = Math.Sqrt(dx * dx + dy * dy);
        if (len <= Eps) return new Point(1, 0);
        return new Point(dx / len, dy / len);
    }

    /// <summary>第 i 段上、指向空地的那条法向。</summary>
    public Point InwardNormalOf(int i, PaintGrid paint)
    {
        int n = _pts.Count;
        if (n == 0) return new Point(0, 1);
        i = ((i % n) + n) % n;

        Point a = _extPts[i];
        Point b = _extPts[i + 1];
        double dx = b.X - a.X;
        double dy = b.Y - a.Y;
        double len = Math.Sqrt(dx * dx + dy * dy);
        if (len <= Eps) return new Point(0, 1);

        Point baseNormal = new(-dy / len, dx / len);

        int sign = _normalSign[i];
        if (sign == 0)
        {
            Point mid = new((a.X + b.X) * 0.5, (a.Y + b.Y) * 0.5);
            Point probe = new(mid.X + baseNormal.X * InwardProbe, mid.Y + baseNormal.Y * InwardProbe);
            sign = PaintFree(probe, paint) ? 1 : -1;
            _normalSign[i] = sign;
        }

        return sign > 0 ? baseNormal : new Point(-baseNormal.X, -baseNormal.Y);
    }

    /// <summary>s 处、指向空地的那条法向。</summary>
    public Point InwardAt(double s, PaintGrid paint) => InwardNormalOf(SegmentAt(s), paint);

    /// <summary>点 p 在环上的最近位置（弧长）。</summary>
    public double Project(Point p)
    {
        int n = _pts.Count;
        if (n == 0) return 0;

        double bestS = 0;
        double bestD = double.MaxValue;

        for (int i = 0; i < n; i++)
        {
            Point a = _extPts[i];
            Point b = _extPts[i + 1];
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;
            double len2 = dx * dx + dy * dy;

            double t = 0;
            if (len2 > Eps)
            {
                t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / len2;
                if (t < 0) t = 0;
                else if (t > 1) t = 1;
            }

            double qx = a.X + dx * t;
            double qy = a.Y + dy * t;
            double d = (p.X - qx) * (p.X - qx) + (p.Y - qy) * (p.Y - qy);
            if (d < bestD)
            {
                bestD = d;
                bestS = _extCum[i] + (_extCum[i + 1] - _extCum[i]) * t;
            }
        }

        return Wrap(bestS);
    }

    // ── 围地结算 ─────────────────────────────────────────────

    /// <summary>
    /// 以起笔点 sA、回框点 sB 结算一次围地：
    /// 返回「轨迹 + 较短那段边框」围成的多边形（要填色的那块），
    /// newLoop 是替换后的新边框（轨迹替换掉短弧）；确认手势有效后再 <see cref="Apply"/>。
    /// </summary>
    public List<Point> Capture(double sA, double sB, IReadOnlyList<Point> trail, out List<Point> newLoop)
    {
        double fwdBA = Wrap(sA - sB);        // 从 B 沿环前进到 A 的长度

        // 起笔点和回框点几乎重合：没围出东西，边框保持不变
        if (fwdBA <= Eps || _length - fwdBA <= Eps)
        {
            newLoop = _pts;
            return new List<Point>();
        }

        List<Point> keptBtoA;                 // 保留的那段「长路」，方向 B→A
        List<Point> shortBtoA;                // 被围掉的「短弧」，方向 B→A

        if (fwdBA >= _length * 0.5)
        {
            keptBtoA = Walk(sB, sA);          // 前进 B→A 本身就是长路
            shortBtoA = Walk(sA, sB);         // 前进 A→B 是短弧
            shortBtoA.Reverse();
        }
        else
        {
            shortBtoA = Walk(sB, sA);         // 前进 B→A 本身就是短弧
            keptBtoA = Walk(sA, sB);          // 前进 A→B 是长路
            keptBtoA.Reverse();
        }

        // 被围掉的多边形 = 轨迹 A→B + 短弧 B→A
        var poly = new List<Point>(trail.Count + shortBtoA.Count + 2);
        for (int i = 0; i < trail.Count; i++) poly.Add(trail[i]);
        for (int i = 1; i < shortBtoA.Count - 1; i++) poly.Add(shortBtoA[i]);
        while (poly.Count > 2 && Same(poly[0], poly[^1])) poly.RemoveAt(poly.Count - 1);

        // 新边框 = 长路 B→A + 这次轨迹 A→B
        var loop = new List<Point>(keptBtoA.Count + trail.Count + 2);
        for (int i = 0; i < keptBtoA.Count; i++) loop.Add(keptBtoA[i]);
        for (int i = 1; i < trail.Count; i++) loop.Add(trail[i]);
        while (loop.Count > 3 && Same(loop[0], loop[^1])) loop.RemoveAt(loop.Count - 1);

        newLoop = loop;
        return poly;
    }

    /// <summary>采用新的边框（Capture 算出来、且手势有效的那份）。</summary>
    public void Apply(List<Point> loop)
    {
        _pts = loop;
        Rebuild();
    }

    // ── 内部 ─────────────────────────────────────────────────

    /// <summary>沿环前进，返回从 from 到 to 的点列（含两端）。</summary>
    private List<Point> Walk(double from, double to)
    {
        from = Wrap(from);
        double span = Wrap(Wrap(to) - from);

        var list = new List<Point> { PointAt(from) };
        if (span <= Eps) return list;             // 起止重合：就是这一个点

        double target = from + span;
        int ek = SegmentAt(from) + 1;
        int limit = 2 * _pts.Count;

        while (ek <= limit && _extCum[ek] < target - 1e-9)
        {
            if (!Same(list[^1], _extPts[ek])) list.Add(_extPts[ek]);
            ek++;
        }

        Point end = PointAt(target);
        if (!Same(list[^1], end)) list.Add(end);
        return list;
    }

    private void Rebuild()
    {
        // 去掉完全重复的相邻点、以及首尾重复
        var pts = new List<Point>(_pts.Count);
        foreach (Point p in _pts)
        {
            if (pts.Count == 0 || !Same(pts[^1], p)) pts.Add(p);
        }
        while (pts.Count > 3 && Same(pts[0], pts[^1])) pts.RemoveAt(pts.Count - 1);

        _pts = pts;
        int n = _pts.Count;

        _extPts = new List<Point>(2 * n + 1);
        _extCum = new double[2 * n + 1];
        for (int i = 0; i <= 2 * n; i++) _extPts.Add(_pts[i % n]);
        for (int i = 0; i < 2 * n; i++)
        {
            double dx = _extPts[i + 1].X - _extPts[i].X;
            double dy = _extPts[i + 1].Y - _extPts[i].Y;
            _extCum[i + 1] = _extCum[i] + Math.Sqrt(dx * dx + dy * dy);
        }

        _length = _extCum[n];
        _normalSign = new int[Math.Max(n, 1)];
        _version++;
    }

    private static bool PaintFree(Point p, PaintGrid paint) => paint.IsFree(p);

    private static bool Same(Point a, Point b) =>
        Math.Abs(a.X - b.X) < SameEps && Math.Abs(a.Y - b.Y) < SameEps;
}
