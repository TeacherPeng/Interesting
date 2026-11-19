using System.Windows;
using System.Windows.Input;

namespace WeiDi;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        PreviewKeyDown += (_, e) =>
        {
            if (Game.HandleKeyDown(e.Key))
            {
                e.Handled = true;
            }
        };
        PreviewKeyUp += (_, e) => Game.HandleKeyUp(e.Key);

        Deactivated += (_, _) => Game.OnWindowBlur(true);
        Activated += (_, _) => Game.OnWindowBlur(false);
        Loaded += (_, _) => Game.Focus();
    }
}
