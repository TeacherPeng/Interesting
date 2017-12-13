﻿// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

using System;
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
using System.Diagnostics;

namespace CompositionTargetSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Point _pt;
        private Stopwatch _stopwatch = new Stopwatch();
        private double _frameCounter = 0;

        public MainWindow()
        {
            InitializeComponent();
            // Add an event handler to update canvas background color just before it is rendered.
            CompositionTarget.Rendering += UpdateColor;
        }

        // Called just before frame is rendered to allow custom drawing.
        protected void UpdateColor(object sender, EventArgs e)
        {
            if (_frameCounter++ == 0)
            {
                // Starting timing.
                _stopwatch.Start();
            }

            // Determine frame rate in fps (frames per second).
            long frameRate = (long)(_frameCounter / this._stopwatch.Elapsed.TotalSeconds);
            if (frameRate > 0)
            {
                // Update elapsed time, number of frames, and frame rate.
                myStopwatchLabel.Content = _stopwatch.Elapsed.ToString();
                myFrameCounterLabel.Content = _frameCounter.ToString();
                myFrameRateLabel.Content = frameRate.ToString();
            }

            // Update the background of the canvas by converting MouseMove info to RGB info.
            byte redColor = (byte)(_pt.X / 3.0);
            byte blueColor = (byte)(_pt.Y / 2.0);
            myCanvas.Background = new SolidColorBrush(Color.FromRgb(redColor, 0x0, blueColor));
        }

        public void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            // Retreive the coordinates of the mouse button event.
            _pt = e.GetPosition((UIElement)sender);
        }
    }
}