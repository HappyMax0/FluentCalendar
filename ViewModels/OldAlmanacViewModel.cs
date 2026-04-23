using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarWinUI3.ViewModels
{
    public class OldAlmanacViewModel : INotifyPropertyChanged
    {
        private String _solarDate;
        public String SolarDate
        {
            get => _solarDate;
            set
            {
                _solarDate = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(SolarDate)));
            }
        }

        private String _lunarDate;
        public String LunarDate
        {
            get => _lunarDate;
            set
            {
                _lunarDate = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(LunarDate)));
            }
        }

        private String _sixtyCycleDate;
        public String SixtyCycleDate
        {
            get => _sixtyCycleDate;
            set
            {
                _sixtyCycleDate = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(SixtyCycleDate)));
            }
        }

        private String _recommends;
        public String Recommends
        {
            get => _recommends;
            set
            {
                _recommends = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(Recommends)));
            }
        }

        private String _avoids;
        public String Avoids
        {
            get => _avoids;
            set
            {
                _avoids = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(Avoids)));
            }
        }

        private String _goodGods;
        public String GoodGods
        {
            get => _goodGods;
            set
            {
                _goodGods = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(GoodGods)));
            }
        }

        private String _badGods;
        public String BadGods
        {
            get => _badGods;
            set
            {
                _badGods = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(BadGods)));
            }
        }

        private String _pengZu;
        public String PengZu
        {
            get => _pengZu;
            set
            {
                _pengZu = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(PengZu)));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
