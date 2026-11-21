using OpenCvSharp;

namespace ImageProcessing
{
    /// <summary>
    /// 基于暗通道先验的单幅图像去雾（OpenCvSharp 5 重写版）。
    /// 原算法出自 "An Investigation in Dehazing Compressed Images and Video"。
    /// </summary>
    public static class Dehaze
    {
        private const double Alpha = 0.05;   // airlight 平滑系数
        private static int s_airlightPrev;
        private static int s_airlight;
        private static int s_frameCount;
        private static int s_ad;

        /// <summary>中值滤波后的暗通道：逐像素取 min(B,G,R) 再做中值滤波。</summary>
        public static Mat GetMedianDarkChannel(Mat aSrc, int aPatch)
        {
            Mat[] aChannels = Cv2.Split(aSrc);
            Mat aMin = new Mat();
            Mat aDark = new Mat();
            Mat aMedian = new Mat();
            try
            {
                Cv2.Min(aChannels[0], aChannels[1], aMin);
                Cv2.Min(aMin, aChannels[2], aDark);
                Cv2.MedianBlur(aDark, aMedian, aPatch);
                return aMedian;
            }
            finally
            {
                foreach (Mat aChannel in aChannels) aChannel.Dispose();
                aMin.Dispose();
                aDark.Dispose();
            }
        }

        /// <summary>估计大气光：取暗通道最大值（He 等人的方法）。</summary>
        private static int EstimateA(Mat aDarkChannel)
        {
            Cv2.MinMaxIdx(aDarkChannel, out _, out double aMax);
            return (int)aMax;
        }

        /// <summary>估计透射率图: t = (1 - 0.75 * dark / A) * 255。</summary>
        private static Mat EstimateTransmission(Mat aDarkChannel, int aAirlight)
        {
            int aSafe = Math.Max(aAirlight, 1);
            Mat aTransmission = new Mat();
            // 直接利用 ConvertTo 的 alpha/beta 做线性变换:
            // (1 - 0.75*dark/A)*255 = (-0.75*255/A)*dark + 255，并 saturate 到 [0,255]
            aDarkChannel.ConvertTo(aTransmission, MatType.CV_8UC1, -0.75 * 255.0 / aSafe, 255.0);
            return aTransmission;
        }

        /// <summary>去雾: tmax = max(t/255, 0.1); out = clamp(|(src - A)/tmax + A|, 0, 255)。</summary>
        private static Mat GetDehazed(Mat aSource, Mat aTransmission, int aAirlight)
        {
            const double aTMin = 0.1;

            using Mat aTNorm = new Mat();
            aTransmission.ConvertTo(aTNorm, MatType.CV_32FC1, 1.0 / 255.0);
            Cv2.Max(aTNorm, aTMin, aTNorm);

            Mat[] aT3 = { aTNorm, aTNorm, aTNorm };
            using Mat aTNorm3 = new Mat();
            Cv2.Merge(aT3, aTNorm3);

            using Mat aSourceF = new Mat();
            aSource.ConvertTo(aSourceF, MatType.CV_32FC3);

            // 逐运算: result = |(src - A)/tmax + A|
            using Mat aDiff = new Mat();
            Cv2.Subtract(aSourceF, Scalar.All(aAirlight), aDiff);
            using Mat aDivided = new Mat();
            Cv2.Divide(aDiff, aTNorm3, aDivided);
            using Mat aAdded = new Mat();
            Cv2.Add(aDivided, Scalar.All(aAirlight), aAdded);

            using Mat aZero = Mat.Zeros(aAdded.Rows, aAdded.Cols, aAdded.Type());
            using Mat aAbs = new Mat();
            Cv2.Absdiff(aAdded, aZero, aAbs);

            Mat aResult = new Mat();
            aAbs.ConvertTo(aResult, MatType.CV_8UC3); // saturate 到 [0,255]
            return aResult;
        }

        public static Mat DehazeImage(Mat aSrc)
        {
            using Mat aDark = GetMedianDarkChannel(aSrc, 5);
            int aCurrentA = EstimateA(aDark);
            int aUsedA;

            if (s_frameCount == 0)
            {
                s_airlight = aCurrentA;
                s_ad = aCurrentA;
                aUsedA = aCurrentA;
            }
            else
            {
                s_airlightPrev = s_ad;
                s_airlight = aCurrentA;
                s_ad = (int)(Alpha * s_airlight + (1 - Alpha) * s_airlightPrev);
                aUsedA = s_ad;
            }

            using Mat aT = EstimateTransmission(aDark, s_airlight);
            Mat aResult = GetDehazed(aSrc, aT, aUsedA);
            s_frameCount++;
            return aResult;
        }
    }
}
