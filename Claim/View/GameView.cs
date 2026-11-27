using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WeiDi.Game;

namespace WeiDi.View;

/// <summary>
/// 全部画面都由这里一次 OnRender 画完：场地、已围色块、1px 边框、1px 轨迹、圆点、HUD。
/// 只读 GameSession 的快照，不反过来改状态。
/// </summary>
public sealed class GameView : FrameworkElement
{
    private const double Pad = 26.0;

    private static readonly Typeface UiFont = new("Microsoft YaHei UI");
    private static readonly Typeface MonoFont = new("Consolas");

    private readonly GameSession _session = new();
    private readonly Stopwatch _clock = new();
    private readonly HashSet<Key> _down = new();

    private long _lastTicks;
    private double _accumulator;
    private bool _blurPaused;
    private bool _manualPaused;

    private Geometry? _trailGeometry;
    private int _trailGeometryCount = -1;

    private Geometry? _paintGeometry;
    private int _shapeCount = -1;

    private Geometry? _boundaryGeometry;
    private int _boundaryVersion = -1;

    public GameView()
    {
        Focusable = true;
        ClipToBounds = true;
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    /// <summary>离线截图/自检用：只读会话，便于不弹窗直接驱动。</summary>
    internal GameSession Session => _session;

    // ── 生命周期 ──────────────────────────────────────────────

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _clock.Restart();
        _lastTicks = _clock.ElapsedTicks;
        _accumulator = 0;
        CompositionTarget.Rendering += OnFrame;
        Focus();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        CompositionTarget.Rendering -= OnFrame;
        _clock.Stop();
    }

    private void OnFrame(object? sender, EventArgs e)
    {
        long now = _clock.ElapsedTicks;
        double dt = (now - _lastTicks) / (double)Stopwatch.Frequency;
        _lastTicks = now;

        if (dt < 0) dt = 0;
        if (dt > GameConfig.MaxFrameDt) dt = GameConfig.MaxFrameDt;   // 丢帧/失焦恢复后不追帧

        if (_session.Paused || _session.Won || _session.Dead)
        {
            _accumulator = 0;
        }
        else
        {
            _accumulator += dt;
            int guard = 0;
            while (_accumulator >= GameConfig.FixedStep && guard++ < 480)
            {
                _session.Step(GameConfig.FixedStep, Keys());
                _accumulator -= GameConfig.FixedStep;
            }
        }

        InvalidateVisual();
    }

    // ── 输入 ──────────────────────────────────────────────────

    public bool HandleKeyDown(Key key)
    {
        switch (key)
        {
            case Key.Escape:
                _manualPaused = !_manualPaused;
                SyncPause();
                return true;

            case Key.R:
                _manualPaused = false;
                _session.Reset();
                SyncPause();
                return true;

            case Key.Enter:
                if (_session.Won || _session.Dead)
                {
                    _manualPaused = false;
                    _session.Reset();
                    SyncPause();
                }
                return true;

            case Key.Left:
            case Key.Right:
            case Key.Up:
            case Key.Down:
            case Key.A:
            case Key.D:
            case Key.W:
            case Key.S:
                _down.Add(key);
                return true;
        }

        return false;
    }

    public void HandleKeyUp(Key key)
    {
        _down.Remove(key);
    }

    public void OnWindowBlur(bool blurred)
    {
        if (blurred)
        {
            _down.Clear();          // 失焦后 KeyUp 可能收不到，别让按键卡住
            _blurPaused = true;
        }
        else
        {
            _blurPaused = false;
            _lastTicks = _clock.ElapsedTicks;
            _accumulator = 0;
        }

        SyncPause();
    }

    private void SyncPause() => _session.Paused = _blurPaused || _manualPaused;

    private Point Keys()
    {
        double x = 0;
        double y = 0;
        if (_down.Contains(Key.Left) || _down.Contains(Key.A)) x -= 1;
        if (_down.Contains(Key.Right) || _down.Contains(Key.D)) x += 1;
        if (_down.Contains(Key.Up) || _down.Contains(Key.W)) y -= 1;
        if (_down.Contains(Key.Down) || _down.Contains(Key.S)) y += 1;

        // 归一化成单位向量：斜向两键同时按下时模长本为 √2，会让切入阈值失效。
        // Step 内部也会归一化（双保险，直接调用 Step 的测试同样受益）。
        double len = Math.Sqrt(x * x + y * y);
        if (len > 1e-9) return new Point(x / len, y / len);
        return new Point(0, 0);
    }

    // ── 绘制 ──────────────────────────────────────────────────

