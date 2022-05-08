using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Dictation
{
    public class WordsModel : INotifyPropertyChanged
    {
        private const string WordsFileName = "words.xml";

        public void ChooseWordItem()
        {
            int aSum = (from r in Words where !r.Skip select r.Weight).Sum();
            int aPos = _Random.Next(aSum);
            foreach (var aWordItem in from r in Words where !r.Skip select r)
            {
                aPos -= aWordItem.Weight;
                if (aPos <= 0)
                {
                    CurrentWord = aWordItem;
                    return;
                }
            }
        }

        public void LoadWords()
        {
            XDocument aXDocument = XDocument.Load(WordsFileName);
            Words.Clear();
            var aXElements = aXDocument?.Root?.Elements(nameof(WordItem));
            if (aXElements == null) return;
            foreach (var aWordXElement in aXElements)
            {
                WordItem aWordItem = new();
                aWordItem.ReadFromXml(aWordXElement);
                Words.Add(aWordItem);
            }
        }

        public void SaveWords()
        {
            XDocument aXDocument = new(new XElement("Words", from r in Words select r.CreateXElement(nameof(WordItem))));
            aXDocument.Save(WordsFileName);
        }

        public WordItem? CurrentWord { get => _CurrentWord; set { if (_CurrentWord == value) return; _CurrentWord = value; OnPropertyChanged(nameof(CurrentWord)); } }
        private WordItem? _CurrentWord;

        public ObservableCollection<WordItem> Words { get; } = new();

        private readonly Random _Random = new();

        private void OnPropertyChanged([CallerMemberName]string? aPropertyName = null, params string[] aPropertyNames)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
            foreach (var aName in aPropertyNames) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
