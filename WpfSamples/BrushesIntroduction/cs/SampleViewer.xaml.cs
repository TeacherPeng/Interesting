// Copyright © Microsoft Corporation.  All Rights Reserved.
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
using System.Windows.Media.Animation;
using System.Reflection;
using System.Xml;

namespace BrushesIntroduction
{
    /// <summary>
    /// Interaction logic for SampleViewer.xaml
    /// </summary>
    public partial class SampleViewer: Page
    {
        public SampleViewer()
        {
            InitializeComponent();
        }

        private void transitionAnimationStateChanged(object sender, EventArgs args)
        {
            AnimationClock transitionAnimationClock = (AnimationClock)sender;


            if (transitionAnimationClock.CurrentState == ClockState.Filling)
            {
                fadeEnded();
            }
        }


        private void myFrameNavigated(object sender, NavigationEventArgs args)
        {
            DoubleAnimation myFadeInAnimation = (DoubleAnimation)this.Resources["MyFadeInAnimationResource"];
            myFrame.BeginAnimation(Frame.OpacityProperty, myFadeInAnimation, HandoffBehavior.SnapshotAndReplace);
        }

        private void fadeEnded()
        {

            XmlElement el = (XmlElement)myPageList.SelectedItem;
            XmlAttribute att = el.Attributes["Uri"];
            if (att != null)
            {
                myFrame.Navigate(new Uri(att.Value, UriKind.Relative));
            }
            else
            {
                myFrame.Content = null;
            }
        }

        public static RoutedUICommand ExitCommand = 
            new RoutedUICommand("Exit", "Exit", typeof(SampleViewer));

        private void executeExitCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }

}