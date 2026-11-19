using System.Windows;

namespace WeiDi.Game;

internal enum GameMode
{
    /// <summary>圆心压在边框线上，沿边线巡航。</summary>
    Frame,

    /// <summary>圆心在空地内部自由移动，正在拖轨迹。</summary>
    Field,
}

/// <summary>
/// 一局游戏的全部状态与推进逻辑。不碰任何绘制对象，只产出可读快照。
///
/// 规则要点：
/// 1. 离开边框后只画轨迹线，不填任何区域；
/// 2. 轨迹回到边框才结算，围出的那块填色；
/// 3. 这条轨迹随即成为新的边框 —— 圆点可以沿它走，也可以从它再切入空地。
/// </summary>
internal sealed class GameSession
{
    private readonly List<Point> _trail = new();

    private GameMode _mode = GameMode.Frame;
    private double _s;
    private int _sign = 1;
    private int _signBeforeDive = 1;
    private double _sAtDive;
    private Point _pos;
    private Point _dir = new(1, 0);
    private Point _escapeDir = new(1, 0);
    private double _escapeLeft;
    private Point _keys;
    private double _trailLength;
    private bool _won;
    private bool _noEntry;
    private bool _dead;

    public GameSession()
    {
        Paint = new PaintGrid();
        Bound = new Boundary();
        Scanner = new ScanLines(Bound, Paint);
        Reset();
    }

    public PaintGrid Paint { get; }

    /// <summary>当前边框（空地边界）：初始是正方形，围地后含轨迹。</summary>
    public Boundary Bound { get; }

    /// <summary>在边框内扫动的 5 根扫描线。</summary>
    public ScanLines Scanner { get; }

    public GameMode Mode => _mode;

    public Point Position => _pos;

    /// <summary>圆心在边框上的弧长位置（只在 Frame 模式有意义）。</summary>
    public double ArcPosition => _s;

    public IList<Point> Trail => _trail;

    public bool Paused { get; set; }

    public bool Won => _won;

    /// <summary>边框上已无任何可切入的空地（被围死），只能重开。</summary>
    public bool NoEntry => _noEntry;

    /// <summary>圆点离开边框、尚未回框时被扫描线击中 → 游戏结束。</summary>
    public bool Dead => _dead;

    /// <summary>
    /// 扫描线是否致命。正常游玩恒为 true；自检/截图里关掉它，
    /// 免得随机扫动的扫描线干扰既有的围地规则验证与画面核对。
    /// 扫描线本身的扫动与绘制不受此开关影响。
    /// </summary>
    public bool ScanLethal { get; set; } = true;

    public double PaintedRatio => Paint.PaintedRatio;

    /// <summary>已拖出的轨迹总长（自检/诊断用）。</summary>
    internal double TrailLength => _trailLength;

    public void Reset()
    {
        Paint.Reset();
        Bound.Reset();
        Scanner.Reset();
        _trail.Clear();
        _mode = GameMode.Frame;
        _s = Arena.S / 2;                 // 上边正中间起步
        _sign = 1;                        // 顺时针
        _signBeforeDive = 1;
        _sAtDive = _s;
        _pos = Bound.PointAt(_s);
        _dir = Bound.TangentAt(_s);
        _trailLength = 0;
        _keys = default;
        _won = false;
        _noEntry = false;
        _dead = false;
        Paused = false;
    }

