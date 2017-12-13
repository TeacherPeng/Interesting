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

namespace Height_MinHeight_MaxHeight_CSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void changeHeight(object sender, SelectionChangedEventArgs args)
        {
            ListBoxItem li = ((sender as ListBox).SelectedItem as ListBoxItem);
            Double sz1 = Double.Parse(li.Content.ToString());
            rect1.Height = sz1;
            rect1.UpdateLayout();
            txt1.Text = "ActualHeight is set to " + rect1.ActualHeight;
            txt2.Text = "Height is set to " + rect1.Height;
            txt3.Text = "MinHeight is set to " + rect1.MinHeight;
            txt4.Text = "MaxHeight is set to " + rect1.MaxHeight;
        }
        private void changeMinHeight(object sender, SelectionChangedEventArgs args)
        {
            ListBoxItem li = ((sender as ListBox).SelectedItem as ListBoxItem);
            Double sz1 = Double.Parse(li.Content.ToString());
            rect1.MinHeight = sz1;
            rect1.UpdateLayout();
            txt1.Text = "ActualHeight is set to " + rect1.ActualHeight;
            txt2.Text = "Height is set to " + rect1.Height;
            txt3.Text = "MinHeight is set to " + rect1.MinHeight;
            txt4.Text = "MaxHeight is set to " + rect1.MaxHeight;
        }
        private void changeMaxHeight(object sender, SelectionChangedEventArgs args)
        {
            ListBoxItem li = ((sender as ListBox).SelectedItem as ListBoxItem);
            Double sz1 = Double.Parse(li.Content.ToString());
            rect1.MaxHeight = sz1;
            rect1.UpdateLayout();
            txt1.Text = "ActualHeight is set to " + rect1.ActualHeight;
            txt2.Text = "Height is set to " + rect1.Height;
            txt3.Text = "MinHeight is set to " + rect1.MinHeight;
            txt4.Text = "MaxHeight is set to " + rect1.MaxHeight;
        }

        private void clipRect(object sender, RoutedEventArgs args)
        {
            myCanvas.ClipToBounds = true;
            txt5.Text = "Canvas.ClipToBounds is set to " + myCanvas.ClipToBounds;
        }
        private void unclipRect(object sender, RoutedEventArgs args)
        {
            myCanvas.ClipToBounds = false;
            txt5.Text = "Canvas.ClipToBounds is set to " + myCanvas.ClipToBounds;
        }
    }
}