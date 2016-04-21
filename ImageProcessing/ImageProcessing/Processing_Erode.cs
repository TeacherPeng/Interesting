using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Controls;

namespace ImageProcessing
{
    class Processing_Erode : Processing_Emgu
    {
        public Processing_Erode()
        {
            _Control = new Ui_Slider(this); //加入滑动条
            Level = 50;
        }

        public override string Name { get { return "Erode"; } } //函数名
        public override UserControl Control { get { return _Control; } }
        private Ui_Slider _Control;
        public double Level { get; set; }
        protected override IImage ProcessImage_Emgu(Image<Bgr, byte> img)
        {
            return img.Erode((int)Level / 20);
        }
    }
}



