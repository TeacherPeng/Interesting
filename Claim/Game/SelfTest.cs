using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WeiDi.Game;

/// <summary>
/// 无窗口自检：用合成按键跑几段「画线 → 回框 → 填色 → 沿新边框走 → 再切入」，
/// 把关键规则逐条核对一遍。只在 --selftest 启动参数下执行，不影响正常游玩。
/// </summary>
internal static class SelfTest
{
    private const double Dt = GameConfig.FixedStep;

    /// <summary>
    /// 既有围地规则测试用的会话：关掉扫描线的致命判定。
    /// 扫描线会随机扫动，若让它参与判定，同一段脚本时而被击中时而不被击中，
    /// 会让规则自检变成概率性失败。扫描线的几何/碰撞由专项测试单独核对。
    /// </summary>
    private static GameSession NewSafeSession() => new() { ScanLethal = false };

    public static string Run()
    {
        var log = new StringBuilder();
        int passed = 0;
        int failed = 0;

        void Check(bool ok, string name, string detail = "")
        {
            if (ok) passed++;
            else failed++;

            log.AppendLine((ok ? "PASS  " : "FAIL  ") + name + (detail.Length > 0 ? "   " + detail : ""));
        }

        try
        {
            var s = NewSafeSession();

            Check(s.Mode == GameMode.Frame, "开局在边框上");
            Check(Near(s.Position, new Point(450, 0)), "开局在上边中点", Fmt(s.Position));
            Check(Math.Abs(s.Bound.Length - 4 * Arena.S) < 1e-6, "开局边框周长 = 4S", s.Bound.Length.ToString("0.0"));

            // ── 边框上行走（含反向） ──
            Advance(s, 0.2, new Point(1, 0));
            Check(Near(s.Position, new Point(498, 0), 0.5), "沿上边向右走 48 单位", Fmt(s.Position));
            Check(OnBoundary(s) < 1e-6, "圆心精确压在边框上", OnBoundary(s).ToString("0.000000"));

            double uBefore = s.ArcPosition;
            Advance(s, 0.2, new Point(-1, 0));
            Check(s.Position.X < 470 && Math.Abs(s.Position.Y) < 1e-6, "按反向键能真的倒着走",
                Fmt(s.Position) + "   s " + uBefore.ToString("0.0") + " → " + s.ArcPosition.ToString("0.0"));

            Advance(s, 0.2, new Point(1, 0));

            // ── 掉转规则：封闭边框可掉头、未封闭轨迹不可掉头 ──
            // 边框上掉头已由上面「按反向键能真的倒着走」覆盖。
            // 未封闭轨迹：切入并走完起笔通道后，按反向键不得掉头，圆点继续朝原方向画。
            var rs = NewSafeSession();
            Advance(rs, 0.2, new Point(1, 0));
            AdvanceUntil(rs, new Point(0, 1), () => rs.Mode == GameMode.Field, 0.3);
            Advance(rs, 0.35, new Point(1, 0));              // 走完 escape，稳定朝右
            Point turnFrom = rs.Position;
            Advance(rs, 0.15, new Point(-1, 0));             // 按反向键
            Check(rs.Mode == GameMode.Field && rs.Position.X > turnFrom.X + 5,
                "未封闭轨迹上按反向键不会掉头", Fmt(turnFrom) + " → " + Fmt(rs.Position));

            // ── 切入：只画轨迹，不填色 ──
            double ratioBefore = s.PaintedRatio;
            Advance(s, 0.9, new Point(0, 1));
            Check(s.Mode == GameMode.Field, "按向内键切入空地");
            Check(s.Trail.Count > 10, "轨迹点列在增长", s.Trail.Count.ToString());
            Check(Math.Abs(s.PaintedRatio - ratioBefore) < 1e-9, "画线过程中不做任何填色",
                (s.PaintedRatio * 100).ToString("0.000") + "%");

            Advance(s, 0.5, new Point(1, 0));
            Check(s.Position.X > 610, "空地里转向右", Fmt(s.Position));

            // ── 回框：结算填色 + 轨迹成为新边框 ──
            AdvanceUntil(s, new Point(0, -1), () => s.Mode == GameMode.Frame, 1.6);
            Check(s.Mode == GameMode.Frame, "轨迹回到边框");
            Check(s.PaintedRatio > 0.02, "回到边框后才填色", (s.PaintedRatio * 100).ToString("0.00") + "%");
            Check(s.PaintedRatio < 0.05, "填的只是围出的那一块（没整片刷）", (s.PaintedRatio * 100).ToString("0.00") + "%");
            Check(s.Bound.Version == 3, "边框被改写了一次", "v" + s.Bound.Version);
            Check(Math.Abs(s.Position.Y) < 1e-6, "回框点落在边框上", Fmt(s.Position));

            bool trailInBorder = false;
            foreach (Point p in s.Bound.Points)
            {
                if (Near(p, new Point(618, 216), 4.0)) trailInBorder = true;
            }
            Check(trailInBorder, "轨迹上的点已成为边框顶点");

            bool arcStillBorder = false;
            foreach (Point p in s.Bound.Points)
            {
                if (Math.Abs(p.Y) < 1e-6 && p.X > 500 && p.X < 616) arcStillBorder = true;
            }
            Check(!arcStillBorder, "被围掉的那段边线退出了边框");

            // ── 沿新边框行走 ──
            Advance(s, 0.3, new Point(1, 0));
            Check(OnBoundary(s) < 1e-6, "沿新边框继续走仍压在线上", OnBoundary(s).ToString("0.000000"));

            Advance(s, 0.6, new Point(-1, 0));
            Check(OnBoundary(s) < 1e-6, "反向走仍压在线上", OnBoundary(s).ToString("0.000000"));
            Check(s.Position.Y > 20, "反向走进了轨迹那段新边框", Fmt(s.Position));

            // ── 从新边框再切入：按键方向按「几何上的空地」推，不能用代码自己算的法向（否则是自证）──
            Point keys = KeyTowardFree(s);
            double ratioBefore2 = s.PaintedRatio;

            Advance(s, 2 * Dt, keys);
            Check(s.Mode == GameMode.Field, "能从轨迹边框上再切入空地", "按 " + Fmt(keys));
            Check(Math.Abs(s.PaintedRatio - ratioBefore2) < 1e-9, "再次画线也不填色");

            // 起笔通道内按反向键可撤销这一笔（未封闭轨迹上不能掉头，但起笔瞬间允许反悔）
            Advance(s, 2 * Dt, Neg(keys));
            Check(s.Mode == GameMode.Frame, "起笔通道内按反向键撤销切入");
            Check(OnBoundary(s) < 1e-6, "撤销后重新压在边框上", OnBoundary(s).ToString("0.000000"));

            // ── 第二块：从正方形右边线切入，轨迹撞上新围的色块 → 跨接边框结算 ──
            var s2 = NewSafeSession();
            Advance(s2, 0.2, new Point(1, 0));
            Advance(s2, 0.9, new Point(0, 1));
            Advance(s2, 0.5, new Point(1, 0));
            AdvanceUntil(s2, new Point(0, -1), () => s2.Mode == GameMode.Frame, 1.6);
            double r1 = s2.PaintedRatio;
            Check(r1 > 0.02, "第一块围地结算", (r1 * 100).ToString("0.00") + "%");

            Advance(s2, 1.6, new Point(1, 0));          // 沿上边走到右上角，再沿右边线往下
            Check(OnBoundary(s2) < 1e-6, "走到正方形右边线上", Fmt(s2.Position));
            Check(s2.Position.X > 899, "位置在右边线", Fmt(s2.Position));

            AdvanceUntil(s2, new Point(-1, 0), () => s2.Mode == GameMode.Field, 0.5);
            Check(s2.Mode == GameMode.Field, "从右边线向里切入");

            AdvanceUntil(s2, new Point(-1, 0), () => s2.Mode == GameMode.Frame, 2.0);
            Check(s2.Mode == GameMode.Frame, "轨迹撞上新色块即回框");
            double r2 = s2.PaintedRatio;
            Check(r2 > r1 + 0.02, "第二块地也填上了（跨接结算）",
                (r1 * 100).ToString("0.00") + "% → " + (r2 * 100).ToString("0.00") + "%");

            bool allInside2 = true;
            foreach (Point p in s2.Bound.Points)
            {
                if (!Arena.Inside(p)) allInside2 = false;
            }
            Check(allInside2, "两块围地后边框顶点仍在场地内");
            Check(s2.Bound.Points.Count > 10, "边框同时含两段轨迹", s2.Bound.Points.Count.ToString());

            // ── 边框始终闭合、点都在场地内 ──
            bool allInside = true;
            foreach (Point p in s.Bound.Points)
            {
                if (!Arena.Inside(p)) allInside = false;
            }
            Check(allInside, "边框顶点都在场地内");
            Check(s.Bound.Points.Count >= 5, "边框仍是闭环并含轨迹顶点", s.Bound.Points.Count.ToString());

            log.AppendLine();
            log.AppendLine("边框周长 " + s.Bound.Length.ToString("0.0") + "  顶点 " + s.Bound.Points.Count
                           + "  已围 " + (s.PaintedRatio * 100).ToString("0.00") + "%");

            // ── 回框点那个拐角：站在刚接上的那一点上，朝空地按必须能出去 ──
            // 拐角上轨迹线那一行/一列格子都算已填，正右方被这条轨迹线挡住，
            // 法向要在附近偏一点角度才能切出去（偏不过去就会永远卡在这里）。
            var g = BuildTwoBlocks();
            Check(Near(g.Position, new Point(618, 102), 4.0), "回框点位置", Fmt(g.Position));

            Advance(g, 2 * Dt, new Point(1, 0));
            Check(g.Mode == GameMode.Field, "站在回框点朝空地按 右 能切出去",
                "pos=" + Fmt(g.Position) + " mode=" + g.Mode);

            var g2 = BuildTwoBlocks();
            Point start = g2.Position;
            Advance(g2, 0.3, new Point(1, 0));
            Check(!Near(g2.Position, start, 10), "继续按住 右 不会原地卡死", Fmt(g2.Position));

            // ── 圆点轨迹全程只允许水平/垂直，绝不出现斜向 ──
            // 走一条「切入 → 右转 → 上转」的 L 形轨迹，逐段核对位移只有一个分量非零。
            var ax = NewSafeSession();
            Advance(ax, 0.2, new Point(1, 0));
            Advance(ax, 0.9, new Point(0, 1));
            Advance(ax, 0.5, new Point(1, 0));
            Advance(ax, 0.3, new Point(0, -1));
            int skewSegs = 0;
            string skewDetail = "";
            for (int i = 1; i < ax.Trail.Count; i++)
            {
                double dx = Math.Abs(ax.Trail[i].X - ax.Trail[i - 1].X);
                double dy = Math.Abs(ax.Trail[i].Y - ax.Trail[i - 1].Y);
                if (dx > 1e-6 && dy > 1e-6)
                {
                    skewSegs++;
                    if (skewDetail.Length == 0)
                        skewDetail = "i=" + i + " " + Fmt(ax.Trail[i - 1]) + "→" + Fmt(ax.Trail[i]);
                }
            }
            Check(ax.Mode == GameMode.Field && skewSegs == 0,
                "圆点轨迹全程只有水平/垂直段，无斜向",
                "斜向段=" + skewSegs + " / " + ax.Trail.Count + " 点  " + skewDetail);

            // 同时按两个方向键（斜键）时吸附为轴向，走出来的仍是水平或垂直段。
            var diag = NewSafeSession();
            Advance(diag, 0.2, new Point(1, 0));
            Advance(diag, 0.9, new Point(0, 1));          // 先单键切入空地
            Advance(diag, 0.5, new Point(1, 1));          // 右+下：应吸附为水平轴
            int diagSkew = 0;
            for (int i = 1; i < diag.Trail.Count; i++)
            {
                double dx = Math.Abs(diag.Trail[i].X - diag.Trail[i - 1].X);
                double dy = Math.Abs(diag.Trail[i].Y - diag.Trail[i - 1].Y);
                if (dx > 1e-6 && dy > 1e-6) diagSkew++;
            }
            Check(diagSkew == 0, "斜向双键被吸附为轴向，轨迹无斜段",
                "斜向段=" + diagSkew + " / " + diag.Trail.Count + " 点");

            // ── 轨迹中段：按垂直于轨迹的那个键也要能出去 ──
            var h = BuildTwoBlocks();            Advance(h, 0.4, new Point(0, -1));                 // 沿轨迹往回走
            Check(Math.Abs(h.Position.Y - 102) < 3 && h.Position.X > 680, "走到轨迹中段", Fmt(h.Position));

            Point midKey = KeyTowardFree(h);
            Advance(h, 0.3, midKey);
            Check(h.Mode == GameMode.Field, "站在轨迹中段按下垂直键能切入", "按 " + Fmt(midKey));

            double ratioAfter = h.PaintedRatio;
            AdvanceUntil(h, Neg(midKey), () => h.Mode == GameMode.Frame, 1.0);
            Check(Math.Abs(h.PaintedRatio - ratioAfter) < 1e-9, "只是探出去又回来，没有多填一块");

            // ── 全域扫一遍：边框上每一处按四个方向键，有没有「按了不动」的死点 ──
            // 轨迹只允许轴向，边框段因此也都是横平竖直，用两块围地的状态即可覆盖。
            {
                string name = "两块";
                Func<GameSession> build = BuildTwoBlocks;
                var seed = build();
                int frozen = 0;
                for (double sv = 0; sv < seed.Bound.Length; sv += 20)
                {
                    foreach (Point k in new[] { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1) })
                    {
                        var probe = build();
                        WalkTo(probe, sv);
                        if (probe.Mode != GameMode.Frame) continue;
                        // 纯朝墙外按（既不能切入、也无沿边切向分量）时原地不动是合理的，
                        // 不算「按了方向键却卡住」——只检查朝内或沿边有意义的按键。
                        Point dn = probe.Bound.InwardAt(probe.ArcPosition, probe.Paint);
                        Point dt = probe.Bound.TangentAt(probe.ArcPosition);
                        double inward = k.X * dn.X + k.Y * dn.Y;
                        double along = k.X * dt.X + k.Y * dt.Y;
                        if (inward <= 0 && Math.Abs(along) < 1e-6) continue;
                        Point p0 = probe.Position;
                        probe.Step(Dt, k);
                        probe.Step(Dt, k);
                        if (Near(probe.Position, p0, 0.05))
                        {
                            frozen++;
                            log.AppendLine("  卡住 [" + name + "] s=" + sv.ToString("0")
                                           + " key=" + Fmt(k) + " " + probe.Mode);
                        }
                    }
                }
                // 死点必须为零：这是断言，不再只是诊断打印。
                Check(frozen == 0, "[" + name + "] 边框各处按方向键都不会原地卡死",
                    "死点数=" + frozen + " / " + (int)(seed.Bound.Length / 20) * 4);
            }
            foreach (bool together in new bool[] { false, true })
            {
                var u = NewSafeSession();
                for (int i = 0; i < 3000; i++)
                {
                    Point tt = u.Bound.TangentAt(u.ArcPosition);
                    if (Math.Abs(tt.X + 1) < 0.01 && u.Position.X < 300) break;
                    u.Step(Dt, new Point(Math.Sign(tt.X), Math.Sign(tt.Y)));
                }
                Advance(u, 0.5, new Point(1, 0));
                log.AppendLine("  [同时按=" + together + "] 下边线向右 " + Fmt(u.Position)
                               + " t=" + Fmt(u.Bound.TangentAt(u.ArcPosition)));
                Advance(u, 0.5, new Point(0, -1));
                log.AppendLine("    按上 → " + u.Mode + " " + Fmt(u.Position) + " trail=" + u.TrailLength.ToString("0.0"));
                Advance(u, 0.3, new Point(1, 0));
                log.AppendLine("    按右 → " + u.Mode + " " + Fmt(u.Position));
                AdvanceUntil(u, new Point(0, 1), () => u.Mode == GameMode.Frame, 2.0);
                log.AppendLine("    按下 → " + u.Mode + " " + Fmt(u.Position)
                               + " 已围 " + (u.PaintedRatio * 100).ToString("0.00") + "%");

                if (together)
                {
                    Advance(u, 0.4, new Point(-1, -1));
                }
                else
                {
                    StepTrace(u, 96, new Point(-1, 0), log);      // 左
                    StepTrace(u, 72, new Point(0, -1), log);      // 上
                }
                log.AppendLine("    左上后 → " + u.Mode + " " + Fmt(u.Position)
                               + " trail=" + u.TrailLength.ToString("0.0")
                               + " t=" + Fmt(u.Bound.TangentAt(u.ArcPosition))
                               + " n=" + Fmt(u.Bound.InwardAt(u.ArcPosition, u.Paint)));

                StepTrace(u, 48, new Point(1, 0), log);           // 右
                log.AppendLine("    按右后 → " + u.Mode + " " + Fmt(u.Position)
                               + " trail=" + u.TrailLength.ToString("0.0"));
            }

            // ── 扫描线：共点、各自旋转、两端接触边框、不发散、随机变化、碰撞致死 ──
            var scan = new GameSession();       // 致命判定开启，测真实行为
            Check(scan.Scanner.Segments.Count == GameConfig.ScanCount,
                "扫描线共 5 根", scan.Scanner.Segments.Count.ToString());

            Point hub0 = scan.Scanner.Hub;
            Check(Math.Abs(hub0.X - Arena.S * 0.30) < 1e-6 && Math.Abs(hub0.Y - Arena.S * 0.70) < 1e-6,
                "公共交点从左下起步", Fmt(hub0));

            // 推进一段，核对：共点、两端接触边框、各自角度不同且互不发散
            Advance(scan, 2.0, new Point(0, 0));
            int visible = 0;
            bool endsOnBorder = true;
            bool throughHub = true;
            bool distinctAngle = true;
            for (int i = 0; i < scan.Scanner.Segments.Count; i++)
            {
                var seg = scan.Scanner.Segments[i];
                bool vis = !(Math.Abs(seg.A.X - seg.B.X) < 1e-9 && Math.Abs(seg.A.Y - seg.B.Y) < 1e-9);
                if (!vis) continue;
                visible++;
                if (BorderDist(scan, seg.A) > 1e-4 || BorderDist(scan, seg.B) > 1e-4) endsOnBorder = false;
                // 每根弦都应穿过公共交点（点到线段距离≈0）
                if (SegDist(scan.Scanner.Hub, seg.A, seg.B) > 1e-4) throughHub = false;
            }
            // 任意两根的朝向（模 π 后）不同；槽位均分 + 限幅摆动保证绝不重合/发散
            for (int i = 0; i < scan.Scanner.Segments.Count; i++)
            {
                for (int j = i + 1; j < scan.Scanner.Segments.Count; j++)
                {
                    double d = Math.Abs(WrapPi(scan.Scanner.Segments[i].Angle - scan.Scanner.Segments[j].Angle));
                    while (d > Math.PI / 2) d = Math.PI - d;   // 直线无向，取锐角差
                    if (d < 0.02) distinctAngle = false;
                }
            }
            Check(visible >= 1, "扫描线在场内可见", "可见 " + visible + " 根");
            Check(endsOnBorder, "每根扫描线两端都接触边框");
            Check(throughHub, "5 根扫描线交于同一公共点");
            Check(distinctAngle, "5 根线各有自己的角度（不重合、不发散）");

            // 交点在移动
            Point hubA = scan.Scanner.Hub;
            Advance(scan, 0.5, new Point(0, 0));
            Check(Near(scan.Scanner.Hub, hubA, 1e-6) == false, "公共交点持续运动",
                Fmt(hubA) + " → " + Fmt(scan.Scanner.Hub));

            // 每根线各自的朝向都在变（各自旋转）
            double[] ang0 = new double[GameConfig.ScanCount];
            for (int i = 0; i < ang0.Length; i++) ang0[i] = scan.Scanner.Segments[i].Angle;
            Advance(scan, 0.5, new Point(0, 0));
            int rotating = 0;
            for (int i = 0; i < ang0.Length; i++)
                if (Math.Abs(WrapPi(scan.Scanner.Segments[i].Angle - ang0[i])) > 1e-4) rotating++;
            Check(rotating >= 3, "扫描线各自旋转", rotating + "/5 根朝向发生变化");

            // 角度会持续变化（整体旋转 + 个体摆动）
            double baseStart = scan.Scanner.BaseAngle;
            Advance(scan, 1.0, new Point(0, 0));
            Check(Math.Abs(WrapPi(scan.Scanner.BaseAngle - baseStart)) > 0.02, "整体朝向持续旋转",
                baseStart.ToString("0.000") + " → " + scan.Scanner.BaseAngle.ToString("0.000"));

            // 5 根线相邻夹角之和（排序后首尾差）在任意时刻都 ≤ 60°。
            // 采样多个时刻，覆盖摆动相位与整体转向的各种组合。
            double fanMax = 0;
            for (int t = 0; t < 40; t++)
            {
                Advance(scan, 0.15, new Point(0, 0));
                double fan = FanSpreadDeg(scan.Scanner);
                if (fan > fanMax) fanMax = fan;
            }
            Check(fanMax <= 60.0 + 1e-6, "5 根扫描线相邻夹角和恒 ≤ 60°",
                "最大 " + fanMax.ToString("0.0") + "°");

            // 围地后扫描线应被色块挡住：采样每根弦的中段（避开压在边框上的两端），
            // 落在已填色块内的采样点数为零。
            var blocked = BuildTwoBlocks();
            int throughPaint = 0;
            for (int t = 0; t < 30; t++)
            {
                Advance(blocked, 0.1, new Point(0, 0));
                foreach (var seg in blocked.Scanner.Segments)
                {
                    if (Near(seg.A, seg.B, 1e-9)) continue;
                    for (int q = 2; q <= 8; q++)
                    {
                        double f = q / 10.0;
                        Point p = new(seg.A.X + (seg.B.X - seg.A.X) * f,
                                      seg.A.Y + (seg.B.Y - seg.A.Y) * f);
                        if (!blocked.Paint.IsFree(p)) throughPaint++;
                    }
                }
            }
            Check(throughPaint == 0, "扫描线被已填色块挡住，不穿过色块",
                "穿墙采样=" + throughPaint);

            // ── 碰撞：扫描线扫到「已画出的轨迹」任一段即死，不只是圆点当前位置 ──
            var dead = new GameSession();
            Advance(dead, 0.2, new Point(1, 0));
            AdvanceUntil(dead, new Point(0, 1), () => dead.Mode == GameMode.Field, 0.3);
            Advance(dead, 0.3, new Point(1, 0));          // 拖出一段有长度的轨迹
            Check(dead.Mode == GameMode.Field && dead.Trail.Count >= 3,
                "碰撞测试：已进入 Field 且画出了轨迹", "轨迹点 " + dead.Trail.Count);

            // 取轨迹「中段」的一点作交点——它离圆点当前位置很远，
            // 以此证明判负来自扫到轨迹，而不是压到圆点本身。
            Point midTrail = dead.Trail[dead.Trail.Count / 2];
            double farFromDot = Math.Sqrt((midTrail.X - dead.Position.X) * (midTrail.X - dead.Position.X)
                                        + (midTrail.Y - dead.Position.Y) * (midTrail.Y - dead.Position.Y));
            Check(farFromDot > GameConfig.ScanTrailHitRadius * 4,
                "轨迹中段远离圆点当前位置", "相距 " + farFromDot.ToString("0.0"));

            dead.Scanner.SetHubForTest(midTrail);
            Check(!dead.Scanner.Hits(dead.Position, GameConfig.ScanTrailHitRadius),
                "此刻扫描线并未压到圆点当前位置");
            Check(dead.Scanner.HitsTrail(dead.Trail, GameConfig.ScanTrailHitRadius),
                "扫描线扫到轨迹中段 → 命中轨迹");
            dead.Step(Dt, new Point(0, 0));
            Check(dead.Dead, "离开边框后扫到轨迹 → 游戏结束");

            // 扫描线离轨迹很远时不判死：把交点放到场地另一角
            var far = new GameSession();
            Advance(far, 0.2, new Point(1, 0));
            AdvanceUntil(far, new Point(0, 1), () => far.Mode == GameMode.Field, 0.3);
            Advance(far, 0.3, new Point(1, 0));
            far.Scanner.SetHubForTest(new Point(100, Arena.S - 100));
            Check(!far.Scanner.HitsTrail(far.Trail, GameConfig.ScanTrailHitRadius),
                "扫描线远离轨迹时不命中");
            far.Step(Dt, new Point(0, 0));
            Check(!far.Dead, "扫描线远离轨迹时游戏继续");

            // 在边框上巡航，轨迹为空，即使被扫描线压过也不判死
            var onFrame = new GameSession();
            onFrame.Scanner.SetHubForTest(onFrame.Position);
            onFrame.Step(Dt, new Point(1, 0));
            Check(!onFrame.Dead && onFrame.Mode == GameMode.Frame, "在边框上时碰到扫描线不判死");

            // 关闭致命判定后，Field 模式扫到轨迹也不死（截图/录制用）
            var safe = new GameSession { ScanLethal = false };
            Advance(safe, 0.2, new Point(1, 0));
            AdvanceUntil(safe, new Point(0, 1), () => safe.Mode == GameMode.Field, 0.3);
            Advance(safe, 0.3, new Point(1, 0));
            safe.Scanner.SetHubForTest(safe.Trail[safe.Trail.Count / 2]);
            Check(safe.Scanner.HitsTrail(safe.Trail, GameConfig.ScanTrailHitRadius),
                "关闭致命判定时扫描线仍扫到轨迹（只是不判死）");
            safe.Step(Dt, new Point(0, 0));
            Check(!safe.Dead, "关闭致命判定后扫到轨迹不判死");
        }
        catch (Exception ex)
        {
            failed++;
            log.AppendLine("EXCEPTION  " + ex);
        }

