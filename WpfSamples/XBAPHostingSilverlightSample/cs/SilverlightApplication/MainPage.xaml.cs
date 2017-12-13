using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;

namespace SilverlightApplication
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            ScriptableClass scClass = new ScriptableClass();
            HtmlPage.RegisterScriptableObject("scrptClass", scClass);
        }


        private void sendMessageToWPFButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Call the SendMessageToWPFScript method in the SilverlightApplicationTestPage.html
               HtmlPage.Window.Invoke("SendMessageToWPFScript", this.msgTextBox.Text);
               
            }
            catch (Exception exc)
            {
                msgTextBlock.Text = exc.Message.ToString();
            }
        }
    }
}
