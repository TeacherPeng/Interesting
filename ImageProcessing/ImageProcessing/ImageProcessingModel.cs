using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImageProcessing
{
    class ImageProcessingModel : INotifyPropertyChanged
    {
        public void LoadSource(string aFileName)
        {
            Uri aUri = new Uri(aFileName);
            SourceImage = new BitmapImage(aUri);
        }

        public void SaveResult(string aFileName)
        {
            JpegBitmapEncoder aEncoder = new JpegBitmapEncoder();
            aEncoder.Frames.Add(BitmapFrame.Create(ResultImage));
            using (FileStream aStream = new FileStream(aFileName, FileMode.CreateNew))
            {
                aEncoder.Save(aStream);
                aStream.Close();
            }
        }

        public void Exec(Processing aProcessing)
        {
            ResultImage = aProcessing.GetResultImage(SourceImage);
        }

        public BitmapImage SourceImage { get { return _SourceImage; } set { if (_SourceImage == value) return; _SourceImage = value; OnPropertyChanged(nameof(SourceImage)); } }
        private BitmapImage _SourceImage;

        public BitmapSource ResultImage { get { return _ResultImage; } set { if (_ResultImage == value) return; _ResultImage = value; OnPropertyChanged(nameof(ResultImage)); } }
        private BitmapSource _ResultImage;

        public Processing[] Processings { get { return _Processings; } }
        private static readonly Processing[] _Processings = new Processing[]
        {
            new Processing01(),
        };

        private void OnPropertyChanged(string aPropertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName)); }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
