using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageProcessing
{
    abstract class Processing
    {
        public abstract string Name { get; }
        protected abstract byte[] ProcessImage(byte[] aSourceRawData, ref int aPixelWidth, ref int aPixelHeight, int aBytesPerPixel, ref int aStride);

        public virtual BitmapSource GetResultImage(BitmapImage aSourceImage)
        {
            // 转换为标准Bgr32格式
            FormatConvertedBitmap aFormatedImage = new FormatConvertedBitmap(aSourceImage, PixelFormats.Bgr32, null, 0);

            // 提取图片数据
            int aStride = (aFormatedImage.Format.BitsPerPixel * aFormatedImage.PixelWidth + 7) / 8;
            byte[] aSourceRawData = new byte[aStride * aFormatedImage.PixelHeight];
            aFormatedImage.CopyPixels(aSourceRawData, aStride, 0);

            // 处理图片数据
            int aPixelWidth = aFormatedImage.PixelWidth;
            int aPixelHeight = aFormatedImage.PixelHeight;
            byte[] aResultRawData = ProcessImage(aSourceRawData, ref aPixelWidth, ref aPixelHeight, (aFormatedImage.Format.BitsPerPixel + 7) / 8, ref aStride);

            // 生成结果图片
            BitmapSource aImageFromRawData = BitmapImage.Create(
                aPixelWidth, aPixelHeight,
                aFormatedImage.DpiX, aFormatedImage.DpiY,
                aFormatedImage.Format, aFormatedImage.Palette,
                aResultRawData, aStride);

            return aImageFromRawData;
        }
    }
}
