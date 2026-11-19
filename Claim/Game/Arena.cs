using System.Windows;

namespace WeiDi.Game;

/// <summary>
/// 场地就是 [0,S]×[0,S] 这个正方形：开局时它的四条边就是边框。
/// 围地之后边框会被轨迹改写，所以这里只保留场地本身的常量与越界判断。
/// </summary>
internal static class Arena
{
    public const double S = GameConfig.ArenaSize;

    public static bool Inside(Point p) =>
        p.X >= 0 && p.X <= S && p.Y >= 0 && p.Y <= S;

    /// <summary>越界的坐标夹回场地内，用来兜底（正常流程由边框接触判定处理）。</summary>
    public static Point Clamp(Point p) => new(
        p.X < 0 ? 0 : (p.X > S ? S : p.X),
        p.Y < 0 ? 0 : (p.Y > S ? S : p.Y));
}
