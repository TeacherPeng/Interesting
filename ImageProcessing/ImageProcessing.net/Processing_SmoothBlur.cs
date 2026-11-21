using System.Windows.Controls;
using OpenCvSharp;

namespace ImageProcessing
{
    public class Processing_SmoothBlur : Processing
    {
        public Processing_SmoothBlur()
        {
            _Control = new Ui_Slider(this);
            Level = 50;
        }
        public override string Name { get { return "SmoothBlur"; } }
        public override UserControl Control { get { return _Control; } }
        private Ui_Slider _Control;
        public double Level { get; set; }

        protected override Mat ProcessImage(Mat aSource)
        {
            // 原公式: SmoothBlur(temp, temp)，temp = 5 + (int)Level/5
            int aKernel = 5 + (int)Level / 5;
            if (aKernel < 1) aKernel = 1;

            Mat aResult = new Mat();
            Cv2.Blur(aSource, aResult, new Size(aKernel, aKernel));
            return aResult;
        }
    }
}
