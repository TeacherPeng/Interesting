using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Controls;

namespace ImageProcessing
{
    class Processing_SmoothGaussian : Processing_Emgu
    {
        public Processing_SmoothGaussian()
        {
            _Control = new Ui_Slider(this); //加入滑动条
            Level = 50;
        }

        public override string Name { get { return "SmoothGaussian"; } } //函数名
        public override UserControl Control { get { return _Control; } }
        private Ui_Slider _Control;

        public double Level { get; set; }

        protected override IImage ProcessImage_Emgu(Image<Bgr, byte> img)
        {
            return img.SmoothGaussian((int)(Level / 5) * 2 + 1);
        }
    }
}



