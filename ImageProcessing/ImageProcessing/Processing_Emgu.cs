using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows.Media.Imaging;
using static ImageProcessing.Method_BitmapChange;

namespace ImageProcessing
{
    public abstract class Processing_Emgu : Processing
    {
        protected override byte[] ProcessImage(byte[] aSourceRawData, ref int aPixelWidth, ref int aPixelHeight, int aBytesPerPixel, ref int aStride)
        {
            return null;
        }
        protected abstract IImage ProcessImage_Emgu(Image<Bgr, byte> img);
        public override BitmapSource GetResultImage(BitmapImage aSourceImage)
        {
            Image<Bgr, byte> img = new Image<Bgr, byte>(BitmapImage2Bitmap(aSourceImage));
            IImage img1 = ProcessImage_Emgu(img);
            return Bitmap2BitmapImage(img1.Bitmap);
        }
    }
}
