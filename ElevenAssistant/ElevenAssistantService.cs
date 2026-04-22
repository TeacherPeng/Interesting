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

    public void StartElevenAssistant(int minDelay, int maxDelay)
    {
        _minDelay = minDelay;
        _maxDelay = maxDelay;
        if (!_isActing)
        {
            _isActing = true;
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

    private void PerformAction()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.N) return;

        try
        {
            var nextDelay = Swipe();
            _handler?.PostDelayed(_actionRunnable, nextDelay);
        }
        catch (System.Exception ex)
        {
            Android.Util.Log.Error("Eleven Assistant", "Action failed: " + ex.Message);
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

    public override void OnAccessibilityEvent(AccessibilityEvent? e)
    {
    }

    public override void OnInterrupt()
    {
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
                _service.StartElevenAssistant(minDelay, maxDelay);
            }
            else if (intent?.Action == PackageInfo.ActionStop)
            {
                _service.StopElevenAssistant();
            }
        }
    }
}