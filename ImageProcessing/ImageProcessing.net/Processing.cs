using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace ImageProcessing
{
    /// <summary>
    /// 图像处理基类（OpenCvSharp 5 版本）。
    /// 负责 BitmapImage 与 Mat(BGR) 之间的转换，子类只需实现 Mat -&gt; Mat 的处理逻辑。
    /// </summary>
    public abstract class Processing
    {
        public abstract string Name { get; }

        /// <summary>
        /// 核心处理逻辑：输入为 3 通道 BGR 的 Mat，返回处理结果 Mat。
        /// 返回的 Mat 由基类负责释放。
        /// </summary>
        protected abstract Mat ProcessImage(Mat aSource);

        public virtual UserControl? Control { get { return null; } }

        public virtual BitmapSource GetResultImage(BitmapImage aSourceImage)
        {
            using Mat aSource = BitmapSourceConverter.ToMat(aSourceImage);
            using Mat aBgr = ToBgr(aSource);
            using Mat aResult = ProcessImage(aBgr);
            return BitmapSourceConverter.ToBitmapSource(aResult);
        }

        /// <summary>把任意通道数的源 Mat 统一转换为 3 通道 BGR。</summary>
        private static Mat ToBgr(Mat aSource)
        {
            if (aSource.Channels() == 3) return aSource.Clone();

            Mat aBgr = new Mat();
            ColorConversionCodes aCode = aSource.Channels() switch
            {
                4 => ColorConversionCodes.BGRA2BGR,
                1 => ColorConversionCodes.GRAY2BGR,
                _ => ColorConversionCodes.BGRA2BGR
            };
            Cv2.CvtColor(aSource, aBgr, aCode);
            return aBgr;
        }

        public event EventHandler? Apply;
        public void OnApply() { Apply?.Invoke(this, EventArgs.Empty); }
    }
}
