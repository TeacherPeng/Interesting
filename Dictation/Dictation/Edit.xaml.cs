using System.Windows;
using Microsoft.Win32;

namespace Dictation
{
    /// <summary>
    /// Edit.xaml 的交互逻辑
    /// </summary>
    public partial class Edit : Window
    {
        public Edit(WordsModel aModel)
        {
            InitializeComponent();
            DataContext = aModel;
        }

        private WordsModel Model => DataContext as WordsModel;

        private void OnImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog aDlg = new OpenFileDialog()
            {
                Filter = "Excel|*.xls*",
            };
            if (aDlg.ShowDialog() != true) return;

        }
    }
}
