using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace ImageProcessing
{
    public class Processing_DarkChannel : Processing
    {
        public override string Name
        {
            get
            {
                return "DarkChannel";
            }
        }

        public override BitmapSource GetResultImage(BitmapImage aSourceImage)
        {
            Image<Bgr, byte> img = new Image<Bgr, byte>(Method_BitmapChange.BitmapImage2Bitmap(aSourceImage));
            Image<Gray, byte> img1 = Dehaze.getMedianDarkChannel(img,5);
            return Method_BitmapChange.Bitmap2BitmapImage(img1.ToBitmap());
        }

        protected override byte[] ProcessImage(byte[] aSourceRawData, ref int aPixelWidth, ref int aPixelHeight, int aBytesPerPixel, ref int aStride)
        {
            return aSourceRawData;
        }
    }
}
