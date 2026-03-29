

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalendarWinUI3.Models
{
    public class Week : INotifyPropertyChanged
    {
        public DayOfWeek WeekNo { get; set; }

        public int Weeks { get; set; }

        public bool ShowWeekNo { get; set; }

        public bool IsToday { get; set; }

        public int DayNo { get; set; }

        public int MonthNo { get; set; }

        public int YearNo { get; set; }

        public ObservableCollection<Time> Events { get; set; }

        public string SolarFestival
        {
            get => _solarFestival;
            set
            {
                if (_solarFestival != value)
                {
                    _solarFestival = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _solarFestival;

        /// <summary>
        /// 農曆節日
        /// </summary>
        public string LunarFestival
        {
            get => _lunarFestival;
            set
            {
                if (_lunarFestival != value)
                {
                    _lunarFestival = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _lunarFestival;

        public string SolarDay
        {
            get => _solarDay;
            set
            {
                if (_solarDay != value)
                {
                    _solarDay = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _solarDay;

        /// <summary>
        /// 農曆日
        /// </summary>
        public string LunarDay
        {
            get => _lunarDay;
            set
            {
                if (_lunarDay != value)
                {
                    _lunarDay = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _lunarDay;

        /// <summary>
        /// 干支
        /// </summary>
        public string StemsAndBranches
        {
            get => _stemsAndBranches;
            set
            {
                if (_stemsAndBranches != value)
                {
                    _stemsAndBranches = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _stemsAndBranches;

        /// <summary>
        /// 節氣
        /// </summary>
        public string SolarTerms
        {
            get => _solarTerms;
            set
            {
                if (_solarTerms != value)
                {
                    _solarTerms = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _solarTerms;

        /// <summary>
        /// 宜
        /// </summary>
        public string Recommends
        {
            get => _recommends;
            set
            {
                if (_recommends != value)
                {
                    _recommends = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _recommends;

        /// <summary>
        /// 忌
        /// </summary>
        public string Avoids
        {
            get => _avoids;
            set
            {
                if (_avoids != value)
                {
                    _avoids = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _avoids;


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

    }
}
