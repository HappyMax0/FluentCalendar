using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalendarWinUI3.Models
{
    public class Subscription : INotifyPropertyChanged
    {
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }
        private string _Name = string.Empty;

        public string Url
        {
            get => _Url;
            set
            {
                _Url = value;
                OnPropertyChanged();
            }
        }
        private string _Url = string.Empty;

        public bool IsEnabled
        {
            get => _IsEnabled;
            set
            {
                _IsEnabled = value;
                OnPropertyChanged();
            }
        }
        private bool _IsEnabled = true;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
