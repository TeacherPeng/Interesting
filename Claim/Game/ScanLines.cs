using System.Windows;

namespace WeiDi.Game;

/// <summary>
/// 边框内扫动的 5 根扫描线。它们共用同一个运动中的交叉点（枢纽 hub），
/// 像一把转动的扇子：
///  - 枢纽在场地方阵内游走（碰内缩边界反弹、随机变向、避开已填色块）；
///  - 每根线都穿过枢纽，朝向 = 共享基准角 + 自己的槽位角 + 自己的独立摆动；
///  - 基准角持续旋转（整体一起转），每根线又有各自的摆动角速度（各转各的），
///    摆动被限幅、槽位把 5 根线收拢在 ≤60° 的窄扇面内 —— 既各有姿态又不并拢/发散，
///    且 4 个相邻夹角之和恒不超过 60°；
///  - 每根线取包含枢纽的那段空地弦，两端精确落在边框上，色块像墙一样挡住激光。
/// </summary>
internal sealed class ScanLines
{
    /// <summary>一根扫描线的可见弦：A、B 两端落在边框上；Angle 是它当前的朝向角。</summary>
    internal readonly struct Segment
    {
        public Segment(Point a, Point b, double angle)
        {
            A = a;
            B = b;
            Angle = angle;
        }

        public Point A { get; }
        public Point B { get; }
        public double Angle { get; }
    }

    private readonly Boundary _bound;
    private readonly PaintGrid _paint;
    private readonly Random _rng = new();
    private readonly Segment[] _seg = new Segment[GameConfig.ScanCount];

    // 每根线相对基准角的固定槽位（均分半圈）与独立摆动状态
    private readonly double[] _slot = new double[GameConfig.ScanCount];
    private readonly double[] _wobble = new double[GameConfig.ScanCount];
    private readonly double[] _wobbleRate = new double[GameConfig.ScanCount];
    private readonly int[] _wobbleSign = new int[GameConfig.ScanCount];

    private Point _hub;             // 公共交叉点
    private Point _hubVel;          // 交点移动方向（单位向量）
    private double _baseAngle;      // 所有线共享的基准朝向角
    private double _baseSpin;       // 基准角的旋转角速度（带符号）
    private double _changeTimer;
    private bool _frozenForTest;    // 自检碰撞用例：锁定交点，跳过运动/旋转

    public ScanLines(Boundary bound, PaintGrid paint)
    {
        _bound = bound;
        _paint = paint;
        Reset();
    }

    /// <summary>当前可见弦（下标 0..4）。</summary>
    public IReadOnlyList<Segment> Segments => _seg;

    /// <summary>当前公共交叉点（自检/诊断用）。</summary>
    public Point Hub => _hub;

    /// <summary>当前共享基准角（自检/诊断用）。</summary>
    public double BaseAngle => _baseAngle;

    public void Reset()
    {
        _frozenForTest = false;

        // 交点从左下附近起步，朝右上方向移动（屏幕 y 向下）
        _hub = new Point(Arena.S * 0.30, Arena.S * 0.70);
        double dir = -Math.PI / 4 + Rand(-GameConfig.ScanAngleJitter, GameConfig.ScanAngleJitter);
        _hubVel = new Point(Math.Cos(dir), Math.Sin(dir));

        _baseAngle = Rand(0, Math.PI * 2);
        _baseSpin = RandSpin();

        // 槽位居中均布在 [-2Δ, +2Δ]，相邻差 Δ。这样 4 个相邻夹角之和 =
        // 槽位总跨度(4Δ) + 首尾两线摆动的极端差(≤2×限幅)，恒 ≤ ScanFanTotal(60°)。
        int mid = (GameConfig.ScanCount - 1) / 2;
        for (int i = 0; i < GameConfig.ScanCount; i++)
        {
            _slot[i] = (i - mid) * GameConfig.ScanSlotStep;
            _wobble[i] = 0;
            _wobbleSign[i] = _rng.Next(2) == 0 ? -1 : 1;
            _wobbleRate[i] = Rand(GameConfig.ScanWobbleRateMin, GameConfig.ScanWobbleRateMax);
        }

        _changeTimer = Rand(GameConfig.ScanChangeMin, GameConfig.ScanChangeMax);
        Rebuild();
    }

