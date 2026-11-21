using System.Windows.Controls;
using OpenCvSharp;

namespace ImageProcessing
{
    public class Processing_Erode : Processing
    {
        public Processing_Erode()
        {
            _Control = new Ui_Slider(this);
            Level = 50;
        }
        public override string Name { get { return "Erode"; } }
        public override UserControl Control { get { return _Control; } }
        private Ui_Slider _Control;
        public double Level { get; set; }

        protected override Mat ProcessImage(Mat aSource)
        {
            // 原公式: img.Erode((int)Level/20)，迭代次数最少为 1
            int aIterations = Math.Max(1, (int)Level / 20);
            Mat aResult = new Mat();
            Cv2.Erode(aSource, aResult, default, null, aIterations);
            return aResult;
        }
    }
}
