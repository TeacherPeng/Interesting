using System;

namespace ImageProcessing
{
    public class Processing_Negative : Processing
    {
        public override string Name
        {
            get
            {
                return "生成负片";
            }
        }

        protected override byte[] ProcessImage(byte[] aSourceRawData, ref int aPixelWidth, ref int aPixelHeight, int aBytesPerPixel, ref int aStride)
        {
            int i = 0;
            for (int row = 0; row < aPixelHeight; row++)
            {
                for (int j = 0; j < aStride; j += 4)
                {
                    aSourceRawData[i + j] = (byte)(255 - aSourceRawData[i + j]);
                    aSourceRawData[i + j + 1] = (byte)(255 - aSourceRawData[i + j + 1]);
                    aSourceRawData[i + j + 2] = (byte)(255 - aSourceRawData[i + j + 2]);
                }
                i += aStride;
            }
            return aSourceRawData;
        }
    }
}
