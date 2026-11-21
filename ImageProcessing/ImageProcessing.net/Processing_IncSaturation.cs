using System.Windows.Controls;
using OpenCvSharp;

namespace ImageProcessing
{
    public class Processing_IncSaturation : Processing
    {
        public Processing_IncSaturation()
        {
            _Control = new Ui_Slider(this);
            Level = 50;
        }
        public override string Name { get { return "Saturation"; } }
        public override UserControl Control { get { return _Control; } }
        private Ui_Slider _Control;
        public double Level { get; set; }

        protected override Mat ProcessImage(Mat aSource)
        {
            // 原公式（逐像素、BGR）: mid = (b+g+r)/3; ch' = mid + (ch-mid)*(1+(Level-50)/50)
            double aFactor = Level / 50.0;
            using Mat aFloat = new Mat();
            aSource.ConvertTo(aFloat, MatType.CV_32FC3);

            Mat[] aChannels = Cv2.Split(aFloat);
            Mat aB = aChannels[0], aG = aChannels[1], aR = aChannels[2];
            try
            {
                using Mat aMid = (aB + aG + aR) * (1.0 / 3.0);
                using Mat aNB = aMid + (aB - aMid) * aFactor;
                using Mat aNG = aMid + (aG - aMid) * aFactor;
                using Mat aNR = aMid + (aR - aMid) * aFactor;

                using Mat aMerged = new Mat();
                Cv2.Merge(new[] { aNB, aNG, aNR }, aMerged);

                Mat aResult = new Mat();
                // ConvertTo 做 saturate，结果截断到 [0,255]，与原逻辑一致
                aMerged.ConvertTo(aResult, MatType.CV_8UC3);
                return aResult;
            }
            finally
            {
                aB.Dispose(); aG.Dispose(); aR.Dispose();
            }
        }
    }
}
