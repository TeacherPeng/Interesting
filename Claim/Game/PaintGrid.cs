using System.Windows;
using System.Windows.Media;

namespace WeiDi.Game;

/// <summary>
/// 已围色块：矢量多边形留作绘制（边界永远贴着 1px 轨迹，不糊），
/// 栅格只用来做碰撞判定和面积统计。
/// </summary>
internal sealed class PaintGrid
{
    private readonly int _n = GameConfig.GridSize;
    private readonly byte[] _cells;
    private readonly List<PointCollection> _shapes = new();
    private double[] _xs = new double[1024];
    private int _painted;

    public PaintGrid()
    {
        _cells = new byte[_n * _n];
    }

    public IReadOnlyList<PointCollection> Shapes => _shapes;

    public double PaintedRatio => _cells.Length == 0 ? 0.0 : (double)_painted / _cells.Length;

    public int PaintedCells => _painted;

    public void Reset()
    {
        Array.Clear(_cells);
        _shapes.Clear();
        _painted = 0;
    }

    public bool IsPaintedAt(int index) => (uint)index < (uint)_cells.Length && _cells[index] != 0;

    public bool IsPainted(int cx, int cy)
    {
        if ((uint)cx >= (uint)_n || (uint)cy >= (uint)_n) return false;
        return _cells[cy * _n + cx] != 0;
    }

    /// <summary>该格是否仍是空地（场地外一律算「不是空地」）。</summary>
    public bool FreeAt(int cx, int cy)
    {
        if ((uint)cx >= (uint)_n || (uint)cy >= (uint)_n) return false;
        return _cells[cy * _n + cx] == 0;
    }

    public bool IsFree(Point p) => FreeAt((int)Math.Floor(p.X), (int)Math.Floor(p.Y));

    /// <summary>记一块新围地：栅格化多边形 + 把轨迹本身也纳入已围地。</summary>
    public int AddPolygon(List<Point> poly, IReadOnlyList<Point> trail)
    {
        int added = Rasterize(poly);
        added += MarkTrailCells(trail);
        _shapes.Add(new PointCollection(poly));
        return added;
    }

    /// <summary>扫描线（even-odd）栅格化，按格心采样。</summary>
    private int Rasterize(List<Point> poly)
    {
        if (poly.Count < 3) return 0;
        if (_xs.Length < poly.Count) _xs = new double[poly.Count];

        int n = _n;
        int added = 0;

        for (int row = 0; row < n; row++)
        {
            double yc = row + 0.5;
            int cnt = 0;
            for (int i = 0; i < poly.Count; i++)
            {
                Point a = poly[i];
                Point b = poly[(i + 1) % poly.Count];
                if ((a.Y <= yc && b.Y > yc) || (b.Y <= yc && a.Y > yc))
                {
                    double t = (yc - a.Y) / (b.Y - a.Y);
                    _xs[cnt++] = a.X + t * (b.X - a.X);
                }
            }
            if (cnt < 2) continue;

            Array.Sort(_xs, 0, cnt);

            int rowBase = row * n;
            for (int k = 0; k + 1 < cnt; k += 2)
            {
                double x0 = _xs[k];
                double x1 = _xs[k + 1];
                int c0 = (int)Math.Ceiling(x0 - 0.5);
                int c1 = (int)Math.Floor(x1 - 0.5);
                if (c0 < 0) c0 = 0;
                if (c1 > n - 1) c1 = n - 1;
                for (int cx = c0; cx <= c1; cx++)
                {
                    int idx = rowBase + cx;
                    if (_cells[idx] == 0)
                    {
                        _cells[idx] = 1;
                        added++;
                    }
                }
            }
        }

        _painted += added;
        return added;
    }

    private int MarkTrailCells(IReadOnlyList<Point> trail)
    {
        int added = 0;
        for (int i = 1; i < trail.Count; i++)
        {
            added += MarkLine(trail[i - 1], trail[i]);
        }
        _painted += added;
        return added;
    }

    private int MarkLine(Point a, Point b)
    {
        int x0 = Clamp((int)Math.Floor(a.X));
        int y0 = Clamp((int)Math.Floor(a.Y));
        int x1 = Clamp((int)Math.Floor(b.X));
        int y1 = Clamp((int)Math.Floor(b.Y));

        int dx = Math.Abs(x1 - x0);
        int sx = x0 < x1 ? 1 : -1;
        int dy = -Math.Abs(y1 - y0);
        int sy = y0 < y1 ? 1 : -1;
        int err = dx + dy;
        int added = 0;

        while (true)
        {
            int idx = y0 * _n + x0;
            if (_cells[idx] == 0)
            {
                _cells[idx] = 1;
                added++;
            }
            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 >= dy)
            {
                err += dy;
                x0 += sx;
            }
            if (e2 <= dx)
            {
                err += dx;
                y0 += sy;
            }
        }

        return added;
    }

    private int Clamp(int v) => v < 0 ? 0 : (v > _n - 1 ? _n - 1 : v);

    public static Geometry ToGeometry(IList<Point> pts)
    {
        var geo = new StreamGeometry();
        using (var ctx = geo.Open())
        {
            ctx.BeginFigure(pts[0], true, true);
            var rest = new List<Point>(pts.Count - 1);
            for (int i = 1; i < pts.Count; i++) rest.Add(pts[i]);
            ctx.PolyLineTo(rest, true, false);
        }
        geo.Freeze();
        return geo;
    }
}
