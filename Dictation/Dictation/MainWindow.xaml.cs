using System;
using System.Windows;
using System.Speech.Synthesis;

namespace Dictation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Model.LoadWords();
            StartQuestion();
        }

        private WordsModel Model => DataContext as WordsModel;

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Model?.SaveWords();
        }

        private void StartQuestion()
        {
            txtWord.Text = null;
            cmdJudges.Visibility = Visibility.Hidden;
            Model?.ChooseWordItem();
            cmdLook.Visibility = Visibility.Visible;
            System.Threading.ThreadPool.QueueUserWorkItem(Read, Model?.CurrentWord?.Word);
        }

        private readonly SpeechSynthesizer speech = new();
        private void Read(object? aWord = null)
        {
            if (aWord is not string aText) return;
            speech.Speak(aText);
        }

        private void OnRead_Click(object sender, RoutedEventArgs e)
        {
            if (Model?.CurrentWord == null) return;
            Read(Model?.CurrentWord?.Word);
        }

        private void OnSkip_Click(object sender, RoutedEventArgs e)
        {
            Model.CurrentWord.Skip = true;
            StartQuestion();
        }

        private void OnLook_Click(object sender, RoutedEventArgs e)
        {
            txtWord.Text = $"{Model?.CurrentWord?.Word}\n\n{Model?.CurrentWord?.Mean}\n\n{Model?.CurrentWord?.Sentence}";
            cmdLook.Visibility = Visibility.Hidden;
            cmdJudges.Visibility = Visibility.Visible;
        }

        private void OnRight_Click(object sender, RoutedEventArgs e)
        {
            Model?.CurrentWord?.DecreaseWeight();
            StartQuestion();
        }

        private void OnWrong_Click(object sender, RoutedEventArgs e)
        {
            Model?.CurrentWord?.InceaseWeight();
            StartQuestion();
        }

        private void OnReadSentence_Click(object sender, RoutedEventArgs e)
        {
            Read(Model?.CurrentWord?.Sentence);
        }

        private void OnEdit_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Edit aEdit = new(Model);
            aEdit.ShowDialog();
        }

        private void OnEdit_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
