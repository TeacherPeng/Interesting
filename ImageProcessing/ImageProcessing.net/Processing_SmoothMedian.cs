using System.Windows.Controls;
using OpenCvSharp;

namespace ImageProcessing
{
    public class Processing_SmoothMedian : Processing
    {
        public Processing_SmoothMedian()
        {
            _Control = new Ui_Slider(this);
            Level = 50;
        }
        public override string Name { get { return "SmoothMedian"; } }
        public override UserControl Control { get { return _Control; } }
        private Ui_Slider _Control;
        public double Level { get; set; }

        protected override Mat ProcessImage(Mat aSource)
        {
            // 原公式: SmoothMedian((int)(Level/10)*2+1)，核大小需为奇数且 >= 3
            int aKernel = (int)(Level / 10) * 2 + 1;
            if (aKernel < 3) aKernel = 3;

            Mat aResult = new Mat();
            Cv2.MedianBlur(aSource, aResult, aKernel);
            return aResult;
        }
    }
}
