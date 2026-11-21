using System.Windows.Controls;
using OpenCvSharp;

namespace ImageProcessing
{
    public class Processing_IncBrightness : Processing
    {
        public Processing_IncBrightness()
        {
            _Control = new Ui_Slider(this);
            Level = 50;
        }
        public override string Name { get { return "Brightness"; } }
        public override UserControl Control { get { return _Control; } }
        private Ui_Slider _Control;
        public double Level { get; set; }

        protected override Mat ProcessImage(Mat aSource)
        {
            // 原公式: byte * (1 + (Level-50)/50) = byte * (Level/50)，结果 clamp 到 [0,255]
            double aFactor = Level / 50.0;
            Mat aResult = new Mat();
            aSource.ConvertTo(aResult, aSource.Type(), aFactor, 0);
            return aResult;
        }
    }
}
