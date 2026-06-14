using Android.AccessibilityServices;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Display;
using Android.OS;
using Android.Views;
using Android.Views.Accessibility;
using Java.Lang;

namespace ElevenAssistantV2;

[Service(Name = PackageInfo.ServiceName, Permission = "android.permission.BIND_ACCESSIBILITY_SERVICE", Exported = true)]
[IntentFilter(["android.accessibilityservice.AccessibilityService"])]
[MetaData("android.accessibilityservice", Resource = "@xml/accessibility_service_config")]
public class ElevenAssistantV2Service : AccessibilityService
{
    private Handler? _handler;
    private Runnable? _actionRunnable;
    private bool _isActing = false;
    private BroadcastReceiver? _broadcastReceiver;
    private readonly Random _random = new();
    private int _minDelay = 10000;
    private int _maxDelay = 30000;

    // 控制开关
    private bool _enableSwipeCoin = true;
    private bool _enableSchedule = true;
    private bool _adverOnly = false;

    private TimeOnly[] _scheduledTimes = [
        TimeOnly.Parse("9:10"),
        TimeOnly.Parse("11:10"),
        TimeOnly.Parse("13:10"),
        TimeOnly.Parse("15:10"),
        TimeOnly.Parse("17:10"),
    ];
    private DateTime _nextClockInTime = DateTime.MaxValue;

    // 屏幕检测参数
    private const int CheckX = 956;
    private const int CheckY = 718;
    private static readonly int GoldColor = Android.Graphics.Color.ParseColor("#FFC641");
    private static readonly int RedColor = Android.Graphics.Color.ParseColor("#FF3F54");
    private const int ColorThreshold = 60; // 判断“接近”的阈值（RGB欧氏距离）

    public override void OnCreate()
    {
        base.OnCreate();

        _handler = new Handler(Looper.MainLooper);
        _actionRunnable = new Runnable(async () =>
        {
            if (_isActing)
            {
                await PerformActionAsync();
            }
        });

        // 注册广播接收器，MainActivity通过广播控制开始和停止
        _broadcastReceiver = new ActionControlReceiver(this);
        var filter = new IntentFilter();
        filter.AddAction(PackageInfo.ActionStart);
        filter.AddAction(PackageInfo.ActionStop);
        RegisterReceiver(_broadcastReceiver, filter, ReceiverFlags.NotExported);
    }
    public override void OnDestroy()
    {
        StopElevenAssistantV2();
        if (_broadcastReceiver != null)
        {
            UnregisterReceiver(_broadcastReceiver);
            _broadcastReceiver = null;
        }
        base.OnDestroy();
    }

    public override void OnAccessibilityEvent(AccessibilityEvent? e) { }
    public override void OnInterrupt() { }

    public void StartElevenAssistantV2(int minDelay, int maxDelay, bool enableSwipe, bool enableSchedule, bool adverOnly, string startTime)
    {
        _minDelay = minDelay;
        _maxDelay = maxDelay;
        _enableSwipeCoin = enableSwipe;
        _enableSchedule = enableSchedule;
        _adverOnly = adverOnly;
        _monitor_time = DateTime.Now;
        // 异步等待截屏和颜色判定结果
        // 调用系统的截屏接口（在无障碍服务中静默执行）
        // 1. 获取默认的 Display Manager
        displayManager = (DisplayManager)GetSystemService(Context.DisplayService);
        defaultDisplay = displayManager.GetDisplay(Display.DefaultDisplay);

        // 2. 基于当前 Service 的 Context 和默认 Display，创建一个关联了显示的 Visual Context
        // 注意：此方法需要 Android 11 (API 30) 及以上版本支持
        visualContext = CreateDisplayContext(defaultDisplay);

        if (!_isActing)
        {
            _isActing = true;
            if (_enableSchedule)
                SelectClockInTime();
            else
                _nextClockInTime = DateTime.MaxValue;

            _handler?.RemoveCallbacksAndMessages(null);
            _handler?.PostDelayed(_actionRunnable, 1000);
        }
    }

