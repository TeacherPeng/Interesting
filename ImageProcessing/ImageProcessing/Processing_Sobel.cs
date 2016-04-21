using Emgu.CV.Structure;
using System.Windows.Media.Imaging;
using Emgu.CV;
using System.Windows.Controls;

namespace ImageProcessing
{
    class Processing_Sobel : Processing
    {
        public override string Name { get { return "Sobel"; } } //函数

        public override BitmapSource GetResultImage(BitmapImage aSourceImage)
        {
            Image<Bgr, byte> img = new Image<Bgr, byte>(Method_BitmapChange.BitmapImage2Bitmap(aSourceImage));
            Image<Bgr, float> img1 = img.Sobel(1, 0, 3);
            return Method_BitmapChange.Bitmap2BitmapImage(img1.ToBitmap());
        }

        protected override byte[] ProcessImage(byte[] aSourceRawData, ref int aPixelWidth, ref int aPixelHeight, int aBytesPerPixel, ref int aStride)
        {
            return aSourceRawData;
        }

    }
}



