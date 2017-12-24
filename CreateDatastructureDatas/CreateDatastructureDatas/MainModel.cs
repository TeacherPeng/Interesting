using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CreateDatastructureDatas
{
    class MainModel : INotifyPropertyChanged
    {
        public string Data { get => _Data; set { if (_Data == value) return; _Data = value; OnPropertyChanged(nameof(Data)); } }
        private string _Data;

        public int DataCount { get { return _DataCount; } set { if (_DataCount == value) return; _DataCount = value; OnPropertyChanged(nameof(DataCount)); } }
        private int _DataCount = 16;

        public int SwapCount { get { return _SwapCount; } set { if (_SwapCount == value) return; _SwapCount = value; OnPropertyChanged(nameof(SwapCount)); } }
        private int _SwapCount;

        private void OnPropertyChanged([CallerMemberName]string aPropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
        public event PropertyChangedEventHandler PropertyChanged;

        public void CreateDirectedAcyclicGraph()
        {
            DirectedAcyclicGraph aGraph = new DirectedAcyclicGraph();
        }
    }
}
