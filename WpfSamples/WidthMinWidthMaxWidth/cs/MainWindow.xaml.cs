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

namespace Width_MinWidth_MaxWidth_CSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void changeWidth(object sender, SelectionChangedEventArgs args)
        {
            ListBoxItem li = ((sender as ListBox).SelectedItem as ListBoxItem);
            Double sz1 = Double.Parse(li.Content.ToString());
            rect1.Width = sz1;
            rect1.UpdateLayout();
            txt1.Text = "ActualWidth is set to " + rect1.ActualWidth;
            txt2.Text = "Width is set to " + rect1.Width;
            txt3.Text = "MinWidth is set to " + rect1.MinWidth;
            txt4.Text = "MaxWidth is set to " + rect1.MaxWidth;
        }
        private void changeMinWidth(object sender, SelectionChangedEventArgs args)
        {
            ListBoxItem li = ((sender as ListBox).SelectedItem as ListBoxItem);
            Double sz1 = Double.Parse(li.Content.ToString());
            rect1.MinWidth = sz1;
            rect1.UpdateLayout();
            txt1.Text = "ActualWidth is set to " + rect1.ActualWidth;
            txt2.Text = "Width is set to " + rect1.Width;
            txt3.Text = "MinWidth is set to " + rect1.MinWidth;
            txt4.Text = "MaxWidth is set to " + rect1.MaxWidth;
        }
        private void changeMaxWidth(object sender, SelectionChangedEventArgs args)
        {
            ListBoxItem li = ((sender as ListBox).SelectedItem as ListBoxItem);
            Double sz1 = Double.Parse(li.Content.ToString());
            rect1.MaxWidth = sz1;
            rect1.UpdateLayout();
            txt1.Text = "ActualWidth is set to " + rect1.ActualWidth;
            txt2.Text = "Width is set to " + rect1.Width;
            txt3.Text = "MinWidth is set to " + rect1.MinWidth;
            txt4.Text = "MaxWidth is set to " + rect1.MaxWidth;
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
