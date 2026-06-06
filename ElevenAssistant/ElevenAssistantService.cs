using Android.AccessibilityServices;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views.Accessibility;
using Java.Lang;
using System;

namespace ElevenAssistant;

[Service(Name = PackageInfo.ServiceName, Permission = "android.permission.BIND_ACCESSIBILITY_SERVICE", Exported = true)]
[IntentFilter(["android.accessibilityservice.AccessibilityService"])]
[MetaData("android.accessibilityservice", Resource = "@xml/accessibility_service_config")]
public class ElevenAssistantService : AccessibilityService
{
    private Handler? _handler;
    private Runnable? _actionRunnable;
    private bool _isActing = false;
    private BroadcastReceiver? _broadcastReceiver;
    private readonly Random _random = new();
    private int _minDelay = 10000;
    private int _maxDelay = 30000;

    // 控制开关
    private bool _enableSwipe = true;
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
        _actionRunnable = new Runnable(() =>
        {
            if (_isActing)
            {
                PerformAction();
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
        StopElevenAssistant();
        if (_broadcastReceiver != null)
        {
            UnregisterReceiver(_broadcastReceiver);
            _broadcastReceiver = null;
        }
        base.OnDestroy();
    }

    public override void OnAccessibilityEvent(AccessibilityEvent? e) { }
    public override void OnInterrupt() { }

    public void StartElevenAssistant(int minDelay, int maxDelay, bool enableSwipe, bool enableSchedule, bool adverOnly, string startTime)
    {
        _minDelay = minDelay;
        _maxDelay = maxDelay;
        _enableSwipe = enableSwipe;
        _enableSchedule = enableSchedule;
        _adverOnly = adverOnly;

        if (!_isActing)
        {
            _isActing = true;
            if (_enableSchedule)
                SelectClockInTime();
            else
                _nextClockInTime = DateTime.MaxValue;

            _handler?.RemoveCallbacksAndMessages(null);
            _handler?.PostDelayed(_actionRunnable, 1000);
            //var aPrompt = _adverOnly ? "开始点广告" : (_enableSchedule ? "开始定时打卡" : "开始刷视频");
            //Toast.MakeText(this, aPrompt, ToastLength.Short)?.Show();
        }
    }

    public void StopElevenAssistant()
    {
        _isActing = false;
        // remove all pending callbacks and messages to ensure no scheduled actions remain
        _handler?.RemoveCallbacksAndMessages(null);
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
        Toast.MakeText(this, prompt, ToastLength.Long)?.Show();
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
        Toast.MakeText(this, "今日打卡已完成，等待明天", ToastLength.Long)?.Show();
        _nextClockInTime = DateTime.MaxValue; // 不再执行，直到第二天重启服务
    }

    private void AdvertiseClick()
    {
        // 1. 点击看广告
        Click("看广告", 883, 958);

        // 2. 点击关闭广告
        _handler?.PostDelayed(new Runnable(() => Click("关闭广告", 981, 156)), 35000);

        // 预约下一次看广告
        Toast.MakeText(this, "10分钟后看下一个广告", ToastLength.Long)?.Show();
        _handler?.PostDelayed(new Runnable(() => AdvertiseClick()), (11 * 60) * 1000);
    }

    private void PerformAction()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.N) return;

        try
        {
            // 优先处理点广告（如果开启）
            if (_adverOnly)
            {
                AdvertiseClick();
                return;
            }

            // 次优先处理定时打卡（如果开启）
            if (_enableSchedule && DateTime.Now >= _nextClockInTime)
            {
                ClockIn();
                SelectClockInTime();
                return;
            }

            // 如果开启Swipe，则按新策略：每秒检查指定像素颜色决定是否Swipe
            if (_enableSwipe)
            {
                //Toast.MakeText(this, "检查屏幕状态决定是否执行Swipe...", ToastLength.Short)?.Show();
                //MonitorPixelAndMaybeSwipe();
                var nextDelay = Swipe();
                _handler?.PostDelayed(_actionRunnable, nextDelay);
                return;
            }

            // 如果既不开启Swipe也不开启定时任务，则仅短暂轮询等待（避免忙循环）
            _handler?.PostDelayed(_actionRunnable, 2000);
        }
        catch (System.Exception ex)
        {
            Android.Util.Log.Error("Eleven Assistant", "Action failed: " + ex.Message);
        }
    }

