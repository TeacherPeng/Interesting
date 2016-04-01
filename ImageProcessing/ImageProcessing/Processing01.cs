using System;

namespace ImageProcessing
{
    class Processing01 : Processing
    {
        public override string Name
        {
            get
            {
                return "加5像素黑框";
            }
        }

        protected override byte[] ProcessImage(byte[] aSourceRawData, int aPixelWidth, int aPixelHeight, int aBytesPerPixel, int aStride)
        {
            // 上边框
            int aIndex = 0;
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < aStride; i++) aSourceRawData[aIndex + i] = 0;
                aIndex += aStride;
            }
            // 下边框
            aIndex = (aPixelHeight - 5) * aStride;
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < aStride; i++) aSourceRawData[aIndex + i] = 0;
                aIndex += aStride;
            }
            // 左边框
            aIndex = 0;
            for (int i = 0; i < aPixelHeight; i++)
            {
                for (int j = 0; j < 5 * aBytesPerPixel; j++) aSourceRawData[aIndex + j] = 0;
                aIndex += aStride;
            }
            // 右边框
            aIndex = (aPixelWidth - 5) * aBytesPerPixel;
            for (int i = 0; i < aPixelHeight; i++)
            {
                for (int j = 0; j < 5 * aBytesPerPixel; j++) aSourceRawData[aIndex + j] = 0;
                aIndex += aStride;
            }
            return aSourceRawData;
        }
    }
}
