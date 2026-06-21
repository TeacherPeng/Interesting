using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Widget;

namespace ElevenAssistantV2;

public static class PackageInfo
{
    public const string PackageName = "com.pengsw.elevenassistantv2";
    public const string ServiceName = $"{PackageName}.elevenassistantv2Service";
    public const string ActionStart = $"{PackageName}.START_ACTION";
    public const string ActionStop = $"{PackageName}.STOP_ACTION";
    public const string ExtraMinDelay = "min_delay";
    public const string ExtraMaxDelay = "max_delay";
    public const string ExtraEnableSwipe = "enable_swipe";
    public const string ExtraEnableSchedule = "enable_schedule";
    public const string ExtraAdverOnly = "adver_only";
    public const string ExtraStartTime = "start_time";
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
        var _chkEnableSwipe = FindViewById<CheckBox>(Resource.Id.chkEnableSwipe);
        var _chkEnableSchedule = FindViewById<CheckBox>(Resource.Id.chkEnableSchedule);
        var _chkAdverOnly = FindViewById<CheckBox>(Resource.Id.chkAdverOnly);

        int[] scheduledTimeViewIds =
        [
            Resource.Id.txtScheduledTime1,
            Resource.Id.txtScheduledTime2,
            Resource.Id.txtScheduledTime3,
            Resource.Id.txtScheduledTime4,
            Resource.Id.txtScheduledTime5,
        ];
        for (int i = 0; i < ElevenAssistantV2Service.ScheduledTimes.Length && i < scheduledTimeViewIds.Length; i++)
        {
            FindViewById<TextView>(scheduledTimeViewIds[i])!.Text =
                ElevenAssistantV2Service.ScheduledTimes[i].ToString("HH:mm");
        }

        // 预设按钮：设置编辑框的值
        _btnPreset1?.Click += (s, e) =>
        {
            _editMinDelay!.Text = "4000";
            _editMaxDelay!.Text = "20000";
            Toast.MakeText(this, "已设置预设：4 - 20秒", ToastLength.Short)?.Show();
        };

        _btnPreset2?.Click += (s, e) =>
        {
            _editMinDelay!.Text = "10000";
            _editMaxDelay!.Text = "30000";
            Toast.MakeText(this, "已设置预设：10 - 30秒", ToastLength.Short)?.Show();
        };
        _btnPreset3?.Click += (s, e) =>
        {
            _editMinDelay!.Text = "2000";
            _editMaxDelay!.Text = "5000";
            Toast.MakeText(this, "已设置预设：2 - 5秒", ToastLength.Short)?.Show();
        };

        _btnStart?.Click += (s, e) =>
        {
            int minDelay = int.TryParse(_editMinDelay?.Text, out var m) ? m : 2000;
            int maxDelay = int.TryParse(_editMaxDelay?.Text, out var x) ? x : 5000;
            if (minDelay >= maxDelay)
            {
                Toast.MakeText(this, "最小延时必须小于最大延时", ToastLength.Short)?.Show();
                return;
            }

            // 引导用户关闭电池优化
            if (!CheckBatteryOptimization()) return;

            // 提示用户开启无障碍服务（可选）
            if (!CheckAccessibilityPermission()) return;

            bool enableSwipe = _chkEnableSwipe?.Checked ?? true;
            bool enableSchedule = _chkEnableSchedule?.Checked ?? true;
            bool adverOnly = _chkAdverOnly?.Checked ?? false;

            CallService(PackageInfo.ActionStart, "开始", minDelay, maxDelay, enableSwipe, enableSchedule, adverOnly);
            LaunchApp("com.ss.android.ugc.aweme.lite");
        };

        _btnStop?.Click += (s, e) =>
        {
            // 停止时把所有开关设置为 false（服务收到停止广播后会停止动作）
            CallService(PackageInfo.ActionStop, "停止", 0, 0, false, false, false);
        };

    }

    private void CallService(string action, string tooltip, int minDelay, int maxDelay, bool enableSwipe, bool enableSchedule, bool adverOnly)
    {
        var intent = new Intent(action);
        intent.SetPackage(PackageName);
        intent.PutExtra(PackageInfo.ExtraMinDelay, minDelay);
        intent.PutExtra(PackageInfo.ExtraMaxDelay, maxDelay);
        intent.PutExtra(PackageInfo.ExtraEnableSwipe, enableSwipe);
        intent.PutExtra(PackageInfo.ExtraEnableSchedule, enableSchedule);
        intent.PutExtra(PackageInfo.ExtraAdverOnly, adverOnly);
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

    private bool CheckAccessibilityPermission()
    {
        var enabledServices = Settings.Secure.GetString(ContentResolver, Settings.Secure.EnabledAccessibilityServices);
        var serviceName = $"{PackageInfo.PackageName}/{PackageInfo.ServiceName}";
        var accessibilityEnabled = !string.IsNullOrEmpty(enabledServices) && enabledServices.Contains(serviceName);
        if (accessibilityEnabled) return true;

        // 跳转到无障碍设置页
        Toast.MakeText(this, "请先在设置中启用无障碍服务！", ToastLength.Long)?.Show();
        var intent = new Intent(Android.Provider.Settings.ActionAccessibilitySettings);
        StartActivity(intent);
        return false;
    }

    private bool CheckBatteryOptimization()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.M) return true;

        var powerManager = (PowerManager?)GetSystemService(PowerService);
        if (powerManager == null) return true;

        if (powerManager.IsIgnoringBatteryOptimizations(PackageName)) return true;

        Toast.MakeText(this, "请允许忽略电池优化以保证后台运行", ToastLength.Long)?.Show();
        var intent = new Intent(Android.Provider.Settings.ActionRequestIgnoreBatteryOptimizations);
        intent.SetData(Android.Net.Uri.Parse($"package:{PackageName}"));
        StartActivity(intent);
        return false;
    }
}