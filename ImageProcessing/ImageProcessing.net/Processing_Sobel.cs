using OpenCvSharp;

namespace ImageProcessing
{
    public class Processing_Sobel : Processing
    {
        public override string Name { get { return "Sobel"; } }

        protected override Mat ProcessImage(Mat aSource)
        {
            // 原公式: img.Sobel(1, 0, 3) —— x 方向一阶，3x3 核
            using Mat aSobel = new Mat();
            Cv2.Sobel(aSource, aSobel, MatType.CV_16S, 1, 0, 3);

            Mat aResult = new Mat();
            // 转回 8U 并取绝对值，与原 Emgu 的显示效果一致
            Cv2.ConvertScaleAbs(aSobel, aResult);
            return aResult;
        }
    }
}
