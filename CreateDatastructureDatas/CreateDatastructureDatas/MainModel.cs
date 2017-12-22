using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CreateDatastructureDatas
{
    class MainModel : INotifyPropertyChanged
    {
        private void OnPropertyChanged([CallerMemberName]string aPropertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