    public void Update(double dt)
    {
        // 自检碰撞用例锁定了交点，此时只按当前姿态重建弦即可。
        if (_frozenForTest) { Rebuild(); return; }

        // 每隔一会儿随机换一次：整体转向、交点方向、各自摆动速率。
        _changeTimer -= dt;
        if (_changeTimer <= 0)
        {
            double d = Rand(0, Math.PI * 2);
            _hubVel = new Point(Math.Cos(d), Math.Sin(d));
            _baseSpin = RandSpin();
            for (int i = 0; i < GameConfig.ScanCount; i++)
            {
                _wobbleRate[i] = Rand(GameConfig.ScanWobbleRateMin, GameConfig.ScanWobbleRateMax);
                _wobbleSign[i] = _rng.Next(2) == 0 ? -1 : 1;
            }
            _changeTimer = Rand(GameConfig.ScanChangeMin, GameConfig.ScanChangeMax);
        }

        MoveHub(dt);

        // 整体旋转
        _baseAngle += _baseSpin * dt;

        // 每根线各自的摆动；到限幅就反弹，避免并拢/发散
        for (int i = 0; i < GameConfig.ScanCount; i++)
        {
            _wobble[i] += _wobbleSign[i] * _wobbleRate[i] * dt;
            if (_wobble[i] > GameConfig.ScanWobbleLimit)
            {
                _wobble[i] = GameConfig.ScanWobbleLimit;
                _wobbleSign[i] = -1;
            }
            else if (_wobble[i] < -GameConfig.ScanWobbleLimit)
            {
                _wobble[i] = -GameConfig.ScanWobbleLimit;
                _wobbleSign[i] = 1;
            }
        }

        Rebuild();
    }

    /// <summary>点 p 是否落在任一可见扫描线的命中半径内。</summary>
    public bool Hits(Point p, double radius)
    {
        double rr = radius * radius;
        for (int i = 0; i < _seg.Length; i++)
        {
            Segment s = _seg[i];
            if (Same(s.A, s.B)) continue;
            double d2 = DistToSegmentSq(p, s.A, s.B);
            if (d2 <= rr) return true;
        }
        return false;
    }

    /// <summary>
    /// 任一可见扫描线是否扫到运动轨迹折线（trail 相邻点连成的线段）。
    /// 线段相交、或两线段最短距离 ≤ radius 即判命中——这样圆点离开边框后，
    /// 扫描线扫过它已经画出的任何一段轨迹（不只是圆点当前位置）都会触发。
    /// </summary>
    public bool HitsTrail(IList<Point> trail, double radius)
    {
        if (trail == null || trail.Count < 2) return false;
        double rr = radius * radius;
        for (int i = 0; i < _seg.Length; i++)
        {
            Segment s = _seg[i];
            if (Same(s.A, s.B)) continue;
            for (int j = 1; j < trail.Count; j++)
            {
                Point p = trail[j - 1];
                Point q = trail[j];
                if (Same(p, q)) continue;
                if (SegSegDistSq(s.A, s.B, p, q) <= rr) return true;
            }
        }
        return false;
    }

    // ── 测试钩子（仅自检使用） ──────────────────────────────

    /// <summary>
    /// 把公共交点锁定到 p，使 5 根线都穿过 p，并冻结运动便于确定性碰撞测试。
    /// Reset 会清除冻结。
    /// </summary>
    internal void SetHubForTest(Point p)
    {
        _frozenForTest = true;
        _hub = p;
        Rebuild();
    }

    // ── 内部 ────────────────────────────────────────────────

