using OpenCvSharp;

namespace ImageProcessing
{
    public class Processing_DarkChannel : Processing
    {
        public override string Name { get { return "DarkChannel"; } }

        protected override Mat ProcessImage(Mat aSource)
        {
            return Dehaze.GetMedianDarkChannel(aSource, 5);
        }
    }
}
