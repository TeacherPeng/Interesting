using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace AndroidTarget
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // 使用自定义触摸视图替代默认布局，显示划屏轨迹与点击位置
            SetContentView(new TouchView(this));
        }

        // 自定义视图：响应触摸以绘制划屏轨迹（Path）和点击位置（圆点）
        private class TouchView : View
        {
            readonly Paint _pathPaint;
            readonly Paint _tapPaint;

            readonly List<Android.Graphics.Path> _paths = new();
            readonly List<PointF> _taps = new();

            Android.Graphics.Path? _currentPath;
            float _downX, _downY;
            readonly int _touchSlop;
            readonly Handler _handler;
            readonly Java.Lang.IRunnable _clearRunnable;
            const long ClearDelayMs = 2000;

            public TouchView(Context context) : base(context)
            {
                // 路径画笔
                _pathPaint = new Paint
                {
                    AntiAlias = true,
                    StrokeWidth = 8f,
                    Color = Color.Cyan,
                    StrokeJoin = Paint.Join.Round,
                    StrokeCap = Paint.Cap.Round,
                };
                _pathPaint.SetStyle(Paint.Style.Stroke);
                // 点击点画笔
                _tapPaint = new Paint
                {
                    AntiAlias = true,
                    Color = Color.Magenta,
                };
                _tapPaint.SetStyle(Paint.Style.Fill);

                _touchSlop = ViewConfiguration.Get(context).ScaledTouchSlop;
                _handler = new Handler(Looper.MainLooper);
                _clearRunnable = new Java.Lang.Runnable(() =>
                {
                    _paths.Clear();
                    _taps.Clear();
                    Invalidate();
                });

                // 设置视图背景颜色为 #FFC641
                SetBackgroundColor(Color.ParseColor("#FFC641"));

                // 启用触摸
                Focusable = true;
                FocusableInTouchMode = true;
            }

            public override bool OnTouchEvent(MotionEvent e)
            {
                switch (e.ActionMasked)
                {
                    case MotionEventActions.Down:
                        _handler.RemoveCallbacks(_clearRunnable);

                        _downX = e.GetX();
                        _downY = e.GetY();

                        _currentPath = new Android.Graphics.Path();
                        _currentPath.MoveTo(_downX, _downY);
                        // 临时将当前 path 添加到集合，若最后判断为点击可移除
                        _paths.Add(_currentPath);
                        Invalidate();
                        return true;

                    case MotionEventActions.Move:
                        if (_currentPath != null)
                        {
                            // 使用平滑线段
                            for (int i = 0; i < e.PointerCount; i++)
                            {
                                float mx = e.GetX(i);
                                float my = e.GetY(i);
                                _currentPath.LineTo(mx, my);
                            }
                            Invalidate();
                        }
                        return true;

                    case MotionEventActions.Up:
                    case MotionEventActions.Cancel:
                        float upX = e.GetX();
                        float upY = e.GetY();
                        float dx = upX - _downX;
                        float dy = upY - _downY;
                        float distSq = dx * dx + dy * dy;

                        // 判断是否为点击：位移小于阈值
                        if (distSq <= (_touchSlop * _touchSlop))
                        {
                            // 视为点击：移除刚才添加的空路径，记录点击点
                            if (_currentPath != null && _paths.Count > 0)
                            {
                                _paths.Remove(_currentPath);
                            }
                            _taps.Add(new PointF(upX, upY));
                        }
                        else
                        {
                            // 视为划屏：保留当前路径（已经在集合中）
                        }

                        _currentPath = null;

                        // 安排若干秒后清除展示
                        _handler.RemoveCallbacks(_clearRunnable);
                        _handler.PostDelayed(_clearRunnable, ClearDelayMs);
                        Invalidate();
                        return true;
                }

                return base.OnTouchEvent(e);
            }

            protected override void OnDraw(Canvas canvas)
            {
                base.OnDraw(canvas);

                // 绘制所有划屏轨迹
                foreach (var p in _paths)
                {
                    canvas.DrawPath(p, _pathPaint);
                }

                // 绘制所有点击位置（圆点）
                const float tapRadius = 30f;
                foreach (var t in _taps)
                {
                    canvas.DrawCircle(t.X, t.Y, tapRadius, _tapPaint);
                }
            }
        }
    }
}