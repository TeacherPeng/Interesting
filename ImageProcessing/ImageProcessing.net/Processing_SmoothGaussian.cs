using System.Windows.Controls;
using OpenCvSharp;

namespace ImageProcessing
{
    public class Processing_SmoothGaussian : Processing
    {
        public Processing_SmoothGaussian()
        {
            _Control = new Ui_Slider(this);
            Level = 50;
        }
        public override string Name { get { return "SmoothGaussian"; } }
        public override UserControl Control { get { return _Control; } }
        private Ui_Slider _Control;
        public double Level { get; set; }

        protected override Mat ProcessImage(Mat aSource)
        {
            // 原公式: SmoothGaussian((int)(Level/5)*2+1)，核大小需为正奇数
            int aKernel = (int)(Level / 5) * 2 + 1;
            if (aKernel < 1) aKernel = 1;

            Mat aResult = new Mat();
            Cv2.GaussianBlur(aSource, aResult, new Size(aKernel, aKernel), 0);
            return aResult;
        }
    }
}
