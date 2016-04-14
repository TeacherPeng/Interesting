using Emgu.CV.Structure;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using Emgu.CV;
using System.Drawing.Imaging;

namespace ImageProcessing
{
    class Processing_Emgu : Processing
    {
        public override string Name
        {
            get
            {
                return "Emgu";
            }
        }

        public override BitmapSource GetResultImage(BitmapImage aSourceImage)
        {
            Image<Bgr, byte> img = new Image<Bgr, byte>(BitmapImage2Bitmap(aSourceImage));
            Image<Bgr, byte> img1=img.SmoothGaussian(31);
            return Bitmap2BitmapImage(img1.ToBitmap());
        }

        protected override byte[] ProcessImage(byte[] aSourceRawData, ref int aPixelWidth, ref int aPixelHeight, int aBytesPerPixel, ref int aStride)
        {
            return aSourceRawData;
        }

        //类型转换函数 ！！
        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }


        private BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}



