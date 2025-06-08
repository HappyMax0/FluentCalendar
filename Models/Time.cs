using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CalendarWinUI3.Models
{
    public class Time : INotifyPropertyChanged
    {
        public DateTime StartTime
        {
            get => _StartTime;
            set
            {
                _StartTime = value;
                OnPropertyChanged();
            }
        }
        private DateTime _StartTime;

        public string Summary
        {
            get => _Summary;
            set
            {
                _Summary = value;
                OnPropertyChanged();
            }
        }
        private string _Summary;

        public string Description
        { 
            get => _Description;
            set
            {
                _Description = value;
                OnPropertyChanged();
            } 
        }
        private string _Description;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public override string ToString()
        {
            return $"{StartTime} {Summary}";
        }
    }
}
