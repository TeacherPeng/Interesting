﻿using System;
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
using MyControls;

namespace WpfHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Application app;
        private Window myWindow;
        FontWeight initFontWeight;
        Double initFontSize;
        FontStyle initFontStyle;
        SolidColorBrush initBackBrush;
        SolidColorBrush initForeBrush;
        FontFamily initFontFamily;
        bool UIIsReady = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Init(object sender, EventArgs e)
        {
            app = System.Windows.Application.Current;
            myWindow = (Window)app.MainWindow;
            myWindow.SizeToContent = SizeToContent.WidthAndHeight;
            wfh.TabIndex = 10;
            initFontSize = wfh.FontSize;
            initFontWeight = wfh.FontWeight;
            initFontFamily = wfh.FontFamily;
            initFontStyle = wfh.FontStyle;
            initBackBrush = (SolidColorBrush)wfh.Background;
            initForeBrush = (SolidColorBrush)wfh.Foreground;
            (wfh.Child as MyControl1).OnButtonClick += new MyControl1.MyControlEventHandler(Pane1_OnButtonClick);
            UIIsReady = true;
        }

        private void BackColorChanged(object sender, RoutedEventArgs e)
        {
            if (sender == rdbtnBackGreen)
                wfh.Background = new SolidColorBrush(Colors.LightGreen);
            else if (sender == rdbtnBackSalmon)
                wfh.Background = new SolidColorBrush(Colors.LightSalmon);
            else if (UIIsReady == true)
                wfh.Background = initBackBrush;
        }

        private void ForeColorChanged(object sender, RoutedEventArgs e)
        {
            if (sender == rdbtnForeRed)
                wfh.Foreground = new SolidColorBrush(Colors.Red);
            else if (sender == rdbtnForeYellow)
                wfh.Foreground = new SolidColorBrush(Colors.Yellow);
            else if (UIIsReady == true)
                wfh.Foreground = initForeBrush;
        }

        private void FontChanged(object sender, RoutedEventArgs e)
        {
            if (sender == rdbtnTimes)
                wfh.FontFamily = new FontFamily("Times New Roman");
            else if (sender == rdbtnWingdings)
                wfh.FontFamily = new FontFamily("Wingdings");
            else if (UIIsReady == true)
                wfh.FontFamily = initFontFamily;
        }
        private void FontSizeChanged(object sender, RoutedEventArgs e)
        {
            if (sender == rdbtnTen)
                wfh.FontSize = 10;
            else if (sender == rdbtnTwelve)
                wfh.FontSize = 12;
            else if (UIIsReady == true)
                wfh.FontSize = initFontSize;
        }
        private void StyleChanged(object sender, RoutedEventArgs e)
        {
            if (sender == rdbtnItalic)
                wfh.FontStyle = FontStyles.Italic;
            else if (UIIsReady == true)
                wfh.FontStyle = initFontStyle;
        }
        private void WeightChanged(object sender, RoutedEventArgs e)
        {
            if (sender == rdbtnBold)
                wfh.FontWeight = FontWeights.Bold;
            else if (UIIsReady == true)
                wfh.FontWeight = initFontWeight;
        }

        //Handle button clicks on the Windows Form control
        private void Pane1_OnButtonClick(object sender, MyControlEventArgs args)
        {
            txtName.Inlines.Clear();
            txtAddress.Inlines.Clear();
            txtCity.Inlines.Clear();
            txtState.Inlines.Clear();
            txtZip.Inlines.Clear();

            if (args.IsOK)
            {
                txtName.Inlines.Add( " " + args.MyName );
                txtAddress.Inlines.Add( " " + args.MyStreetAddress );
                txtCity.Inlines.Add( " " + args.MyCity );
                txtState.Inlines.Add( " " + args.MyState );
                txtZip.Inlines.Add( " " + args.MyZip );
            }
        }
    }
}
