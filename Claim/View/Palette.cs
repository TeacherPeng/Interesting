using System.Windows;
using System.Windows.Media;

namespace WeiDi.View;

/// <summary>配色：深底、细线、单色高亮。</summary>
internal static class Palette
{
    public static readonly Brush Background = Frozen(new SolidColorBrush(Color.FromRgb(0x06, 0x08, 0x0C)));

    public static readonly Brush FieldFill = Frozen(new LinearGradientBrush(
        Color.FromRgb(0x18, 0x22, 0x2E), Color.FromRgb(0x0F, 0x17, 0x21), new Point(0, 0), new Point(0, 1)));

    public static readonly Brush Border = Frozen(new SolidColorBrush(Color.FromArgb(0xFF, 0x93, 0xAA, 0xC0)));

    public static readonly Brush Trail = Frozen(new SolidColorBrush(Color.FromRgb(0x8A, 0xEC, 0xFF)));
    public static readonly Brush TrailGlow = Frozen(new SolidColorBrush(Color.FromArgb(0x3C, 0x6E, 0xDB, 0xFF)));

    public static readonly Brush PaintedFill = Frozen(new SolidColorBrush(Color.FromArgb(0xEE, 0x1B, 0x5E, 0x6E)));
    public static readonly Brush PaintedEdge = Frozen(new SolidColorBrush(Color.FromArgb(0xB4, 0x7F, 0xE3, 0xFF)));

    // 扫描线：危险的红橙色，区别于青色轨迹与灰白边框
    public static readonly Brush ScanLine = Frozen(new SolidColorBrush(Color.FromRgb(0xFF, 0x6B, 0x4A)));
    public static readonly Brush ScanGlow = Frozen(new SolidColorBrush(Color.FromArgb(0x40, 0xFF, 0x4A, 0x2E)));

    public static readonly Brush DotFill = Frozen(new SolidColorBrush(Color.FromRgb(0xEC, 0xF9, 0xFF)));
    public static readonly Brush DotRing = Frozen(new SolidColorBrush(Color.FromArgb(0xC0, 0x76, 0xE6, 0xFF)));

    public static readonly Brush DotGlow = Frozen(new RadialGradientBrush(
        new GradientStopCollection
        {
            new GradientStop(Color.FromArgb(0x66, 0x6E, 0xDB, 0xFF), 0.0),
            new GradientStop(Color.FromArgb(0x00, 0x6E, 0xDB, 0xFF), 1.0),
        }));

    public static readonly Brush HudText = Frozen(new SolidColorBrush(Color.FromArgb(0xC8, 0x9A, 0xAF, 0xC4)));
    public static readonly Brush HudDim = Frozen(new SolidColorBrush(Color.FromArgb(0x88, 0x74, 0x89, 0x9E)));
    public static readonly Brush HudStrong = Frozen(new SolidColorBrush(Color.FromArgb(0xF0, 0xDC, 0xEC, 0xF8)));

    public static readonly Brush Overlay = Frozen(new SolidColorBrush(Color.FromArgb(0xB8, 0x04, 0x06, 0x0A)));

    private static Brush Frozen(Brush b)
    {
        b.Freeze();
        return b;
    }
}
