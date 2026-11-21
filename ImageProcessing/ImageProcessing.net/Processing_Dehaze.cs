using OpenCvSharp;

namespace ImageProcessing
{
    public class Processing_Dehaze : Processing
    {
        public override string Name { get { return "Dehaze"; } }

        protected override Mat ProcessImage(Mat aSource)
        {
            return Dehaze.DehazeImage(aSource);
        }
    }
}