    protected override void OnRender(DrawingContext dc)
    {
        double w = ActualWidth;
        double h = ActualHeight;
        if (w <= 0 || h <= 0) return;

        dc.DrawRectangle(Palette.Background, null, new Rect(0, 0, w, h));

        double side = Math.Min(w, h) - Pad * 2;
        if (side < 160) side = 160;
        double ox = Math.Round((w - side) / 2);
        double oy = Math.Round((h - side) / 2);
        double k = side / Arena.S;
        double hair = 1.0 / k;                     // 1 设备像素折回逻辑单位

        Rect field = new(ox, oy, side, side);
        dc.DrawRectangle(Palette.FieldFill, null, field);

        // 半像素对齐：逻辑 0 落在设备像素中心，1px 描边才不会骑在两条像素之间被摊成半透明
        dc.PushTransform(new MatrixTransform(k, 0, 0, k, ox + 0.5, oy + 0.5));

        // 已围色块（矢量，边界贴着 1px 轨迹）
        // 不描边：色块之间若相邻会留下一条假「边界」线，让玩家误以为那里还能走；
        // 色块面向空地的那条外沿，本来就由下面的边框线负责画。
        SyncShapeGeometries();
        if (_paintGeometry != null)
        {
            dc.DrawGeometry(Palette.PaintedFill, null, _paintGeometry);
        }

        // 1px 边框（＝空地边界：正方形四边 + 已围出的轨迹，轨迹回到边框后成为新边框）
        SyncBoundaryGeometry();
        Pen borderPen = new(Palette.Border, hair);
        if (_boundaryGeometry != null)
        {
            dc.DrawGeometry(null, borderPen, _boundaryGeometry);
        }

        // 5 根扫描线（两端接触边框，整束扫动；离开边框后扫到轨迹即死）
        DrawScanLines(dc, hair);

        // 1px 轨迹（只画线，不填色；回到边框才结算填色）
        DrawTrail(dc, hair);

        // 圆点
        DrawDot(dc, hair);

        dc.Pop();

        DrawHud(dc, field);
    }

    /// <summary>
    /// 把所有已围色块合并成一条 PathGeometry 一次性填充：相邻两块共享同一条边，
    /// 分开画时那条缝会被两次半透明叠加各盖一半，留下一条比别处暗的接缝线。
    /// </summary>
    private void SyncShapeGeometries()
    {
        IReadOnlyList<PointCollection> shapes = _session.Paint.Shapes;
        if (_shapeCount == shapes.Count) return;

        var path = new PathGeometry { FillRule = FillRule.Nonzero };
        for (int i = 0; i < shapes.Count; i++)
        {
            PointCollection shape = shapes[i];
            if (shape.Count < 3) continue;

            var figure = new PathFigure
            {
                StartPoint = shape[0],
                IsClosed = true,
                IsFilled = true,
            };
            var rest = new List<Point>(shape.Count - 1);
            for (int j = 1; j < shape.Count; j++) rest.Add(shape[j]);
            figure.Segments.Add(new PolyLineSegment(rest, false));
            path.Figures.Add(figure);
        }
        path.Freeze();

        _paintGeometry = path.Figures.Count > 0 ? path : null;
        _shapeCount = shapes.Count;
    }

    private void SyncBoundaryGeometry()
    {
        int version = _session.Bound.Version;
        if (version == _boundaryVersion) return;

        _boundaryGeometry = BuildPolyline(_session.Bound.Points, true);
        _boundaryVersion = version;
    }

    /// <summary>
    /// 折线 → 几何。closed 时首尾相接（边框用），否则是开口线（轨迹用）。
    /// 轨迹一定不能闭合：起笔点和当前点在同一条直线上，闭合的末边会 180° 反向，
    /// 斜接会在笔帽外面戳出一截尖刺。
    /// </summary>
    private static Geometry? BuildPolyline(IEnumerable<Point> pts, bool closed)
    {
        var points = pts as List<Point> ?? new List<Point>(pts);
        if (points.Count < 2) return null;

        var geo = new StreamGeometry();
        using (StreamGeometryContext ctx = geo.Open())
        {
            ctx.BeginFigure(points[0], false, closed);
            var rest = new List<Point>(points.Count - 1);
            for (int i = 1; i < points.Count; i++) rest.Add(points[i]);
            ctx.PolyLineTo(rest, true, false);
        }
        geo.Freeze();
        return geo;
    }

