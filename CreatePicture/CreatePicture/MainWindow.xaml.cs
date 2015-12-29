using Microsoft.Win32;
using System;
using System.Windows;

namespace CreatePicture
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _Model = new PictureModel();
            this.DataContext = _Model;
        }
        private PictureModel _Model;
        private void OnExec_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _Model.Exec();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnSaveCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog aDlg = new SaveFileDialog();
                aDlg.Filter = "代码文件 (*.code)|*.code|所有文件 (*.*)|*.*";
                if (aDlg.ShowDialog() != true) return;
                _Model.SaveCode(aDlg.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnSavePicture_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            try
            {
                SaveFileDialog aDlg = new SaveFileDialog();
                aDlg.Filter = "Jpeg文件 (*.jpg)|*.jpg|所有文件 (*.*)|*.*";
                if (aDlg.ShowDialog() != true) return;
                _Model.SavePicture(aDlg.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnSavePicture_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _Model != null && _Model.Picture != null;
        }

        private void OnLoadCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog aDlg = new OpenFileDialog();
                aDlg.Filter = "代码文件 (*.code)|*.code|所有文件 (*.*)|*.*";
                if (aDlg.ShowDialog() != true) return;
                _Model.LoadCode(aDlg.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
