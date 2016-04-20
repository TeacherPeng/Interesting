using System;
using System.Windows;
using System.Windows.Controls;

namespace ImageProcessing
{
    /// <summary>
    /// Processing_IncBrightness_Ui.xaml 的交互逻辑
    /// </summary>
    public partial class Processing_IncSaturation_Ui : UserControl
    {
        public Processing_IncSaturation_Ui(Processing_IncSaturation aProcessing)
        {
            InitializeComponent();
            _Processing = aProcessing;
            this.DataContext = aProcessing;
        }
        Processing_IncSaturation _Processing;
        private void OnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _Processing.OnApply();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
