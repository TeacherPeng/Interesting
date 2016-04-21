using Emgu.CV.Structure;
using System.Windows.Media.Imaging;
using Emgu.CV;
using static ImageProcessing.Method_BitmapChange;
using System;

namespace ImageProcessing
{
    class Processing_SmoothBilatral : Processing_Emgu
    {
        public override string Name { get { return "SmoothBilatral"; } } //函数名
        protected override IImage ProcessImage_Emgu(Image<Bgr, byte> img)
        {
            return img.SmoothBilatral(51, 100, 100);
        }
    }
}



