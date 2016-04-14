using System;
using System.Windows.Controls;

namespace ImageProcessing
{
    public class Processing_IncBrightness : Processing
    {
        public Processing_IncBrightness()
        {
            _Control = new Processing_IncBrightness_Ui(this);
            Level = 15;
        }
        public override string Name { get { return "提高亮度"; } }
        public override UserControl Control { get { return _Control; } }
        private Processing_IncBrightness_Ui _Control;

        public double Level { get; set; }
        
        protected override byte[] ProcessImage(byte[] aSourceRawData, ref int aPixelWidth, ref int aPixelHeight, int aBytesPerPixel, ref int aStride)
        {
            for (int i = 0; i < aSourceRawData.Length; i++) { int abyte = (int)(aSourceRawData[i] * Level / 10); if (abyte > 255) abyte = 255; aSourceRawData[i] = (byte)abyte; }
            return aSourceRawData;
        }
    }
}