    public void Step(double dt, Point keys)
    {
        // 把方向键统一成单位向量：单键模长 1、双键斜向原本是 √2。
        // 若不归一化，StepFrame 里 inward=keys·n 与固定阈值 DiveThreshold 比较时，
        // 斜键只要 cos>0.35/√2≈0.247 就能切入，比单键(0.35)更宽松——同一位置
        // 单键/斜键手感不一致。统一到单位向量后，阈值对所有按键组合语义一致。
        double keyLen = Math.Sqrt(keys.X * keys.X + keys.Y * keys.Y);
        if (keyLen > 1e-9) keys = new Point(keys.X / keyLen, keys.Y / keyLen);

        // 再把方向吸附到最近的坐标轴：本作轨迹只允许水平/垂直，绝不出现斜线。
        // 双键(如右+下)取 X、Y 中较大的那个轴；相等时取水平，保证确定性。
        keys = SnapToAxis(keys);
        _keys = keys;
        if (_won || _dead || Paused) return;

        // 扫描线始终在扫动（边框上巡航时也扫，营造持续威胁）。
        Scanner.Update(dt);

        if (_mode == GameMode.Frame)
        {
            StepFrame(dt, keys);
        }
        else
        {
            StepField(dt, keys);
        }

        // 碰撞判定只在「离开边框、尚未回框」时生效：即 Field 模式。
        // 一旦这一子步真的从 Field 回框（CloseLoop 把 _mode 切回 Frame），就不算被击中。
        // 判定对象是「圆点已画出的整条轨迹」：扫描线扫过轨迹的任何一段即游戏结束，
        // 而不只是压到圆点当前位置。起笔点在边框上，也算轨迹的一部分。
        if (ScanLethal && _mode == GameMode.Field && Scanner.HitsTrail(_trail, GameConfig.ScanTrailHitRadius))
        {
            _dead = true;
        }
    }

    // ── 边框上 ────────────────────────────────────────────────

    private void StepFrame(double dt, Point keys)
    {
        Point t = Bound.TangentAt(_s);
        Point n = Bound.InwardAt(_s, Paint);

        double keyTangential = keys.X * t.X + keys.Y * t.Y;
        double inward = keys.X * n.X + keys.Y * n.Y;

        // 想往里切：按键朝空地这一侧就够了，不要求和切向分量比大小
        //（比大小的话，斜向边框段上单按一个键两分量各 0.707，会被永远判成沿边走）。
        if (inward > GameConfig.DiveThreshold)
        {
            // 切入方向就用「玩家实际按的那几个键」：按右就该往右走线。
            // 正前方被刚才画的那条轨迹线挡住时，再在附近偏几度绕出去。
            // keys 已在 Step 入口归一化，直接就是单位向量。
            Point want = keys;
            if (TryDiveDirection(want, out Point diveDir))
            {
                StartDive(diveDir, want);
                return;
            }
        }

        // 其余一律沿边框走。切不进去也照走，绝不原地顶死。
        if (Math.Abs(keyTangential) > 1e-6)
        {
            _sign = keyTangential > 0 ? 1 : -1;
        }

        _s = Bound.Wrap(_s + _sign * GameConfig.Speed * dt);
        _pos = Bound.PointAt(_s);
        _dir = new Point(t.X * _sign, t.Y * _sign);
    }

    /// <summary>沿这个方向往里，够不够一整段（起笔保护那么长）空地。</summary>
    private bool PathClear(Point dir)
    {
        for (double d = 2; d <= GameConfig.DiveClearance; d += 2)
        {
            Point p = new(_pos.X + dir.X * d, _pos.Y + dir.Y * d);
            if (!Arena.Inside(p) || !Paint.IsFree(p)) return false;
        }
        return true;
    }

    private static readonly Point[] AxisDirs =
    {
        new(1, 0), new(0, 1), new(-1, 0), new(0, -1),
    };

    /// <summary>把任意方向吸附到最近的坐标轴（水平或垂直）；零向量保持为零。</summary>
    private static Point SnapToAxis(Point v)
    {
        if (Math.Abs(v.X) < 1e-9 && Math.Abs(v.Y) < 1e-9) return default;
        if (Math.Abs(v.X) >= Math.Abs(v.Y))
            return new Point(v.X >= 0 ? 1 : -1, 0);
        return new Point(0, v.Y >= 0 ? 1 : -1);
    }

