using Android.Content;
using Android.Content.PM;
using Android.Provider;

namespace AutoSwipe;

public static class PackageInfo
{
    public const string PackageName = "com.pengsw.autoswipe";
    public const string ServiceName = $"{PackageName}.AutoSwipeService";
    public const string ActionStartSwipe = $"{PackageName}.START_SWIPE";
    public const string ActionStopSwipe = $"{PackageName}.STOP_SWIPE";
}

[Activity(Label = "Auto Swipe", MainLauncher = true, LaunchMode = LaunchMode.SingleTop)]
public class MainActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.activity_main);

        var _btnStart = FindViewById<Button>(Resource.Id.cmdStart);
        var _btnStop = FindViewById<Button>(Resource.Id.cmdStop);

        _btnStart?.Click += (s, e) =>
        {
            var intent = new Intent(PackageInfo.ActionStartSwipe);
            intent.SetPackage(PackageName); // 👈 仅发送给本应用的组件（安全！）
            SendBroadcast(intent);
            Toast.MakeText(this, "启动划屏指令已发送", ToastLength.Short)?.Show();

            // 尝试切换/启动目标 App(抖音极速版)
            var targetPackage = "com.ss.android.ugc.aweme.lite";
            LaunchApp(targetPackage);
        };

        _btnStop?.Click += (s, e) =>
        {
            var intent = new Intent(PackageInfo.ActionStopSwipe);
            intent.SetPackage(PackageName); // 限定只发给本 App
            SendBroadcast(intent);
            Toast.MakeText(this, "停止划屏指令已发送", ToastLength.Short)?.Show();
        };

        // 提示用户开启无障碍服务（可选）
        CheckAccessibilityPermission();
    }

    private void LaunchApp(string packageName)
    {
        try
        {
            var pm = PackageManager;
            var launchIntent = pm?.GetLaunchIntentForPackage(packageName);
            if (launchIntent != null)
            {
                launchIntent.AddFlags(ActivityFlags.NewTask);
                StartActivity(launchIntent);
            }
            else
            {
                Toast.MakeText(this, "未检测到目标应用", ToastLength.Long)?.Show();
            }
        }
        catch (Exception ex)
        {
            Toast.MakeText(this, $"无法启动目标应用:{ex.Message}", ToastLength.Long)?.Show();
        }
    }

    private void CheckAccessibilityPermission()
    {
        var enabledServices = Settings.Secure.GetString(ContentResolver, Settings.Secure.EnabledAccessibilityServices);
        var serviceName = $"{PackageInfo.PackageName}/{PackageInfo.ServiceName}";
        var accessibilityEnabled = !string.IsNullOrEmpty(enabledServices) && enabledServices.Contains(serviceName);
        if (accessibilityEnabled) return;

        // 跳转到无障碍设置页
        Toast.MakeText(this, "请先在设置中启用无障碍服务！", ToastLength.Long)?.Show();
        var intent = new Intent(Android.Provider.Settings.ActionAccessibilitySettings);
        StartActivity(intent);
    }
}