        log.AppendLine(passed + " 项通过 / " + failed + " 项失败");
        return log.ToString();
    }

    /// <summary>
    /// 两个已围地块的状态（第一块从顶边切入围住，第二块从右边线切入、轨迹横着收回）。
    /// 用它来复现「站在新轨迹上」的输入手感。
    /// </summary>
    private static GameSession BuildTwoBlocks()
    {
        var g = NewSafeSession();
        Advance(g, 0.2, new Point(1, 0));
        Advance(g, 0.9, new Point(0, 1));
        Advance(g, 0.5, new Point(1, 0));
        AdvanceUntil(g, new Point(0, -1), () => g.Mode == GameMode.Frame, 1.6);
        Advance(g, 1.6, new Point(1, 0));
        AdvanceUntil(g, new Point(-1, 0), () => g.Mode == GameMode.Field, 0.5);
        AdvanceUntil(g, new Point(-1, 0), () => g.Mode == GameMode.Frame, 2.0);
        return g;
    }

    /// <summary>
    /// 按几何判断「哪个方向键指向空地」：探针只查栅格，不看代码算出来的法向。
    /// 这样测试才不是拿实现证明实现。
    /// </summary>
    private static Point KeyTowardFree(GameSession s)
    {
        Point[] keys = { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };
        foreach (Point k in keys)
        {
            Point p = new(s.Position.X + k.X * 10, s.Position.Y + k.Y * 10);
            if (Arena.Inside(p) && s.Paint.IsFree(p)) return k;
        }
        return new Point(0, 1);
    }

    /// <summary>
    /// 离线截图：把两条典型画面渲染成 PNG，用来核对边框是否沿轨迹改道。
    /// 1 = 第一块已围好、第二段轨迹正在拖；2 = 跨接结算出第二块之后。
    /// </summary>
    public static void Shot(string path, int stage)
    {
        var view = new WeiDi.View.GameView
        {
            Width = 1000,
            Height = 1000,
        };

        GameSession s = view.Session;
        s.ScanLethal = false;   // 截图只看画面，不让随机扫描线把圆点判死

        Advance(s, 0.2, new Point(1, 0));
        Advance(s, 0.9, new Point(0, 1));
        Advance(s, 0.5, new Point(1, 0));
        AdvanceUntil(s, new Point(0, -1), () => s.Mode == GameMode.Frame, 1.6);

        if (stage == 1)
        {
            Advance(s, 0.5, new Point(1, 0));
            Advance(s, 0.45, new Point(0, 1));
        }
        else
        {
            Advance(s, 1.6, new Point(1, 0));
            AdvanceUntil(s, new Point(-1, 0), () => s.Mode == GameMode.Field, 0.5);
            AdvanceUntil(s, new Point(-1, 0), () => s.Mode == GameMode.Frame, 2.0);
        }

        view.Measure(new Size(1000, 1000));
        view.Arrange(new Rect(0, 0, 1000, 1000));
        view.UpdateLayout();

        var bmp = new RenderTargetBitmap(1000, 1000, 96, 96, PixelFormats.Pbgra32);
        bmp.Render(view);

        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bmp));

        using var fs = System.IO.File.Create(path);
        encoder.Save(fs);
    }

    private static void Advance(GameSession s, double seconds, Point keys)
    {
        int steps = (int)Math.Round(seconds / Dt);
        for (int i = 0; i < steps; i++) s.Step(Dt, keys);
    }

    /// <summary>逐子步打点，用来定位「按键之后到底发生了什么」。</summary>
    private static void StepTrace(GameSession s, int steps, Point keys, StringBuilder lg)
    {
        for (int i = 0; i < steps; i++)
        {
            Point t0 = s.Bound.TangentAt(s.ArcPosition);
            bool wasFrame = s.Mode == GameMode.Frame;
            s.Step(Dt, keys);
            if (!wasFrame || s.Mode != GameMode.Frame || i % 8 == 0)
            {
                lg.AppendLine("      i=" + i + " " + s.Mode + " " + Fmt(s.Position)
                              + " trail=" + s.TrailLength.ToString("0.0")
                              + " t=" + Fmt(t0)
                              + " n=" + Fmt(s.Bound.InwardAt(s.ArcPosition, s.Paint)));
            }
        }
    }

    /// <summary>沿边框（正向）走到弧长 ≈ target 的位置。</summary>
    private static void WalkTo(GameSession g, double target)
    {
        for (int i = 0; i < 2000; i++)
        {
            if (Math.Abs(g.Bound.Wrap(target - g.ArcPosition)) < 24) return;
            Point t = g.Bound.TangentAt(g.ArcPosition);
            g.Step(Dt, new Point(Math.Sign(t.X), Math.Sign(t.Y)));
            if (g.Mode != GameMode.Frame) return;
        }
    }

    private static void AdvanceUntil(GameSession s, Point keys, Func<bool> stop, double maxSeconds)
    {
        int steps = (int)Math.Round(maxSeconds / Dt);
        for (int i = 0; i < steps; i++)
        {
            s.Step(Dt, keys);
            if (stop()) return;
        }
    }

    private static Point Neg(Point p) => new(-p.X, -p.Y);

    private static bool Near(Point a, Point b, double tol = 1e-6) =>
        Math.Abs(a.X - b.X) <= tol && Math.Abs(a.Y - b.Y) <= tol;

    /// <summary>点到当前边框折线的最短距离。</summary>
    private static double BorderDist(GameSession s, Point p)
    {
        Point q = s.Bound.PointAt(s.Bound.Project(p));
        double dx = p.X - q.X;
        double dy = p.Y - q.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private static double WrapPi(double a)
    {
        while (a > Math.PI) a -= Math.PI * 2;
        while (a < -Math.PI) a += Math.PI * 2;
        return a;
    }

    /// <summary>
    /// 5 根扫描线按朝向排序后首尾两线的夹角（度），即相邻夹角之和。
    /// 各线朝向可能跨 ±π 接缝，先把相对基准角归一到 (-π/2, π/2] 再排。
    /// </summary>
    private static double FanSpreadDeg(ScanLines sc)
    {
        double[] rel = new double[sc.Segments.Count];
        for (int i = 0; i < rel.Length; i++)
        {
            double d = WrapPi(sc.Segments[i].Angle - sc.BaseAngle);
            while (d > Math.PI / 2) d -= Math.PI;
            while (d <= -Math.PI / 2) d += Math.PI;
            rel[i] = d;
        }
        Array.Sort(rel);
        return (rel[^1] - rel[0]) * 180.0 / Math.PI;
    }

    /// <summary>点到线段的距离。</summary>
    private static double SegDist(Point p, Point a, Point b)
    {
        double dx = b.X - a.X, dy = b.Y - a.Y;
        double len2 = dx * dx + dy * dy;
        if (len2 < 1e-12) return Math.Sqrt((p.X - a.X) * (p.X - a.X) + (p.Y - a.Y) * (p.Y - a.Y));
        double t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / len2;
        if (t < 0) t = 0;
        else if (t > 1) t = 1;
        double qx = a.X + dx * t - p.X, qy = a.Y + dy * t - p.Y;
        return Math.Sqrt(qx * qx + qy * qy);
    }

    private static string Fmt(Point p) => "(" + p.X.ToString("0.0") + ", " + p.Y.ToString("0.0") + ")";

    /// <summary>圆心到边框线的距离。</summary>
    private static double OnBoundary(GameSession s)
    {
        Point p = s.Position;
        Point q = s.Bound.PointAt(s.Bound.Project(p));
        double dx = p.X - q.X;
        double dy = p.Y - q.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}
