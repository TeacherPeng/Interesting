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

namespace ListViewCustomView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ChangeView("GridView");
        }
void SwitchViewMenu(object sender, RoutedEventArgs args)
        {
            MenuItem mi = (MenuItem)sender;
            ChangeView(mi.Header.ToString());
        }

        void ChangeView(string str)
        {
            if (str == "GridView")
            {
                lv.View = lv.FindResource("gridView") as ViewBase;
                currentView.Text = "GridView";
            }
            else if (str == "IconView")
            {
                lv.View = lv.FindResource("iconView") as ViewBase;
                currentView.Text = "IconView";
            }
            else if (str == "TileView")
            {
                //Set the ListView View property to the tileView custom view
                lv.View = lv.FindResource("tileView") as ViewBase;
                currentView.Text = "TileView";
            }
            else if (str == "OneButtonHeaderView")
            {
                lv.View = lv.FindResource("OneButtonHeaderView") 
                     as ViewBase;
                currentView.Text = "OneButtonHeaderView";
            }
        }
    }
}
