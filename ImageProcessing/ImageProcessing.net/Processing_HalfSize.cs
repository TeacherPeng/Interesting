using OpenCvSharp;

namespace ImageProcessing
{
    public class Processing_HalfSize : Processing
    {
        public override string Name { get { return "缩小"; } }

        protected override Mat ProcessImage(Mat aSource)
        {
            Mat aResult = new Mat();
            // 采用 INTER_AREA 插值，缩小图像时抗锯齿效果优于简单地隔点取样
            Cv2.Resize(aSource, aResult, new Size(), 0.5, 0.5, InterpolationFlags.Area);
            return aResult;
        }
    }
}
