﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace QuaternionView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Set globals
        Quaternion startQuaternion = new Quaternion(0,0,1,0);
        Quaternion endQuaternion = new Quaternion();
        TranslateTransform3D myTranslation = new TranslateTransform3D();
        RotateTransform3D baseRotateTransform3D = new RotateTransform3D();
        QuaternionRotation3D startRotation = new QuaternionRotation3D();
        QuaternionRotation3D endRotation = new QuaternionRotation3D();
        Transform3DGroup myprocTransformGroup = new Transform3DGroup();
        Rotation3DAnimation mydefaultAnimation = new Rotation3DAnimation();       

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateWXYZSettings(object sender, EventArgs e)
        {
            //Clear prior values
            myprocTransformGroup.Children.Clear();
            XAxisValue.Clear();
            YAxisValue.Clear();
            ZAxisValue.Clear();
            AngleValue.Clear();

            //Read new settings
            Double setW = System.Convert.ToDouble(WValue.Text);
            Double setX = System.Convert.ToDouble(XValue.Text);
            Double setY = System.Convert.ToDouble(YValue.Text);
            Double setZ = System.Convert.ToDouble(ZValue.Text);

            endQuaternion = new Quaternion(setX, setY, setZ, setW);
            endRotation.Quaternion = endQuaternion;

            //Update axis and angle textboxes
            XAxisValue.Text = endQuaternion.Axis.X.ToString();
            YAxisValue.Text = endQuaternion.Axis.Y.ToString();
            ZAxisValue.Text = endQuaternion.Axis.Z.ToString();
            AngleValue.Text = endQuaternion.Angle.ToString();

            startAnimation();
        }

        private void UpdateParseXYZWSettings(object sender, EventArgs e)
        {
            //Clear prior values
            myprocTransformGroup.Children.Clear();
            XAxisValue.Clear();
            YAxisValue.Clear();
            ZAxisValue.Clear();
            AngleValue.Clear();

            try
            {
                endQuaternion = Quaternion.Parse(ParseValue.Text);
            }
            catch
            {
                ParseValue.Text = "Must input (X, Y, Z, W)";
            }

            endRotation.Quaternion = endQuaternion;

            //Update axis and angle textboxes
            XAxisValue.Text = endQuaternion.Axis.X.ToString();
            YAxisValue.Text = endQuaternion.Axis.Y.ToString();
            ZAxisValue.Text = endQuaternion.Axis.Z.ToString();
            AngleValue.Text = endQuaternion.Angle.ToString();

            startAnimation();
        }

        private void UpdateAxisAngleSettings(object sender, EventArgs e)
        {
            //Clear prior values
            myprocTransformGroup.Children.Clear();
            WValue.Clear();
            XValue.Clear();
            YValue.Clear();
            ZValue.Clear();

            //Read new settings
            Double angle = System.Convert.ToDouble(AngleValue.Text);
            try
            {
                Double xaxis = System.Convert.ToDouble(XAxisValue.Text);
                Double yaxis = System.Convert.ToDouble(YAxisValue.Text);
                Double zaxis = System.Convert.ToDouble(ZAxisValue.Text);

                endQuaternion = new Quaternion(new Vector3D(xaxis, yaxis, zaxis), angle);
            }
            catch
            {
                XAxisValue.Text = "Axis must be nonzero Vector3D";
                YAxisValue.Text = "Axis must be nonzero Vector3D";
                ZAxisValue.Text = "Axis must be nonzero Vector3D";
            }

            endRotation.Quaternion = endQuaternion;

            //Update quaternion display
            WValue.Text = endQuaternion.W.ToString();
            XValue.Text = endQuaternion.X.ToString();
            YValue.Text = endQuaternion.Y.ToString();
            ZValue.Text = endQuaternion.Z.ToString();

            //build in some if clauses to determine the animation method to call
            startAnimation();
        }

        public void startAnimation()
        {
            mydefaultAnimation.From = startRotation;
            mydefaultAnimation.To = endRotation;
            mydefaultAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(3000));
            mydefaultAnimation.FillBehavior = FillBehavior.HoldEnd;

            baseRotateTransform3D.BeginAnimation(RotateTransform3D.RotationProperty, mydefaultAnimation);
            myprocTransformGroup.Children.Add(baseRotateTransform3D);
            topModelVisual3D.Transform = myprocTransformGroup;

            //Update text boxes
            //TransformMatrix.Text = myprocTransformGroup.Value.ToString();
            //TransformMatrix.Text = baseRotateTransform3D.Value.ToString();

            AxisAngleString.Text = endQuaternion.ToString();
            //resulting string is (x,y,z,w)

        }

    }
}