    /// <summary>
    /// 优先沿玩家按的那个轴向直切；被挡住就依次尝试其余三个轴向。
    /// 全程只用 4 个轴向（绝不偏斜角），保证切入的第一段轨迹也是水平或垂直。
    /// 回框点拐角上法向正好顺着刚画的轨迹线时，玩家换个轴向键即可切出。
    /// </summary>
    internal bool TryDiveDirection(Point preferred, out Point dir)
    {
        dir = preferred;
        Point p = SnapToAxis(preferred);
        if (Math.Abs(p.X) < 1e-9 && Math.Abs(p.Y) < 1e-9) return false;

        if (PathClear(p)) { dir = p; return true; }
        foreach (Point d in AxisDirs)
        {
            if (Math.Abs(d.X - p.X) < 1e-9 && Math.Abs(d.Y - p.Y) < 1e-9) continue;
            if (PathClear(d)) { dir = d; return true; }
        }
        return false;
    }

    /// <summary>起笔通道内按纯反向键：撤销这次切入，退回边框原位（还没真正画出轨迹）。</summary>
    private void CancelDive()
    {
        _mode = GameMode.Frame;
        _s = _sAtDive;
        _sign = _signBeforeDive;
        _pos = Bound.PointAt(_s);
        Point t = Bound.TangentAt(_s);
        _dir = new Point(t.X * _sign, t.Y * _sign);
        _trail.Clear();
        _trailLength = 0;
        _escapeLeft = 0;
    }

    private void StartDive(Point inward, Point want)
    {
        _mode = GameMode.Field;
        _signBeforeDive = _sign;
        _sAtDive = _s;
        _pos = Bound.PointAt(_s);         // 起笔点精确落在边框上
        _dir = inward;
        _trail.Clear();
        _trail.Add(_pos);
        _trailLength = 0;

        // 起笔点必然压在边框线上，而轨迹型边框那一侧的格子是已涂的。
        // 若切入后立即套用 BlockRadius 贴障碍判定，第一步就会被起笔点旁的轨迹格判成贴障碍、
        // 原地顶死（表现为「按了方向键却什么也没画出来」）。
        // 所以先沿切入方向（若被迫偏转也用它）走出 DiveClearance 这么长一段「起笔通道」，
        // 冲离起笔轨迹格之后再交还按键与常规碰撞判定。
        // 注意：这里不能只在「方向被迫偏转」时才设 escape —— 法向直切同样贴着起笔轨迹格，
        // 一样会卡死。恒定启用才能让所有切入都先冲出起笔区。
        _escapeDir = inward;
        _escapeLeft = GameConfig.DiveClearance;
    }

    // ── 空地里 ────────────────────────────────────────────────

