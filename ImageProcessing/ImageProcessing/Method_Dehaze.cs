//#define DEBUG
#undef DEBUG

using System;

//System
using System.Drawing;

//EMGU
using Emgu.CV;
using Emgu.CV.Structure;

public static class Dehaze
{
    static double alpha = 0.05;    //alpha smoothing
    static int Airlightp;          //airlight value of previous frame
    static int Airlight;           //airlight value of current frame
    static int FrameCount = 0;     //frame number
    static int ad;                 //temp airlight value
    //=====================================================================================================
    //Image and video dehazing based on the paper: An Investigation in Dehazing Compressed Images and Video
    //Authors: Kristofor Gibson, D˜ung V˜o and Truong Nguyen
    //Original Code by Yuze Chen
    //https://github.com/Chrisawa/image-video-dehazing-OpenCV/blob/master/dehaze_mdcp.cpp
    //chenyuze1988@gmail.com
    //Translated to EMGUcv C# by Chris Johnson (MMU)
    //http://www.emgu.com/forum/viewtopic.php?f=7&t=4982
    //=====================================================================================================

    //median filtered dark channel
    public static Image<Gray, byte> getMedianDarkChannel(Image<Bgr, byte> src, int patch)
    {
        Image<Gray, byte> rgbmin = new Image<Gray, byte>(src.Width, src.Height); //Mat::zeros(src.rows, src.cols, CV_8UC1);

        for (int m = 0; m < src.Width; m++)
        {
            for (int n = 0; n < src.Height; n++)
            {
                //we don't need to take out src(x,y) intensity we can read it out and applyit the rgbmin directley
                rgbmin.Data[n, m, 0] = Math.Min(Math.Min(src.Data[n, m, 0], src.Data[n, m, 1]), src.Data[n, m, 2]);
            }
        }
        rgbmin = rgbmin.SmoothMedian(patch); //equivilant to medianBlur(rgbmin, rgbmin, patch);
#if DEBUG
        CvInvoke.cvShowImage("rgbmin", rgbmin);
#endif
        return rgbmin;
    }


    //estimate airlight by the brightest pixel in dark channel (proposed by He et al.)
    private static int estimateA(Image<Gray, byte> DC)
    {
        double[] minDC, maxDC;
        Point[] minDC_Loc, maxDC_Loc;
        DC.MinMax(out minDC, out maxDC, out minDC_Loc, out maxDC_Loc);
        return (int)maxDC[0];
    }


    //estimate transmission map
    private static Image<Gray, Byte> estimateTransmission(Image<Gray, Byte> DCP, int ac)
    {
        double w = 0.75;
        Image<Gray, Byte> transmission = new Image<Gray, byte>(DCP.Width, DCP.Height); //CV_8UC1 == gray image

        for (int m = 0; m < DCP.Width; m++)
        {
            for (int n = 0; n < DCP.Height; n++)
            {
                transmission.Data[n, m, 0] = (byte)((1 - w * (double)DCP.Data[n, m, 0] / ac) * 255);
            }
        }
#if DEBUG
        CvInvoke.cvShowImage("transmission", transmission);
#endif
        return transmission;
    }


    //dehazing foggy image
    private static Image<Bgr, Byte> getDehazed(Image<Bgr, Byte> source, Image<Gray, Byte> t, int al)
    {
        double tmin = 0.1;
        double tmax;

        //Scalar inttran;
        //Vec3b intsrc;
        Image<Bgr, Byte> dehazed = source.CopyBlank(); //CV_8UC3 == Bgr image 

        for (int i = 0; i < source.Width; i++)
        {
            for (int j = 0; j < source.Height; j++)
            {
                //	inttran = t.at<uchar>(i,j);
                //	intsrc = source.at<Vec3b>(i,j);

                tmax = ((double)t.Data[j, i, 0] / 255) < tmin ? tmin : ((double)t.Data[j, i, 0] / 255);
                for (int k = 0; k < 3; k++)
                {
                    dehazed.Data[j, i, 0] = (byte)(Math.Abs(((double)source.Data[j, i, 0] - al) / tmax + al) > 255 ? 255 : Math.Abs(((double)source.Data[j, i, 0] - al) / tmax + al)); //blue
                    dehazed.Data[j, i, 1] = (byte)(Math.Abs(((double)source.Data[j, i, 1] - al) / tmax + al) > 255 ? 255 : Math.Abs(((double)source.Data[j, i, 1] - al) / tmax + al)); //green
                    dehazed.Data[j, i, 2] = (byte)(Math.Abs(((double)source.Data[j, i, 2] - al) / tmax + al) > 255 ? 255 : Math.Abs(((double)source.Data[j, i, 2] - al) / tmax + al)); //red
                }
            }
        }
        return dehazed;
    }


    public static Image<Bgr, Byte> Dehaze_Image(Image<Bgr, Byte> src)
    {
        Image<Gray, Byte> darkChannel;  //灰度图
        Image<Gray, Byte> T;
        Image<Bgr, Byte> fogfree = src.CopyBlank();


        if (FrameCount == 0)
        {
            darkChannel = getMedianDarkChannel(src, 5);
            Airlight = estimateA(darkChannel);
            T = estimateTransmission(darkChannel, Airlight);
            ad = Airlight;
            fogfree = getDehazed(src, T, Airlight);
        }
        else
        {
            Airlightp = ad;
            darkChannel = getMedianDarkChannel(src, 5);
            Airlight = estimateA(darkChannel);
            T = estimateTransmission(darkChannel, Airlight);
            ad = (int)(alpha * (double)(Airlight) + (1 - alpha) * (double)(Airlightp));//airlight smoothing
            fogfree = getDehazed(src, T, ad);
        }
        FrameCount++;
        return fogfree;
    }
}