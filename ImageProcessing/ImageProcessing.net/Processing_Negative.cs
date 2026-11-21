using OpenCvSharp;

namespace ImageProcessing
{
    public class Processing_Negative : Processing
    {
        public override string Name { get { return "Negative"; } }

        protected override Mat ProcessImage(Mat aSource)
        {
            Mat aResult = new Mat();
            Cv2.BitwiseNot(aSource, aResult);
            return aResult;
        }
    }
}
