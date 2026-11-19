namespace WeiDi.Game;

/// <summary>
/// 全部可调参数集中在此，改这里就够了。
/// 逻辑坐标固定 900x900，与窗口尺寸无关；窗口只影响渲染缩放。
/// </summary>
internal static class GameConfig
{
    /// <summary>场地边长（逻辑单位）。</summary>
    public const double ArenaSize = 900.0;

    /// <summary>栅格步长 = 1 单位，用于碰撞与面积统计（绘制走矢量，不栅格化）。</summary>
    public const int GridSize = (int)ArenaSize;

    /// <summary>圆点半径：纯视觉，不参与判定（圆心才是轨迹点）。</summary>
    public const double DotRadius = 6.0;

    /// <summary>恒定速度（逻辑单位/秒）。</summary>
    public const double Speed = 240.0;

    /// <summary>边框线宽（逻辑单位），渲染时按 1/k 折成 1 设备像素。</summary>
    public const double BorderLineWidth = 1.0;

    /// <summary>轨迹线宽（逻辑单位），同上。</summary>
    public const double TrailLineWidth = 1.0;

    /// <summary>轨迹点列抽稀距离：走这么远才记一个点，控顶点数又不丢形状。</summary>
    public const double TrailDecimate = 2.0;

    /// <summary>区域内移动时，圆心与已围色块的最小间隙。</summary>
    public const double BlockRadius = 0.9;

    /// <summary>圆心离已围色块小于此值即算「回到边框」，本次轨迹闭合。</summary>
    public const double ContactGap = 1.0;

    /// <summary>起笔后先走这么长不判回框，避免在起笔点旁边（贴着色块）一出发就闭合。</summary>
    public const double FirstCaptureGrace = 12.0;

    /// <summary>围出的面积小于此值（平方单位）的手势不结算，边框也不变。</summary>
    public const double MinCaptureArea = 400.0;

    /// <summary>切入探针距离：从边框往里探这么远有连续空地才允许切入。</summary>
    public const double DiveProbe = DotRadius + 2.0;

    /// <summary>切入时要求这条路至少这么长都是空地（和起笔保护对齐，避免切进去立刻又闭合）。</summary>
    public const double DiveClearance = FirstCaptureGrace + 2.0;

    /// <summary>
    /// 按键「指向空地」的分量超过此值就想切入。不能用「切向分量占优才算走、否则算切」的对比规则：
    /// 斜向边框段上单按一个方向键，两分量恰好各 0.707，会永远被判成沿边走，切不进去。
    /// </summary>
    public const double DiveThreshold = 0.35;

    /// <summary>切向分量低于此阈值视为「近乎垂直入射」，走回框方向优先级链。</summary>
    public const double TangentialEpsilon = 0.15;

    /// <summary>剩余可围面积低于此比例即通关。</summary>
    public const double WinRemainRatio = 0.04;

    /// <summary>单帧 dt 上限，防止拖窗口/断点恢复后瞬移。</summary>
    public const double MaxFrameDt = 0.05;

    /// <summary>物理固定步长。</summary>
    public const double FixedStep = 1.0 / 240.0;

    // ── 扫描线（5 根共用一个运动交点，各自旋转；离开边框后碰上即死） ─────────

    /// <summary>扫描线条数。</summary>
    public const int ScanCount = 5;

    /// <summary>公共交叉点（枢纽）的移动速度（单位/秒）。</summary>
    public const double ScanHubSpeed = 70.0;

    /// <summary>枢纽与场地边缘保持的距离，避免交点贴边。</summary>
    public const double ScanHubMargin = 90.0;

    /// <summary>整体旋转（所有线共享的那一份角速度）的绝对值区间（弧度/秒）。</summary>
    public const double ScanSpinMin = 0.35;
    public const double ScanSpinMax = 0.75;

    /// <summary>
    /// 5 根线相邻夹角之和（= 首尾两线的张角）上限，60°。
    /// 4 个相邻夹角之和 = 槽位总跨度 + 首尾两根摆动的极端差，必须 ≤ 这个值。
    /// </summary>
    public const double ScanFanTotal = Math.PI / 3.0;

    /// <summary>
    /// 相邻两根线的槽位角度间隔（弧度，约 11.5°）。4 段共约 45.8°。
    /// 满足两条约束：① 相邻间隔 &gt; 2×摆动限幅 → 永不交叉/重合；
    /// ② 4×间隔 + 2×摆动限幅 ≈ 54° &lt; ScanFanTotal(60°) → 留有安全余量。
    /// </summary>
    public const double ScanSlotStep = 0.20;

    /// <summary>
    /// 每根线相对自己基准槽位的独立摆动限幅（弧度，约 4.3°）。
    /// 摆动被限制住，5 根线就不会互相并拢或发散，也不会让扇面张角超标。
    /// </summary>
    public const double ScanWobbleLimit = 0.075;

    /// <summary>每根线独立摆动的角速度绝对值区间（弧度/秒）。</summary>
    public const double ScanWobbleRateMin = 0.5;
    public const double ScanWobbleRateMax = 1.1;

    /// <summary>初始整体朝向的随机扰动幅度（弧度）。</summary>
    public const double ScanAngleJitter = 0.52;

    /// <summary>每隔多久（秒）随机换一次整体转向、交点方向与各自摆动速率。</summary>
    public const double ScanChangeMin = 1.6;
    public const double ScanChangeMax = 3.2;

    /// <summary>圆点圆心到扫描线的距离小于此值即判被击中（圆点半径 + 一点余量）。</summary>
    public const double ScanHitRadius = DotRadius + 1.0;

    /// <summary>
    /// 扫描线到运动轨迹的命中半径：扫过已画出的轨迹任一段，距离小于此值即判负。
    /// 轨迹是细线，取比圆点半径小的余量（覆盖扫描线亮芯宽度即可）。
    /// </summary>
    public const double ScanTrailHitRadius = 2.5;
}
