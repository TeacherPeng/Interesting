using OpenCvSharp;

namespace ImageProcessing
{
    public class Processing_Border : Processing
    {
        public override string Name { get { return "加5像素黑框"; } }

        protected override Mat ProcessImage(Mat aSource)
        {
            const int aBorder = 5;
            Mat aResult = new Mat();
            Cv2.CopyMakeBorder(aSource, aResult, aBorder, aBorder, aBorder, aBorder,
                BorderTypes.Constant, Scalar.All(0));
            return aResult;
        }
    }
}
