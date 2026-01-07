using Android.AccessibilityServices;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views.Accessibility;
using Java.Lang;

namespace AutoSwipe;

[Service(Name = PackageInfo.ServiceName, Permission = "android.permission.BIND_ACCESSIBILITY_SERVICE", Exported = true)]
[IntentFilter(["android.accessibilityservice.AccessibilityService"])]
[MetaData("android.accessibilityservice", Resource = "@xml/accessibility_service_config")]
public class AutoSwipeService : AccessibilityService
{
    private Handler? _handler;
    private Runnable? _swipeRunnable;
    private bool _isSwiping = false;
    private BroadcastReceiver? _broadcastReceiver;
    private readonly Random _random = new();

    public override void OnCreate()
    {
        base.OnCreate();

        _handler = new Handler(Looper.MainLooper);
        _swipeRunnable = new Runnable(() =>
        {
            if (_isSwiping)
            {
                PerformSwipe();
                _handler.PostDelayed(_swipeRunnable, _random.Next(2000, 10001)); // 每2~10秒滑动一次
            }
        });

        // 注册广播接收器，MainActivity通过广播控制开始和停止
        _broadcastReceiver = new SwipeControlReceiver(this);
        var filter = new IntentFilter();
        filter.AddAction(PackageInfo.ActionStartSwipe);
        filter.AddAction(PackageInfo.ActionStopSwipe);
        RegisterReceiver(_broadcastReceiver, filter, ReceiverFlags.NotExported);
    }
    public override void OnDestroy()
    {
        StopAutoSwipe();
        if (_broadcastReceiver != null)
        {
            UnregisterReceiver(_broadcastReceiver);
            _broadcastReceiver = null;
        }
        base.OnDestroy();
    }

    public override void OnAccessibilityEvent(AccessibilityEvent? e)
    {
        // 判断是否是 WindowStateChanged 事件
        //if (e?.EventType == EventTypes.WindowStateChanged)
        //{
        //    // 此时通常表示新页面/Activity 已显示，可安全获取当前屏幕内容
        //    if (e.PackageName != "com.ss.android.ugc.aweme.lite") return;
        //    System.Diagnostics.Debug.WriteLine($"[WindowStateChanged] Package: {e.PackageName}, Class: {e.ClassName}");

        //    var root = GetRootInActiveWindow((int)PrefetchType.DescendantsBreadthFirst);
        //    if (root != null)
        //    {
        //        LogScreenContent(root, 1);
        //        root.Recycle(); // 别忘了回收
        //    }
        //}
    }

    public override void OnInterrupt() { }

    public void StartAutoSwipe()
    {
        if (!_isSwiping)
        {
            _isSwiping = true;
            _handler?.PostDelayed(_swipeRunnable, 1000); // 1秒后开始
        }
    }

    public void StopAutoSwipe()
    {
        _isSwiping = false;
        _handler?.RemoveCallbacks(_swipeRunnable);
    }

    private void PerformSwipe()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.N) return;

        try
        {
            // 偷懒：直接使用固定范围的随机坐标
            var gestureBuilder = new GestureDescription.Builder();
            int startX = _random.Next(450, 551);
            int startY = _random.Next(1400, 1501);
            int endX = _random.Next(450, 551);
            int endY = _random.Next(150, 251);
            var path = new Android.Graphics.Path();
            path.MoveTo(startX, startY);
            path.LineTo(endX, endY);

            var stroke = new GestureDescription.StrokeDescription(path, 0, 300);
            gestureBuilder.AddStroke(stroke);
            DispatchGesture(gestureBuilder.Build(), null, null);
        }
        catch (System.Exception ex)
        {
            Android.Util.Log.Error("AutoSwipe", "Swipe failed: " + ex.Message);
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
    private class SwipeControlReceiver(AutoSwipeService service) : BroadcastReceiver
    {
        private readonly AutoSwipeService _service = service;

        public override void OnReceive(Context? context, Intent? intent)
        {
            if (intent?.Action == PackageInfo.ActionStartSwipe)
            {
                _service.StartAutoSwipe();
            }
            else if (intent?.Action == PackageInfo.ActionStopSwipe)
            {
                _service.StopAutoSwipe();
            }
        }
    }
}