    private void StepField(double dt, Point keys)
    {
        double step = GameConfig.Speed * dt;
        Point dirBefore = _dir;

        // 起笔通道段：此刻圆心还贴着起笔点的轨迹格，常规贴障碍判定会把它原地顶死。
        // 这段专门用来冲离起笔区，期间按 escapeDir 直走，并接受玩家按键的微调
        //（只有按键朝向与 escapeDir 同侧时才叠加，朝里顶回障碍的方向则忽略），
        // 让玩家在切入瞬间依然能感到方向键在响应，而不是完全被锁住。
        bool escaping = _escapeLeft > 0;
        if (escaping)
        {
            // 起笔通道内按「纯反向键」= 玩家想撤回这一笔：退回边框原位，不算掉转。
            if (keys.X != 0 || keys.Y != 0)
            {
                double align = keys.X * _escapeDir.X + keys.Y * _escapeDir.Y;
                if (align < -0.9) { CancelDive(); return; }
            }
            // 起笔方向锁定为轴向，不再按 0.5 混合按键——那是斜向的来源。
            // 同向键本来就保持这个方向，垂直键本就被忽略，行为等价且绝不产生斜线。
            _dir = _escapeDir;
            _escapeLeft -= step;
        }
        else if (keys.X != 0 || keys.Y != 0)
        {
            // keys 已在 Step 入口归一化成单位向量，直接用即可。
            // 未封闭轨迹上不允许掉转：按键带「往回走」分量（与当前朝向成钝角，点积 < 0）时忽略，
            // 圆点保持朝向继续画，直到轨迹回到边框结算，不会沿刚画出的轨迹原路倒回去。
            // 直角/锐角转向（点积 ≥ 0）照常允许——那是合法的绕行围地，不算掉头。
            //（边框上不分这个——StepFrame 里按反向键可以正常掉头。）
            double forward = keys.X * _dir.X + keys.Y * _dir.Y;
            if (forward >= -1e-9) _dir = keys;
        }

        double nx = _pos.X + _dir.X * step;
        double ny = _pos.Y + _dir.Y * step;

        bool hitArenaEdge = false;
        if (nx < 0) { nx = 0; hitArenaEdge = true; }
        else if (nx > Arena.S) { nx = Arena.S; hitArenaEdge = true; }
        if (ny < 0) { ny = 0; hitArenaEdge = true; }
        else if (ny > Arena.S) { ny = Arena.S; hitArenaEdge = true; }

        Point candidate = new(nx, ny);
        _trailLength += Distance(_pos, candidate);

        // 本步就要贴上边线/色块，且已经离起笔点足够远 → 轨迹回到边框，结算
        // 起笔通道段豁免：它正贴着起笔轨迹格冲出，不能把那当成「回框」。
        if (!escaping && _trailLength >= GameConfig.FirstCaptureGrace
            && (hitArenaEdge || NearPainted(candidate, GameConfig.ContactGap)))
        {
            _pos = candidate;
            CloseLoop(Bound.Project(_pos));
            return;
        }

        Point moved = candidate;
        // 起笔通道段同样豁免贴障碍滑行，否则会在起笔轨迹格旁原地顶死。
        if (!escaping && !hitArenaEdge && NearPainted(candidate, GameConfig.BlockRadius))
        {
            // 只取消指向障碍的那个分量 → 自然沿墙滑行
            Point slideAlongY = new(_pos.X, ny);
            Point slideAlongX = new(nx, _pos.Y);
            if (!NearPainted(slideAlongY, GameConfig.BlockRadius)) moved = slideAlongY;
            else if (!NearPainted(slideAlongX, GameConfig.BlockRadius)) moved = slideAlongX;
            else moved = _pos;
        }

        // 转向那一子步，先把转向前的位置强制记为拐角点。否则抽稀会跨过拐角：
        // 垂直段末端不足一个抽稀步长、水平段首步又够长时，两个记录点会被连成一条
        //「假斜线」——使轨迹看起来出现了本不该有的斜向段。
        if (!escaping && (Math.Abs(dirBefore.X - _dir.X) > 1e-9 || Math.Abs(dirBefore.Y - _dir.Y) > 1e-9))
        {
            AppendTrail(_pos, true);
        }

        _pos = moved;
        AppendTrail(_pos, false);
    }

    private void CloseLoop(double sB)
    {
        _pos = Bound.PointAt(sB);
        AppendTrail(_pos, true);

        double sA = _sAtDive;
        List<Point> poly = Bound.Capture(sA, sB, _trail, out List<Point> newLoop);
        double area = Math.Abs(Shoelace(poly));

        bool valid = poly.Count >= 3 && area >= GameConfig.MinCaptureArea;

        if (valid)
        {
            // 轨迹成为新边框，围出的那块填色
            Bound.Apply(newLoop);
            Paint.AddPolygon(poly, _trail);
            _s = 0;                        // 新边框从回框点起算，前进方向＝切入前的方向
        }
        else
        {
            // 手势太小：不填色，边框保持不变，回原位继续走
            _s = sB;
        }

        // 回框后的走向：速度投影到切向，其次看按住的键，最后沿用切入前的周向
        Point t = Bound.TangentAt(_s);
        Point dir = new Point(_dir.X, _dir.Y);
        double tangential = dir.X * t.X + dir.Y * t.Y;

        if (Math.Abs(tangential) >= GameConfig.TangentialEpsilon)
        {
            _sign = tangential > 0 ? 1 : -1;
        }
        else
        {
            double keyTangential = _keys.X * t.X + _keys.Y * t.Y;
            if (keyTangential != 0) _sign = keyTangential > 0 ? 1 : -1;
            else _sign = _signBeforeDive;
        }

        _mode = GameMode.Frame;
        _pos = Bound.PointAt(_s);
        _dir = new Point(t.X * _sign, t.Y * _sign);
        _trail.Clear();
        _trailLength = 0;

        CheckWin();
    }