    /// <summary>
    /// 5 根扫描线：每根是与边框的一条弦（两端接触边框）。柔光 + 亮芯两层，
    /// 让它看起来更像扫过的光束而不是普通边线。
    /// </summary>
    private void DrawScanLines(DrawingContext dc, double hair)
    {
        IReadOnlyList<ScanLines.Segment> segs = _session.Scanner.Segments;
        if (segs.Count == 0) return;

        var glowPen = new Pen(Palette.ScanGlow, hair * 9)
        {
            StartLineCap = PenLineCap.Round,
            EndLineCap = PenLineCap.Round,
        };
        var corePen = new Pen(Palette.ScanLine, hair * 2)
        {
            StartLineCap = PenLineCap.Round,
            EndLineCap = PenLineCap.Round,
        };

        for (int i = 0; i < segs.Count; i++)
        {
            ScanLines.Segment s = segs[i];
            if (Math.Abs(s.A.X - s.B.X) < 1e-9 && Math.Abs(s.A.Y - s.B.Y) < 1e-9) continue;

            var geo = new StreamGeometry();
            using (StreamGeometryContext ctx = geo.Open())
            {
                ctx.BeginFigure(s.A, false, false);
                ctx.LineTo(s.B, true, false);
            }
            geo.Freeze();

            dc.DrawGeometry(null, glowPen, geo);
            dc.DrawGeometry(null, corePen, geo);
        }
    }

    private void DrawTrail(DrawingContext dc, double hair)
    {
        IList<Point> trail = _session.Trail;
        if (_session.Mode != GameMode.Field || trail.Count < 2) return;

        if (_trailGeometryCount != trail.Count)
        {
            _trailGeometry = BuildPolyline(trail, false);
            _trailGeometryCount = trail.Count;
        }

        if (_trailGeometry == null) return;

        var glowPen = new Pen(Palette.TrailGlow, hair * 5)
        {
            StartLineCap = PenLineCap.Flat,
            EndLineCap = PenLineCap.Flat,
        };
        var linePen = new Pen(Palette.Trail, hair)
        {
            StartLineCap = PenLineCap.Flat,
            EndLineCap = PenLineCap.Flat,
        };

        dc.DrawGeometry(null, glowPen, _trailGeometry);
        dc.DrawGeometry(null, linePen, _trailGeometry);
    }

    private void DrawDot(DrawingContext dc, double hair)
    {
        Point p = _session.Position;
        double r = GameConfig.DotRadius;

        dc.DrawEllipse(Palette.DotGlow, null, p, r * 3.2, r * 3.2);
        dc.DrawEllipse(Palette.DotFill, new Pen(Palette.DotRing, hair), p, r, r);
    }

    // ── HUD ───────────────────────────────────────────────────

    private void DrawHud(DrawingContext dc, Rect field)
    {
        double dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;
        int percent = (int)Math.Round(_session.PaintedRatio * 100);
        string mode = _session.Mode == GameMode.Frame ? "边框" : "绘制中";

        FormattedText top = Text($"已围 {percent}%      {mode}", MonoFont, 14, Palette.HudStrong, dpi);
        dc.DrawText(top, new Point(field.X + 14, field.Y + 10));

        FormattedText bottom = Text("方向键 控向   ·   向内切进区域   ·   当心自碰轨迹与扫描线   ·   R 重开   ·   Esc 暂停",
            UiFont, 12, Palette.HudDim, dpi);
        dc.DrawText(bottom, new Point(field.X + 14, field.Y + field.Height - 26));

        if (_session.Dead)
        {
            DrawCenterText(dc, field, dpi, "轨迹碰撞", "碰到了自己的轨迹或被扫描线击中   ·   Enter / R 重开");
        }
        else if (_session.Paused)
        {
            DrawCenterText(dc, field, dpi, "已暂停", "Esc 继续");
        }
        else if (_session.Won)
        {
            DrawCenterText(dc, field, dpi, "围地完成", $"已围 {percent}%   ·   Enter / R 重开");
        }
        else if (_session.NoEntry && _session.Mode == GameMode.Frame)
        {
            DrawCenterText(dc, field, dpi, "无处可入", $"已围 {percent}%   ·   边框已被围死，R 重开");
        }
    }

    private static void DrawCenterText(DrawingContext dc, Rect field, double dpi, string title, string hint)
    {
        dc.DrawRectangle(Palette.Overlay, null, field);

        FormattedText t1 = Text(title, UiFont, 34, Palette.HudStrong, dpi, bold: true);
        FormattedText t2 = Text(hint, UiFont, 14, Palette.HudText, dpi);

        double cx = field.X + field.Width / 2;
        double cy = field.Y + field.Height / 2;
        dc.DrawText(t1, new Point(cx - t1.Width / 2, cy - t1.Height - 8));
        dc.DrawText(t2, new Point(cx - t2.Width / 2, cy + 8));
    }

    private static FormattedText Text(string s, Typeface face, double size, Brush brush, double dpi, bool bold = false)
    {
        return new FormattedText(
            s,
            CultureInfo.CurrentUICulture,
            FlowDirection.LeftToRight,
            bold ? new Typeface(face.FontFamily, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal) : face,
            size,
            brush,
            dpi);
    }
}
