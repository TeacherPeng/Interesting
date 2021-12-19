using System.Windows;

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
    }
}
