﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Controls;
using System.IO;

namespace Animation_TimingBehaviors
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void myAppStartup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            NavigationWindow myNavigationWindow = new NavigationWindow();
            myNavigationWindow.Navigate(new SampleViewer());
            myNavigationWindow.Show();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            
            try {
                StreamWriter wr = new StreamWriter("error.txt");
                wr.Write(args.ExceptionObject.ToString());
                wr.Close();
            
            }catch
            {
            
            }
            
            MessageBox.Show("Unhandled exception: " + args.ExceptionObject.ToString());
        }     
    
    }

}
