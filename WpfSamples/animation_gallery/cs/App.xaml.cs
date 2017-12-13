using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;


namespace Microsoft.Samples.Animation.AnimationGallery
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        
        }
    
        protected override void OnStartup(StartupEventArgs e)
        {
            Window myWindow = new Window();
            myWindow.Content = new GridSampleViewer();
            myWindow.Show();
            base.OnStartup(e);
        }

    }
}
