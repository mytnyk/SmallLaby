using System.ComponentModel;

namespace SmallLabyWpfPlayer
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
