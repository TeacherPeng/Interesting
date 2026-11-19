using System.IO;
using System.Windows;

namespace WeiDi;

public partial class App : Application
{
    public App()
    {
        DispatcherUnhandledException += (_, e) => Log(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            if (e.ExceptionObject is Exception ex) Log(ex);
        };
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // --selftest：无窗口跑一遍规则自检，结果写到 _selftest.log
        if (e.Args.Contains("--selftest"))
        {
            string log = Game.SelfTest.Run();
            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "_selftest.log"), log);
            Shutdown();
            return;
        }

        // --shot：离线渲染两张画面到 _shot_1.png / _shot_2.png，用于核对绘制
        if (e.Args.Contains("--shot"))
        {
            SelfTest_Shot();
            Shutdown();
            return;
        }

        new MainWindow().Show();
    }

    private static void SelfTest_Shot()
    {
        Game.SelfTest.Shot(Path.Combine(AppContext.BaseDirectory, "_shot_1.png"), 1);
        Game.SelfTest.Shot(Path.Combine(AppContext.BaseDirectory, "_shot_2.png"), 2);
    }

    private static void Log(Exception ex)
    {
        try
        {
            File.AppendAllText(
                Path.Combine(AppContext.BaseDirectory, "crash.log"),
                DateTime.Now.ToString("s") + Environment.NewLine + ex + Environment.NewLine + Environment.NewLine);
        }
        catch
        {
            // 记不下来就算了，不能因为写日志再崩一次
        }
    }
}
