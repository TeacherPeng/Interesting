using System.Windows.Controls;
using OpenCvSharp;

namespace ImageProcessing
{
    public class Processing_IncConstrast : Processing
    {
        public Processing_IncConstrast()
        {
            _Control = new Ui_Slider(this);
            Level = 50;
        }
        public override string Name { get { return "Constrast"; } }
        public override UserControl Control { get { return _Control; } }
        private Ui_Slider _Control;
        public double Level { get; set; }

        protected override Mat ProcessImage(Mat aSource)
        {
            // 原公式: (byte - 127.5) * k + 127.5，等价于 byte*k + 127.5*(1-k)
            double c = (Level - 50) / 50;
            double k = Math.Tan((45 + 44 * c) / 180 * Math.PI);

            Mat aResult = new Mat();
            // ConvertTo 会做 saturate（截断到 [0,255]），与原逻辑一致
            aSource.ConvertTo(aResult, aSource.Type(), k, 127.5 * (1 - k));
            return aResult;
        }
    }
}
