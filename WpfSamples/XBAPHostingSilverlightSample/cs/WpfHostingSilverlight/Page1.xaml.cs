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

namespace WPFBrowserApplication
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            this.Loaded += new RoutedEventHandler(HomePage_Loaded);
            InitializeComponent();
        }

        void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            // Register the scriptable object
            this.webBrowser.ObjectForScripting = new ScriptableClass();
        }

        private void sendMessageToSilverlightButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              // Call the SendMessageToSilverlightScript method in the SilverlightApplicationTestPage.html
                this.webBrowser.InvokeScript("SendMessageToSilverlightScript", this.msgTextBox.Text);
                
            }
            catch (Exception exc)
            {
                msgTextBox.Text = exc.InnerException.Message.ToString();
            }
        }
    }
}