    public void StopElevenAssistantV2()
    {
        _isActing = false;
        // remove all pending callbacks and messages to ensure no scheduled actions remain
        _handler?.RemoveCallbacksAndMessages(null);

        // 释放与显示相关的资源以避免内存泄漏（如果已创建）
        try
        {
            if (visualContext != null)
            {
                visualContext.Dispose();
                visualContext = null;
            }
        }
        catch (System.Exception ex)
        {
            Android.Util.Log.Warn("Eleven Assistant", "Dispose visualContext failed: " + ex.Message);
        }

        try
        {
            if (defaultDisplay != null)
            {
                defaultDisplay.Dispose();
                defaultDisplay = null;
            }
        }
        catch (System.Exception ex)
        {
            Android.Util.Log.Warn("Eleven Assistant", "Dispose defaultDisplay failed: " + ex.Message);
        }

        try
        {
            if (displayManager != null)
            {
                displayManager.Dispose();
                displayManager = null;
            }
        }
        catch (System.Exception ex)
        {
            Android.Util.Log.Warn("Eleven Assistant", "Dispose displayManager failed: " + ex.Message);
        }
    }

    private long Swipe()
    {
        var gestureBuilder = new GestureDescription.Builder();

        // 在指定范围内随机生成手势的起点和终点坐标
        int startX = _random.Next(450, 551);
        int startY = _random.Next(1500, 1601);
        int endX = _random.Next(450, 551);
        int endY = _random.Next(800, 1000);

        // 生成手势轨迹
        var path = new Android.Graphics.Path();
        path.MoveTo(startX, startY);

        int steps = _random.Next(18, 32);
        double freq = _random.NextDouble() * 2.0 + 2.0;
        float amplitude = _random.Next(6, 16);

        for (int i = 1; i <= steps; i++)
        {
            float t = (float)i / steps; // 0..1

            // linear interpolation between start and end
            float baseX = startX + (endX - startX) * t;
            float baseY = startY + (endY - startY) * t;

            // sine wave on X to simulate tremor, plus small random noise on both axes
            double sine = System.Math.Sin(t * freq * 2.0 * System.Math.PI);
            float jitterX = (float)(sine * amplitude + (_random.NextDouble() * 4.0 - 2.0));
            float jitterY = (float)(_random.NextDouble() * 4.0 - 2.0);

            float px = baseX + jitterX;
            float py = baseY + jitterY;

            path.LineTo(px, py);
        }

        long duration = _random.Next(250, 401);
        var stroke = new GestureDescription.StrokeDescription(path, 0, duration);
        gestureBuilder.AddStroke(stroke);

        Android.Util.Log.Debug("Elevent Assistant", "Dispatching swipe gesture");
        DispatchGesture(gestureBuilder.Build(), null, null);

        return _random.Next(_minDelay, _maxDelay + 1);
    }

    private void Click(string prompt, int x, int y)
    {
        // simple tap gesture at the specified coordinates
        var gestureBuilder = new GestureDescription.Builder();
        var path = new Android.Graphics.Path();
        path.MoveTo(x, y);
        path.LineTo(x, y);
        var stroke = new GestureDescription.StrokeDescription(path, 0, 50);
        gestureBuilder.AddStroke(stroke);
        DispatchGesture(gestureBuilder.Build(), null, null);
    }

