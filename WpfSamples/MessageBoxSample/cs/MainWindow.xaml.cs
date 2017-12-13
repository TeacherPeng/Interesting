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

namespace MessageBoxSample
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

        void showMessageBoxButton_Click(object sender, RoutedEventArgs e)
        {
            // Configure the message box
            Window owner = ((bool)ownerCheckBox.IsChecked ? this : null);
            string messageBoxText = this.messageBoxText.Text;
            string caption = this.caption.Text;
            MessageBoxButton button = (MessageBoxButton)Enum.Parse(typeof(MessageBoxButton), this.buttonComboBox.Text);
            MessageBoxImage icon = (MessageBoxImage)Enum.Parse(typeof(MessageBoxImage), this.imageComboBox.Text);
            MessageBoxResult defaultResult = (MessageBoxResult)Enum.Parse(typeof(MessageBoxResult), this.defaultResultComboBox.Text);
            MessageBoxOptions options = (MessageBoxOptions)Enum.Parse(typeof(MessageBoxOptions), this.optionsComboBox.Text);

            // Show message box, passing the window owner if specified
            MessageBoxResult result;
            if (owner == null)
            {
                result = MessageBox.Show(messageBoxText, caption, button, icon, defaultResult, options);
            }
            else
            {
                result = MessageBox.Show(owner, messageBoxText, caption, button, icon, defaultResult, options);
            }
            
            // Show the result
            resultTextBlock.Text = "Result = " + result.ToString();
        }
    }
}