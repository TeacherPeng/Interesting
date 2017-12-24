using System;
using System.Windows;

namespace CreateDatastructureDatas
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _Model = new MainModel();
            this.DataContext = _Model;
        }

        private MainModel _Model;

        private void Exec(Action aAction)
        {
            try
            {
                aAction();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnSequence_Click(object sender, RoutedEventArgs e)
        {
        }

        private void OnBiTree_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OnConnectedGraph_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OnDirectedAcyclicGraph_Click(object sender, RoutedEventArgs e)
        {
            Exec(_Model.CreateDirectedAcyclicGraph);
        }
    }
}
