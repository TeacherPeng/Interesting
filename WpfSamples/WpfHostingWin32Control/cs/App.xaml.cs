using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Runtime.InteropServices;

namespace WpfHostingWin32Control
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("comctl32.dll", EntryPoint = "InitCommonControls", CharSet = CharSet.Auto)]
        public static extern void InitCommonControls();
        private void ApplicationStartup(object sender, StartupEventArgs args)
        {
            InitCommonControls();
            HostWindow host = new HostWindow();
            host.InitializeComponent();
        }
    }
}
