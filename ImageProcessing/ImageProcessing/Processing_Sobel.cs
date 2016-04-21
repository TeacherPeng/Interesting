using Emgu.CV.Structure;
using System.Windows.Media.Imaging;
using Emgu.CV;
using static ImageProcessing.Method_BitmapChange;
using System;

namespace ImageProcessing
{
    class Processing_Sobel : Processing_Emgu
    {
        public override string Name { get { return "Sobel"; } } //函数
        protected override IImage ProcessImage_Emgu(Image<Bgr, byte> img)
        {
            return img.Sobel(1, 0, 3);
        }
    }
}



