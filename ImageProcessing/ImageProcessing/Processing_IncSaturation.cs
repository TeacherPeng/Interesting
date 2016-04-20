using System;
using System.Windows.Controls;

namespace ImageProcessing
{
    public class Processing_IncSaturation : Processing
    {
        public Processing_IncSaturation()
        {
            _Control = new Ui_Slider(this);
            Level = 50;
        }
        public override string Name { get { return "Saturation"; } }
        public override UserControl Control { get { return _Control; } }
        private Ui_Slider _Control;
        public double Level { get; set; }
        
        protected override byte[] ProcessImage(byte[] aSourceRawData, ref int aPixelWidth, ref int aPixelHeight, int aBytesPerPixel, ref int aStride)
        {
            int t = 0, abyte;
            double r,g,b;
            r = g = b = 0;
            for (int i = 0; i < aSourceRawData.Length; i+=4)
            {
                b = aSourceRawData[i-3];
                g = aSourceRawData[i-2];
                r = aSourceRawData[i-1];
                double mid = (r + g + b) / 3;
                abyte = (int)(mid+ (b-mid)* (1+(Level - 50) / 50));
                if (abyte > 255) abyte = 255; if (abyte < 0) abyte = 0; aSourceRawData[i-3] = (byte)abyte;
                abyte = (int)(mid + (g - mid) * (1 + (Level - 50) / 50));
                if (abyte > 255) abyte = 255; if (abyte < 0) abyte = 0; aSourceRawData[i-2] = (byte)abyte;
                abyte = (int)(mid + (r - mid) * (1 + (Level - 50) / 50));
                if (abyte > 255) abyte = 255; if (abyte < 0) abyte = 0; aSourceRawData[i-1] = (byte)abyte;
            }
            return aSourceRawData;
        }
    }
}
