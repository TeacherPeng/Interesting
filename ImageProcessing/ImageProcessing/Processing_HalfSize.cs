using System;

namespace ImageProcessing
{
    class Processing_HalfSize : Processing
    {
        public override string Name { get { return "缩小"; } }

        protected override byte[] ProcessImage(byte[] aSourceRawData, ref int aPixelWidth, ref int aPixelHeight, int aBytesPerPixel, ref int aStride)
        {
            byte[] aResultRasData = new byte[aBytesPerPixel * (aPixelWidth / 2) * (aPixelHeight / 2)];
            int rowIndex = 0, resultIndex = 0;
            for (int row = 0; row < aPixelHeight / 2; row++)
            {
                int colIndex = rowIndex;
                for (int col = 0; col < aPixelWidth / 2; col++)
                {
                    for (int i = 0; i < aBytesPerPixel; i++) aResultRasData[resultIndex++] = aSourceRawData[colIndex + i];
                    colIndex += aBytesPerPixel + aBytesPerPixel;
                }
                rowIndex += aStride + aStride;
            }
            aPixelWidth /= 2;
            aPixelHeight /= 2;
            aStride = aBytesPerPixel * aPixelWidth;
            return aResultRasData;
        }
    }
}
