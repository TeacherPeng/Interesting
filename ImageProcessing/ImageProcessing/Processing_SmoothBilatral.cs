using Emgu.CV.Structure;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using Emgu.CV;
using System.Drawing.Imaging;
using System.Windows.Controls;

namespace ImageProcessing
{
    class Processing_SmoothBilatral : Processing
    {
        public override string Name { get { return "SmoothBilatral"; } } //函数名

        public override BitmapSource GetResultImage(BitmapImage aSourceImage)
        {
            Image<Bgr, byte> img = new Image<Bgr, byte>(Method_BitmapChange.BitmapImage2Bitmap(aSourceImage));
            Image<Bgr, byte> img1 = img.SmoothBilatral(51,100,100);
            return Method_BitmapChange.Bitmap2BitmapImage(img1.ToBitmap());
        }

        protected override byte[] ProcessImage(byte[] aSourceRawData, ref int aPixelWidth, ref int aPixelHeight, int aBytesPerPixel, ref int aStride)
        {
            return aSourceRawData;
        }

    }
}



