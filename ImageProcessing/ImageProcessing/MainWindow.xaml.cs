using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;

namespace ImageProcessing
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _Model = new ImageProcessingModel();
            this.DataContext = _Model;
        }

        private ImageProcessingModel _Model;

        private void OnOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                OpenFileDialog aDlg = new OpenFileDialog();
                aDlg.Filter = "Jpg文件 (*.jpg)|*.jpg|Bmp文件 (*.bmp)|*.bmp|所有文件 (*.*)|*.*";
                if (aDlg.ShowDialog() != true) return;
                _Model.LoadSource(aDlg.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnOpen_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _Model != null;
        }

        private void OnSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                SaveFileDialog aDlg = new SaveFileDialog();
                aDlg.Filter = "Jpg文件 (*.jpg)|*.jpg|Bmp文件 (*.bmp)|*.bmp|所有文件 (*.*)|*.*";
                if (aDlg.ShowDialog() != true) return;
                _Model.SaveResult(aDlg.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _Model?.ResultImage != null;
        }

        private void OnClose_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnClose_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OnProcess_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                _Model.Exec(e.Parameter as Processing);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnProcess_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _Model?.SourceImage != null && e.Parameter as Processing != null;
        }
    }
}
