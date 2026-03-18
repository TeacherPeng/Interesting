using Android.Content;
using Android.Content.PM;
using Android.Provider;
using Android.Widget;

namespace ElevenAssistant;

public static class PackageInfo
{
    public const string PackageName = "com.pengsw.elevenassistant";
    public const string ServiceName = $"{PackageName}.elevenassistantService";
    public const string ActionStart = $"{PackageName}.START_ACTION";
    public const string ActionStop = $"{PackageName}.STOP_ACTION";
    public const string ExtraMinDelay = "min_delay";
    public const string ExtraMaxDelay = "max_delay";
}

[Activity(Label = "Eleven Assistant", MainLauncher = true, LaunchMode = LaunchMode.SingleTop)]
public class MainActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.activity_main);

        var _btnStart = FindViewById<Button>(Resource.Id.cmdStart);
        var _btnStop = FindViewById<Button>(Resource.Id.cmdStop);
        var _btnPreset1 = FindViewById<Button>(Resource.Id.cmdPreset1);
        var _btnPreset2 = FindViewById<Button>(Resource.Id.cmdPreset2);
        var _btnPreset3 = FindViewById<Button>(Resource.Id.cmdPreset3);
        var _editMinDelay = FindViewById<EditText>(Resource.Id.editMinDelay);
        var _editMaxDelay = FindViewById<EditText>(Resource.Id.editMaxDelay);

        // 预设按钮：设置编辑框的值
        _btnPreset1?.Click += (s, e) =>
        {
            _editMinDelay!.Text = "2000";
            _editMaxDelay!.Text = "10000";
            Toast.MakeText(this, "已设置预设：2000 - 10000 毫秒", ToastLength.Short)?.Show();
        };

        _btnPreset2?.Click += (s, e) =>
        {
            _editMinDelay!.Text = "5000";
            _editMaxDelay!.Text = "30000";
            Toast.MakeText(this, "已设置预设：5000 - 30000 毫秒", ToastLength.Short)?.Show();
        };
        _btnPreset3?.Click += (s, e) =>
        {
            _editMinDelay!.Text = "4000";
            _editMaxDelay!.Text = "5000";
            Toast.MakeText(this, "已设置预设：4000 - 5000 毫秒", ToastLength.Short)?.Show();
        };

        _btnStart?.Click += (s, e) =>
        {
            int minDelay = int.TryParse(_editMinDelay?.Text, out var m) ? m : 2000;
            int maxDelay = int.TryParse(_editMaxDelay?.Text, out var x) ? x : 10000;
            if (minDelay >= maxDelay)
            {
                Toast.MakeText(this, "最小延时必须小于最大延时", ToastLength.Short)?.Show();
                return;
            }
            CallService(PackageInfo.ActionStart, "开始", minDelay, maxDelay);
            LaunchApp("com.ss.android.ugc.aweme.lite");
        };

        _btnStop?.Click += (s, e) =>
        {
            CallService(PackageInfo.ActionStop, "停止", 0, 0);
        };

        // 提示用户开启无障碍服务（可选）
        CheckAccessibilityPermission();
    }

    private void CallService(string action, string tooltip, int minDelay, int maxDelay)
    {
        var intent = new Intent(action);
        intent.SetPackage(PackageName);
        intent.PutExtra(PackageInfo.ExtraMinDelay, minDelay);
        intent.PutExtra(PackageInfo.ExtraMaxDelay, maxDelay);
        SendBroadcast(intent);
        Toast.MakeText(this, tooltip, ToastLength.Short)?.Show();
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