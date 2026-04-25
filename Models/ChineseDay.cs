

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using tyme.culture.nine;

namespace CalendarWinUI3.Models
{
    public class ChineseDay : INotifyPropertyChanged
    {
        public DayOfWeek WeekNo { get; set; }

        public int Weeks { get; set; }

        public bool ShowWeekNo { get; set; }

        public bool IsToday { get; set; }

        public int DayNo { get; set; }

        public int MonthNo { get; set; }

        public int YearNo { get; set; }

        public ObservableCollection<Time> Events { get; set; }

        /// <summary>
        /// 公历现代节日
        /// </summary>
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

        /// <summary>
        /// 公历日期
        /// </summary>
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
        /// 農曆日期
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
        /// 干支日期
        /// </summary>
        public string SixtyCycle
        {
            get => _sixtyCycle;
            set
            {
                if (_sixtyCycle != value)
                {
                    _sixtyCycle = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _sixtyCycle;

        /// <summary>
        /// 二十四節氣
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

        public string Constellation
        {
            get => _constellation;
            set
            {
                if(value != _constellation)
                {
                    _constellation = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _constellation;

        /// <summary>
        /// 梅雨天
        /// </summary>
        public string PlumRainDay
        {
            get => _plumRainDay;
            set
            {
                if(value != _plumRainDay)
                {
                    _plumRainDay = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _plumRainDay;

        /// <summary>
        /// 三伏天
        /// </summary>
        public string DogDay
        {
            get => _dogDay;
            set
            {
                if (value != _dogDay)
                {
                    _dogDay = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _dogDay;

        /// <summary>
        /// 数九天
        /// </summary>
        public string NineDay
        {
            get => _nineDay;
            set
            {
                if (_nineDay != value)
                {
                    _nineDay = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _nineDay;

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

        /// <summary>
        /// 吉煞
        /// </summary>
        public string GoodGods
        {
            get => _goodGods;
            set
            {
                if (_goodGods != value)
                {
                    _goodGods = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _goodGods;

        /// <summary>
        /// 凶煞
        /// </summary>
        public string BadGods
        {
            get => _badGods;
            set
            {
                if (_badGods != value)
                {
                    _badGods = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _badGods;

        /// <summary>
        /// 彭祖
        /// </summary>
        public string PengZu
        {
            get => _pengZu;
            set
            {
                if (_pengZu != value)
                {
                    _pengZu = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _pengZu;

        /// <summary>
        /// 胎神
        /// </summary>
        public string FetusDay
        {
            get => _fetusDay;
            set
            {
                if (_fetusDay != value)
                {
                    _fetusDay = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _fetusDay;

        /// <summary>
        /// 日九星
        /// </summary>
        public string NineStar
        {
            get => _nineStar;
            set
            {
                if (_nineStar != value)
                {
                    _nineStar = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _nineStar;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

    }
}