    // 每秒检查屏幕指定像素，符合条件时执行Swipe
    private void MonitorPixelAndMaybeSwipe()
    {
        // 最大等待次数30次（约30秒）
        const int maxChecks = 30;
        int checks = 0;

        // 每次检查的Runnable
        Runnable checkRunnable = null!;
        checkRunnable = new Runnable(() =>
        {
            checks++;
            // 使用 AccessibilityService.TakeScreenshot API (API 30+) 进行截图
            try
            {
                // 使用单线程执行器来运行回调
                var executor = Java.Util.Concurrent.Executors.NewSingleThreadExecutor();
#pragma warning disable CA1416 // 验证平台兼容性
                TakeScreenshot(0, executor, new ScreenshotCallback((screenshot) =>
                {
                    if (screenshot == null)
                    {
                        Toast.MakeText(this, "截图失败，刷屏...", ToastLength.Short)?.Show();
                        DoSwipeAndScheduleNext();
                        return;
                    }

                    // 尝试从 ScreenshotResult 获取 Bitmap（API 30+，binding 可能支持 GetBitmap 或 Bitmap 属性）
                    Android.Graphics.Bitmap? bmp = null;
                    try
                    {
                        // 某些 platform binding 使用 GetBitmap(), 有的使用 Bitmap 属性；分支尝试两种
                        var getBitmapMethod = screenshot.Class.GetMethod("getBitmap");
                        if (getBitmapMethod != null)
                        {
                            // 尝试调用 getBitmap()
                            var obj = getBitmapMethod.Invoke(screenshot);
                            bmp = obj as Android.Graphics.Bitmap;
                        }
                    }
                    catch
                    {
                        // 忽略反射失败
                        try
                        {
                            bmp = screenshot.GetType().GetProperty("Bitmap")?.GetValue(screenshot) as Android.Graphics.Bitmap;
                        }
                        catch { bmp = null; }
                    }

                    if (bmp == null)
                    {
                        Toast.MakeText(this, "无法获取截图内容，刷屏...", ToastLength.Short)?.Show();
                        // 无法取得bitmap，视为失败继续或超时处理
                        DoSwipeAndScheduleNext();
                        return;
                    }

                    try
                    {
                        // 检查像素颜色
                        int pixelColor = 0;
                        try
                        {
                            // 防止越界异常
                            int px = System.Math.Max(0, System.Math.Min(bmp.Width - 1, CheckX));
                            int py = System.Math.Max(0, System.Math.Min(bmp.Height - 1, CheckY));
                            pixelColor = bmp.GetPixel(px, py);
                        }
                        catch
                        {
                            pixelColor = 0;
                        }

                        if (IsCloseColor(pixelColor, GoldColor, ColorThreshold))
                        {
                            // 发现“接近金色”，按要求继续等待，不执行Swipe，本次周期结束
                            // 重新安排下次检查（保持轮询）
                            _handler?.PostDelayed(checkRunnable, 1000);
                        }
                        else
                        {
                            DoSwipeAndScheduleNext();
                        }
                    }
                    finally
                    {
                        try { bmp.Recycle(); } catch { }
                    }
                }));
#pragma warning restore CA1416 // 验证平台兼容性
            }
            catch (System.Exception ex)
            {
                Android.Util.Log.Warn("Eleven Assistant", "Screenshot failed: " + ex.Message);
                if (checks >= maxChecks)
                {
                    DoSwipeAndScheduleNext();
                }
                else
                {
                    _handler?.PostDelayed(checkRunnable, 1000);
                }
            }
        });

        // 启动第一次检查
        _handler?.Post(checkRunnable);
    }

    // 实际执行 Swipe 并根据返回的延时安排下次执行
    private void DoSwipeAndScheduleNext()
    {
        var nextDelay = Swipe();
        _handler?.PostDelayed(_actionRunnable, 1000);
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
        return distSq <= threshold * threshold;
    }

    private static void LogScreenContent(AccessibilityNodeInfo node, int level)
    {
        if (node == null) return;
        
        var bounds = new Rect();
        node.GetBoundsInScreen(bounds);

        // 打印当前节点信息
        System.Diagnostics.Debug.WriteLine($"{new string(' ', level + level)}, {node.Text}, {node.ClassName}, {node.ViewIdResourceName}");
        if (bounds.CenterX() > 800 || bounds.CenterY() > 2000)
            System.Diagnostics.Debug.WriteLine($"{new string(' ', level + level)}, at ({bounds.CenterX()}, {bounds.CenterY()})");
        //if (level >= 10) return;

        // 递归遍历子节点
        for (int i = 0; i < node.ChildCount; i++)
        {
            var child = node.GetChild(i);
            if (child != null)
            {
                LogScreenContent(child, level + 1);
                // 注意：使用完必须回收，避免内存泄漏
                child.Recycle(); // ⚠️ 重要！
            }
        }
    }

    private static AccessibilityNodeInfo? FindNodeByText(AccessibilityNodeInfo root, string text)
    {
        if (root == null || string.IsNullOrEmpty(text))
            return null;

        // 递归查找包含指定文本的节点
        if (root.Text != null && root.Text.ToString() == text)
            return root;

        for (int i = 0; i < root.ChildCount; i++)
        {
            var child = root.GetChild(i);
            var result = FindNodeByText(child, text);
            if (result != null)
            {
                // 注意：不要回收 result，因为外部会用到
                child?.Recycle();
                return result;
            }
            child?.Recycle();
        }
        return null;
    }

    // 内部广播接收器
    private class ActionControlReceiver(ElevenAssistantService service) : BroadcastReceiver
    {
        private readonly ElevenAssistantService _service = service;

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
                _service.StartElevenAssistant(minDelay, maxDelay, enableSwipe, enableSchedule, adverOnly, startTime);
            }
            else if (intent?.Action == PackageInfo.ActionStop)
            {
                _service.StopElevenAssistant();
            }
        }
    }

    // 截图回调实现（简化，成功/失败分别回调）
    private class ScreenshotCallback : Java.Lang.Object, AccessibilityService.ITakeScreenshotCallback
    {
        private readonly Action<Android.AccessibilityServices.AccessibilityService.ScreenshotResult?> _onSuccess;

        public ScreenshotCallback(Action<Android.AccessibilityServices.AccessibilityService.ScreenshotResult?> onSuccess)
        {
            _onSuccess = onSuccess;
        }

        public void OnSuccess(Android.AccessibilityServices.AccessibilityService.ScreenshotResult screenshot)
        {
            try
            {
                _onSuccess?.Invoke(screenshot);
            }
            catch { }
        }

        public void OnFailure()
        {
            _onSuccess?.Invoke(null);
        }

        public void OnFailure(int errorCode)
        {
            //throw new NotImplementedException();
        }
    }
}