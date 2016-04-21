using Emgu.CV;
using Emgu.CV.Structure;
using static Dehaze;

namespace ImageProcessing
{
    public class Processing_Dehaze : Processing_Emgu
    {
        public override string Name
        {
            get
            {
                return "Dehaze";
            }
        }
        protected override IImage ProcessImage_Emgu(Image<Bgr, byte> img)
        {
            return Dehaze_Image(img);
        }
    }
}