    /// <summary>移动公共交点：内缩边界反弹，撞到已填色块则沿可走的方向滑动。</summary>
    private void MoveHub(double dt)
    {
        double m = GameConfig.ScanHubMargin;
        double lo = m;
        double hi = Arena.S - m;

        double nx = _hub.X + _hubVel.X * GameConfig.ScanHubSpeed * dt;
        double ny = _hub.Y + _hubVel.Y * GameConfig.ScanHubSpeed * dt;

        // 边界反弹
        if (nx < lo) { nx = lo; _hubVel = new Point(Math.Abs(_hubVel.X), _hubVel.Y); }
        else if (nx > hi) { nx = hi; _hubVel = new Point(-Math.Abs(_hubVel.X), _hubVel.Y); }
        if (ny < lo) { ny = lo; _hubVel = new Point(_hubVel.X, Math.Abs(_hubVel.Y)); }
        else if (ny > hi) { ny = hi; _hubVel = new Point(_hubVel.X, -Math.Abs(_hubVel.Y)); }

        // 已填色块当成墙：只取消撞墙的分量，沿墙滑行；两轴都被挡就换个方向。
        Point cand = new(nx, ny);
        if (!HubCellFree(cand))
        {
            Point slideX = new(nx, _hub.Y);
            Point slideY = new(_hub.X, ny);
            if (HubCellFree(slideX)) { cand = slideX; }
            else if (HubCellFree(slideY)) { cand = slideY; }
            else
            {
                cand = _hub;
                double d = Rand(0, Math.PI * 2);
                _hubVel = new Point(Math.Cos(d), Math.Sin(d));
            }
        }

        _hub = cand;
    }

    private bool HubCellFree(Point p)
    {
        if (p.X < 0 || p.X > Arena.S || p.Y < 0 || p.Y > Arena.S) return false;
        return _paint.IsFree(p);
    }

    /// <summary>为 5 根线各求一条穿过枢纽的空地弦，刷新可见端点与朝向角。</summary>
    private void Rebuild()
    {
        IReadOnlyList<Point> pts = _bound.Points;

        for (int i = 0; i < _seg.Length; i++)
        {
            double ang = _baseAngle + _slot[i] + _wobble[i];
            double lx = Math.Cos(ang);
            double ly = Math.Sin(ang);

            if (TryChordThroughHub(pts, _hub, lx, ly, out Point a, out Point b))
            {
                _seg[i] = new Segment(a, b, ang);
            }
            else
            {
                _seg[i] = new Segment(default, default, ang);
            }
        }
    }

    /// <summary>
    /// 求过枢纽 hub、方向为 L 的直线与边框折线相交后「包含 hub 的那一段空地弦」。
    ///
    /// 边框是空地的边界，围地后是凹多边形。直线与它相交会得到偶数个参数，
    /// 排序后 [t0,t1]、[t2,t3]… 交替对应空地内/外区间。hub 在空地里，
    /// 所以取包含 t=0 的那一段 —— 这样弦的两端都精确落在边框上，
    /// 且整段穿过 hub（5 根线因此严格共点），色块像墙一样把激光截断。
    /// </summary>
    private static bool TryChordThroughHub(IReadOnlyList<Point> pts, Point hub, double lx, double ly,
        out Point a, out Point b)
    {
        a = default;
        b = default;
        if (pts == null || pts.Count < 2) return false;

        var ts = new List<double>(pts.Count);
        int n = pts.Count;

        for (int i = 0; i < n; i++)
        {
            Point v = pts[i];
            Point w = pts[(i + 1) % n];
            double ex = w.X - v.X;
            double ey = w.Y - v.Y;
            double den = lx * ey - ly * ex;        // cross(L, e)
            if (Math.Abs(den) < 1e-9) continue;

            double wx = v.X - hub.X;
            double wy = v.Y - hub.Y;
            double t = (wx * ey - wy * ex) / den;  // 沿扫描线、相对 hub 的参数
            double u = (wx * ly - wy * lx) / den;  // 沿边的参数，须在 [0,1]

            if (u < -1e-9 || u > 1 + 1e-9) continue;
            ts.Add(t);
        }

        if (ts.Count < 2) return false;
        ts.Sort();

        var uniq = new List<double>(ts.Count);
        foreach (double t in ts)
        {
            if (uniq.Count == 0 || t - uniq[^1] > 1e-6) uniq.Add(t);
        }

        // 找包含 0 的区间；没有（数值极端情况）则退化为最长的一段。
        double bestLo = 0;
        double bestHi = 0;
        bool found = false;
        for (int k = 0; k + 1 < uniq.Count; k += 2)
        {
            if (uniq[k] <= 0 && 0 <= uniq[k + 1])
            {
                bestLo = uniq[k];
                bestHi = uniq[k + 1];
                found = true;
                break;
            }
        }

        if (!found)
        {
            double bestLen = 0;
            for (int k = 0; k + 1 < uniq.Count; k += 2)
            {
                double len = uniq[k + 1] - uniq[k];
                if (len > bestLen) { bestLen = len; bestLo = uniq[k]; bestHi = uniq[k + 1]; }
            }
            if (bestLen < 1e-6) return false;
        }

        a = new Point(hub.X + lx * bestLo, hub.Y + ly * bestLo);
        b = new Point(hub.X + lx * bestHi, hub.Y + ly * bestHi);
        return true;
    }