    private void ClockIn()
    {
        // perform a sequence of taps with 2 seconds interval
        int interval = 5000; // ms

        // 1. 赚钱： (540, 2265)
        Click("赚钱", 540, 2265);

        // 2. 去打卡：(928, 1513)
        _handler?.PostDelayed(new Runnable(() => Click("去打卡", 928, 1513)), interval); // 第一行1271，第二行1513，第三行1783

        // 3. 打卡：(554,2165)
        interval += 4000;
        _handler?.PostDelayed(new Runnable(() => Click("打卡", 554, 2165)), interval);

        // 4. 指定页面打卡：(888, 1476)
        interval += 4000;
        _handler?.PostDelayed(new Runnable(() => Click("指定页面打卡", 888, 1476)), interval);

        // 执行完指定页面打卡后，连续执行4次回退操作（间隔500ms）
        int backStartDelay = interval + 4000;
        _handler?.PostDelayed(new Runnable(() => PerformGlobalAction(GlobalAction.Back)), backStartDelay);
        backStartDelay += 800;
        _handler?.PostDelayed(new Runnable(() => PerformGlobalAction(GlobalAction.Back)), backStartDelay);
        backStartDelay += 800;
        _handler?.PostDelayed(new Runnable(() => PerformGlobalAction(GlobalAction.Back)), backStartDelay);
        backStartDelay += 800;
        _handler?.PostDelayed(new Runnable(() => PerformGlobalAction(GlobalAction.Back)), backStartDelay);
        backStartDelay += 800;
        _handler?.PostDelayed(new Runnable(() => PerformGlobalAction(GlobalAction.Back)), backStartDelay);

        // 继续_actionRunnable的执行
        backStartDelay += 800;
        _handler?.PostDelayed(_actionRunnable, backStartDelay);
    }

    private void SelectClockInTime()
    {
        var now = TimeOnly.FromDateTime(DateTime.Now);
        foreach (var time in _scheduledTimes)
        {
            if (now < time)
            {
                _nextClockInTime = DateTime.Today.Add(time.ToTimeSpan());
                Toast.MakeText(this, $"下次打卡时间: {time:hh\\:mm}", ToastLength.Long)?.Show();
                return;
            }
        }
        _nextClockInTime = DateTime.MaxValue; // 不再执行，直到第二天重启服务
    }

    private void AdvertiseClick()
    {
        // 1. 点击看广告
        Click("看广告", 883, 958);

        // 2. 点击关闭广告
        _handler?.PostDelayed(new Runnable(() => Click("关闭广告", 981, 156)), 35000);

        // 预约下一次看广告
        _handler?.PostDelayed(new Runnable(() => AdvertiseClick()), (11 * 60) * 1000);
    }

