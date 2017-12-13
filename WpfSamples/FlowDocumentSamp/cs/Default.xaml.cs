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

namespace FlowDocument_Viewer
{
    /// <summary>
    /// Interaction logic for Default.xaml
    /// </summary>
    public partial class Page1 : Page
    {
               Application app;

        public void menuExit(object sender, RoutedEventArgs args)
            {
                app = (Application)System.Windows.Application.Current;
                app.Shutdown();
            }

        public void menuAbout(object sender, RoutedEventArgs args)
        {
            frame2.Source = new Uri("about.xaml", UriKind.Relative);
        }
    }

}