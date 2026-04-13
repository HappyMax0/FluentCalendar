using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarWinUI3.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private DateTimeOffset _selectedDay = DateTime.Now;
        public DateTimeOffset SelectedDay
        {
            get => _selectedDay;
            set
            {
                _selectedDay = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(SelectedDay)));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
