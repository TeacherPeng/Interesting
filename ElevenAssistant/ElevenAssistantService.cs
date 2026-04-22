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
    private int _minDelay = 4000;
    private int _maxDelay = 10000;

    // 控制开关
    private bool _enableSwipe = true;
    private bool _enableSchedule = true;

    // 预定执行时间（小时:分钟）
    private static readonly TimeOnly[] ScheduledTimes =
    [
        new TimeOnly(10, 0),
        new TimeOnly(12, 0),
        new TimeOnly(14, 0),
        new TimeOnly(16, 0),
        new TimeOnly(18, 0),
    ];
    private DateTime _nextClockInTime = DateTime.MaxValue;

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

    public override void OnAccessibilityEvent(AccessibilityEvent? e)
    {
        // 保持原有注释逻辑
    }

    public override void OnInterrupt() { }

    public void StartElevenAssistant(int minDelay, int maxDelay, bool enableSwipe, bool enableSchedule)
    {
        _minDelay = minDelay;
        _maxDelay = maxDelay;
        _enableSwipe = enableSwipe;
        _enableSchedule = enableSchedule;

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

        // 2. 去打卡：(928, 1571)
        _handler?.PostDelayed(new Runnable(() => Click("去打卡", 928, 1571)), interval);

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
        foreach (var time in ScheduledTimes)
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

    private void PerformAction()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.N) return;

        try
        {
            // 优先处理定时打卡（如果开启）
            if (_enableSchedule && DateTime.Now >= _nextClockInTime)
            {
                ClockIn();
                SelectClockInTime();
                return;
            }

            // 如果开启Swipe，则执行Swipe并根据返回间隔继续调度
            if (_enableSwipe)
            {
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
                _service.StartElevenAssistant(minDelay, maxDelay, enableSwipe, enableSchedule);
            }
            else if (intent?.Action == PackageInfo.ActionStop)
            {
                _service.StopElevenAssistant();
            }
        }
    }
}