    // ── 轨迹 ──────────────────────────────────────────────────

    private void AppendTrail(Point p, bool force)
    {
        if (_trail.Count > 0)
        {
            Point last = _trail[^1];
            double dx = p.X - last.X;
            double dy = p.Y - last.Y;
            double d2 = dx * dx + dy * dy;
            if (d2 < 0.0001) return;
            if (!force && d2 < GameConfig.TrailDecimate * GameConfig.TrailDecimate) return;
        }

        _trail.Add(p);
    }

    private int ClampCell(int v) => v < 0 ? 0 : (v > Arena.S - 1 ? (int)Arena.S - 1 : v);

    /// <summary>圆点圆心周围 r 内是否有已围色块。</summary>
    private bool NearPainted(Point p, double r)
    {
        int n = GameConfig.GridSize;
        int x0 = ClampCell((int)Math.Floor(p.X - r));
        int x1 = ClampCell((int)Math.Ceiling(p.X + r));
        int y0 = ClampCell((int)Math.Floor(p.Y - r));
        int y1 = ClampCell((int)Math.Ceiling(p.Y + r));
        double rr = r * r;

        for (int cy = y0; cy <= y1; cy++)
        {
            int rowBase = cy * n;
            double dy = cy + 0.5 - p.Y;
            for (int cx = x0; cx <= x1; cx++)
            {
                double dx = cx + 0.5 - p.X;
                if (dx * dx + dy * dy > rr) continue;
                if (Paint.IsPaintedAt(rowBase + cx)) return true;
            }
        }

        return false;
    }

    private static double Distance(Point a, Point b)
    {
        double dx = b.X - a.X;
        double dy = b.Y - a.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private static double Shoelace(IReadOnlyList<Point> poly)
    {
        double sum = 0;
        for (int i = 0; i < poly.Count; i++)
        {
            Point a = poly[i];
            Point b = poly[(i + 1) % poly.Count];
            sum += a.X * b.Y - b.X * a.Y;
        }
        return sum * 0.5;
    }

    // ── 通关 ──────────────────────────────────────────────────

    private void CheckWin()
    {
        // 只按面积判胜：边框被围满但仍留大片空洞不该算通关
        if (Paint.PaintedRatio >= 1 - GameConfig.WinRemainRatio) _won = true;
        _noEntry = !HasDivePoint();
    }

    /// <summary>整条边框上是否还有能切入的空地（防止角落被围死后无解）。</summary>
    private bool HasDivePoint()
    {
        IReadOnlyList<Point> pts = Bound.Points;
        int n = pts.Count;
        if (n < 2) return false;

        for (int i = 0; i < n; i++)
        {
            Point a = pts[i];
            Point b = pts[(i + 1) % n];
            double len = Distance(a, b);
            int samples = Math.Max(1, (int)(len / 30.0));
            Point nrm = Bound.InwardNormalOf(i, Paint);

            for (int k = 0; k < samples; k++)
            {
                double t = (k + 0.5) / samples;
                Point mid = new(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);
                Point probe = new(
                    mid.X + nrm.X * GameConfig.DiveProbe,
                    mid.Y + nrm.Y * GameConfig.DiveProbe);

                if (Arena.Inside(probe) && Paint.IsFree(probe) && !NearPainted(probe, GameConfig.BlockRadius))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
