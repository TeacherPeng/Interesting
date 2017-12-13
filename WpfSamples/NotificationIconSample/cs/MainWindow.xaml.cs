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
using System.Windows.Forms; // NotifyIcon control
using System.Drawing; // Icon


namespace NotificationIconSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
        }

        void click(object sender, RoutedEventArgs e)
        {
            // Configure and show a notification icon in the system tray
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.BalloonTipText = "Hello, NotifyIcon!";
            this.notifyIcon.Text = "Hello, NotifyIcon!";
            this.notifyIcon.Icon = new System.Drawing.Icon("NotifyIcon.ico");
            this.notifyIcon.Visible = true;
            this.notifyIcon.ShowBalloonTip(1000);
        }
    }
}