    private DateTime _monitor_time = DateTime.Now;
    private DisplayManager? displayManager;
    private Display? defaultDisplay;
    private Context? visualContext;
    private async Task PerformActionAsync()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.N) return;

        try
        {
            if (_adverOnly)
            {
                AdvertiseClick();
                return;
            }

            if (_enableSchedule && DateTime.Now >= _nextClockInTime)
            {
                ClockIn();
                SelectClockInTime();
                return;
            }

            if (_enableSwipeCoin)
            {
                bool isTarget = await PixelColorIsTarget(RedColor);

                if (isTarget)
                {
                    Android.Util.Log.Debug("Elevent Assistant", "Pixel is target color, performing swipe");
                    Swipe();
                }
                else
                {
                    Android.Util.Log.Debug("Elevent Assistant", "Pixel is not target color, counting...");
                    if (DateTime.Now - _monitor_time > TimeSpan.FromSeconds(30))
                    {
                        Android.Util.Log.Debug("Elevent Assistant", "Pixel has been not target color for too long, performing swipe");
                        _monitor_time = DateTime.Now;
                        Swipe();
                    }
                }
                var nextDelay = _random.Next(_minDelay, _maxDelay);
                _handler?.PostDelayed(_actionRunnable, nextDelay);
                return;
            }
            else
            {
                var nextDelay = Swipe();
                _handler?.PostDelayed(_actionRunnable, nextDelay); return;
            }
        }
        catch (System.Exception ex)
        {
            Android.Util.Log.Error("Eleven Assistant", "Action failed: " + ex.Message);
        }
    }

    // 检查屏幕指定像素，符合条件时返回 true
    private async Task<bool> PixelColorIsTarget(int aTargetColor)
    {
        // TakeScreenshot 需要 Android 11 (API 30) 及以上
        if (Build.VERSION.SdkInt < BuildVersionCodes.R)
        {
            Android.Util.Log.Warn("Eleven Assistant", "TakeScreenshot requires Android 11 (API 30) or higher.");
            return false;
        }

        var tcs = new TaskCompletionSource<bool>();

        var callback = new ScreenshotCallback(
            onSuccess: screenshot =>
            {
                try
                {
                    using var hwBuffer = screenshot.HardwareBuffer;
                    using var hardwareBitmap = Bitmap.WrapHardwareBuffer(hwBuffer, screenshot.ColorSpace);

                    // 将硬件 Bitmap 转换为软件 Bitmap 以便读取像素
                    using var softwareBitmap = hardwareBitmap?.Copy(Bitmap.Config.Argb8888, false);

                    if (softwareBitmap != null)
                    {
                        int pixelColor = softwareBitmap.GetPixel(CheckX, CheckY);
                        bool isClose = IsCloseColor(pixelColor, aTargetColor, ColorThreshold);
                        tcs.TrySetResult(isClose);
                    }
                    else
                    {
                        tcs.TrySetResult(false);
                    }
                }
                catch (System.Exception ex)
                {
                    Android.Util.Log.Error("Eleven Assistant", "截图解析异常: " + ex.Message);
                    tcs.TrySetResult(false);
                }
            },
            onFailure: errorCode =>
            {
                Android.Util.Log.Error("Eleven Assistant", "截图失败，错误码: " + errorCode);
                tcs.TrySetResult(false);
            }
        );

        TakeScreenshot(visualContext.DeviceId, MainExecutor, callback);

        return await tcs.Task;
    }

    // 判断两个颜色在RGB空间的欧氏距离是否小于阈值
    private static bool IsCloseColor(int colorA, int colorB, int threshold)
    {
        // 从 ARGB 整数中提取 R/G/B 分量（忽略 Alpha）
        int r1 = (colorA >> 16) & 0xFF;
        int g1 = (colorA >> 8) & 0xFF;
        int b1 = colorA & 0xFF;

        int r2 = (colorB >> 16) & 0xFF;
        int g2 = (colorB >> 8) & 0xFF;
        int b2 = colorB & 0xFF;

        int dr = r1 - r2;
        int dg = g1 - g2;
        int db = b1 - b2;
        int distSq = dr * dr + dg * dg + db * db;
        Android.Util.Log.Debug("Eleven Assistant", $"Pixel color: #{r1:X2}{g1:X2}{b1:X2}, DistanceSq: {distSq}");
        return distSq <= threshold * threshold;
    }

    // 内部广播接收器
    private class ActionControlReceiver(ElevenAssistantV2Service service) : BroadcastReceiver
    {
        private readonly ElevenAssistantV2Service _service = service;

        public override void OnReceive(Context? context, Intent? intent)
        {
            if (intent?.Action == PackageInfo.ActionStart)
            {
                int minDelay = intent.GetIntExtra(PackageInfo.ExtraMinDelay, 4000);
                int maxDelay = intent.GetIntExtra(PackageInfo.ExtraMaxDelay, 10000);
                bool enableSwipe = intent.GetBooleanExtra(PackageInfo.ExtraEnableSwipe, true);
                bool enableSchedule = intent.GetBooleanExtra(PackageInfo.ExtraEnableSchedule, true);
                bool adverOnly = intent.GetBooleanExtra(PackageInfo.ExtraAdverOnly, false);
                string startTime = intent.GetStringExtra(PackageInfo.ExtraStartTime) ?? "8:40";
                _service.StartElevenAssistantV2(minDelay, maxDelay, enableSwipe, enableSchedule, adverOnly, startTime);
            }
            else if (intent?.Action == PackageInfo.ActionStop)
            {
                _service.StopElevenAssistantV2();
            }
        }
    }

    private class ScreenshotCallback : Java.Lang.Object, AccessibilityService.ITakeScreenshotCallback
    {
        private readonly Action<AccessibilityService.ScreenshotResult> _onSuccess;
        private readonly Action<int> _onFailure;

        public ScreenshotCallback(Action<AccessibilityService.ScreenshotResult> onSuccess, Action<int> onFailure)
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
        }

        public void OnSuccess(AccessibilityService.ScreenshotResult screenshot) => _onSuccess?.Invoke(screenshot);

        public void OnFailure(int errorCode) => _onFailure?.Invoke(errorCode);
    }
}