    /// <summary>
    /// 两条线段的最短距离平方。相交则为 0；否则取四个端点到对线段距离的最小值。
    /// 共线重叠时端点必落在另一条线段上，同样取到 0。
    /// </summary>
    private static double SegSegDistSq(Point a, Point b, Point c, Point d)
    {
        if (SegmentsIntersect(a, b, c, d)) return 0;
        double best = DistToSegmentSq(a, c, d);
        double v = DistToSegmentSq(b, c, d); if (v < best) best = v;
        v = DistToSegmentSq(c, a, b); if (v < best) best = v;
        v = DistToSegmentSq(d, a, b); if (v < best) best = v;
        return best;
    }

    /// <summary>标准的二维线段相交测试（含端点接触与共线）。</summary>
    private static bool SegmentsIntersect(Point a, Point b, Point c, Point d)
    {
        double c1 = Cross(a, b, c);
        double c2 = Cross(a, b, d);
        double c3 = Cross(c, d, a);
        double c4 = Cross(c, d, b);
        if (((c1 > 0 && c2 < 0) || (c1 < 0 && c2 > 0)) &&
            ((c3 > 0 && c4 < 0) || (c3 < 0 && c4 > 0))) return true;

        // 端点恰好落在另一条线段上（共线或 T 接触）
        if (Math.Abs(c1) < 1e-9 && OnSegment(a, b, c)) return true;
        if (Math.Abs(c2) < 1e-9 && OnSegment(a, b, d)) return true;
        if (Math.Abs(c3) < 1e-9 && OnSegment(c, d, a)) return true;
        if (Math.Abs(c4) < 1e-9 && OnSegment(c, d, b)) return true;
        return false;
    }

    private static double Cross(Point o, Point a, Point b) =>
        (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);

    private static bool OnSegment(Point a, Point b, Point p) =>
        p.X >= Math.Min(a.X, b.X) - 1e-9 && p.X <= Math.Max(a.X, b.X) + 1e-9 &&
        p.Y >= Math.Min(a.Y, b.Y) - 1e-9 && p.Y <= Math.Max(a.Y, b.Y) + 1e-9;

    private static double DistToSegmentSq(Point p, Point a, Point b)
    {
        double dx = b.X - a.X;
        double dy = b.Y - a.Y;
        double len2 = dx * dx + dy * dy;
        if (len2 < 1e-12)
        {
            double ex = p.X - a.X;
            double ey = p.Y - a.Y;
            return ex * ex + ey * ey;
        }

        double t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / len2;
        if (t < 0) t = 0;
        else if (t > 1) t = 1;
        double qx = a.X + dx * t - p.X;
        double qy = a.Y + dy * t - p.Y;
        return qx * qx + qy * qy;
    }

    private static bool Same(Point a, Point b) =>
        Math.Abs(a.X - b.X) < 1e-9 && Math.Abs(a.Y - b.Y) < 1e-9;

    private double RandSpin() =>
        (_rng.Next(2) == 0 ? -1 : 1) * Rand(GameConfig.ScanSpinMin, GameConfig.ScanSpinMax);

    private double Rand(double lo, double hi) => lo + _rng.NextDouble() * (hi - lo);
}
