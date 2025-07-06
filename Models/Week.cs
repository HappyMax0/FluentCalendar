

using System;
using System.Collections.ObjectModel;

namespace CalendarWinUI3.Models
{
    public class Week
    {
        public DayOfWeek WeekNo { get; set; }

        public int Weeks { get; set; }

        public bool ShowWeekNo { get; set; }

        public bool IsToday { get; set; }

        public int DayNo { get; set; }

        public ObservableCollection<Time> Events { get; set; }
    }
}
