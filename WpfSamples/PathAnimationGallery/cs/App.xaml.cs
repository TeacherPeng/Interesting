using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Media.Animation;
using System.IO;

namespace PathAnimationGallery
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
  
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
