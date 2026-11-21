using OpenCvSharp;

namespace ImageProcessing
{
    public class Processing_SmoothBilatral : Processing
    {
        public override string Name { get { return "SmoothBilatral"; } }

        protected override Mat ProcessImage(Mat aSource)
        {
            // 原公式: SmoothBilatral(51, 100, 100)
            Mat aResult = new Mat();
            Cv2.BilateralFilter(aSource, aResult, 51, 100, 100);
            return aResult;
        }
    }
}
