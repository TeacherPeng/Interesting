// Copyright ?Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media;

namespace BrushesIntroduction
{

    /// <summary>
    /// Implements the show/hide gradient stops functionlity for
    /// GradientBrushesExample.xaml.
    /// </summary>
    public partial class GradientBrushesExample : Page
    {
        
        
       public GradientBrushesExample()
       {
            InitializeComponent();
       
       }
       
       private void pageLoaded(object sender, RoutedEventArgs args)
       {
            showHideGradientStopsCheckBox.Checked += new RoutedEventHandler(showGradientStops);
            showHideGradientStopsCheckBox.Unchecked += new RoutedEventHandler(hideGradientStops);
       
       }

       private void showGradientStops(object sender, RoutedEventArgs args)
       {
               gradientLine1.Opacity = 1.0;
               gradientLine2.Opacity = 1.0;
               gradientLine3.Opacity = 1.0;
               gradientLine4.Opacity = 1.0;
               gradientLine5.Opacity = 1.0;
               gradientPath1.Opacity = 1.0;
               gradientPath2.Opacity = 1.0;
               gradientPath3.Opacity = 1.0;
               gradientPath4.Opacity = 1.0;
               gradientPath5.Opacity = 1.0;       
       }
       
       private void hideGradientStops(object sender, RoutedEventArgs args)
       {
      
               gradientLine1.Opacity = 0.0;
               gradientLine2.Opacity = 0.0;
               gradientLine3.Opacity = 0.0;
               gradientLine4.Opacity = 0.0;
               gradientLine5.Opacity = 0.0;
               gradientPath1.Opacity = 0.0;
               gradientPath2.Opacity = 0.0;
               gradientPath3.Opacity = 0.0;
               gradientPath4.Opacity = 0.0;
               gradientPath5.Opacity = 0.0;
       }       
            
    }
   
}