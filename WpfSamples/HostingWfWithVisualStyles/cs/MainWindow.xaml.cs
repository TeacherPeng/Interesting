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
using System.Windows.Forms;

namespace HostingWfWithVisualStyles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Comment out the following line to disable visual
            // styles for the hosted Windows Forms control.
            System.Windows.Forms.Application.EnableVisualStyles();

            // Create a WindowsFormsHost element to host
            // the Windows Forms control.
            System.Windows.Forms.Integration.WindowsFormsHost host = 
                new System.Windows.Forms.Integration.WindowsFormsHost();

            // Create a Windows Forms tab control.
            System.Windows.Forms.TabControl tc = new System.Windows.Forms.TabControl();
            tc.TabPages.Add("Tab1");
            tc.TabPages.Add("Tab2");

            // Assign the Windows Forms tab control as the hosted control.
            host.Child = tc;

            // Assign the host element to the parent Grid element.
            this.grid1.Children.Add(host);
        }

    }
}
