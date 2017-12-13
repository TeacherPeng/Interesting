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

namespace DrawingWithShapeElements
{
    /// <summary>
    /// Interaction logic for SampleViewer.xaml
    /// </summary>
    public partial class SampleViewer : Window
    {
    
    	private Page[] navigationArray;

		public SampleViewer()
		{
		
			navigationArray = new Page[11];
			navigationArray[0] = new ShapeTypes();
			navigationArray[1] = new LineExample();
			navigationArray[2] = new RectangleExample();
			navigationArray[3] = new EllipseExample();
			navigationArray[4] = new PolylineExample();
			navigationArray[5] = new PolygonExample();
			navigationArray[6] = new PathExample();
			navigationArray[7] = new FillRuleExample();
			navigationArray[8] = new LineCapsAndJoinsExample();
			navigationArray[9] = new MiterLimitExample();
           	navigationArray[10] = new StretchExample();
			InitializeComponent();
			
		}
		
		private void sampleSelected(object sender, RoutedEventArgs args)
		{
			//Page selectedSample = navigationArray[sampleSelector.SelectedIndex];
			//mainFrame.Navigate(selectedSample);
		
		}


    }
}
