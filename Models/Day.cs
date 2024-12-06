using System.Collections.Generic;
using Windows.Globalization;

namespace CalendarWinUI3.Models
{
    public class Day
    {
        public int YearNo { get; set; }
        public int MonthNo { get; set; }
        public int DayNo { get; set; }

        public int LunarYear {  get; set; }
        public int LunarMonth { get; set; }
        public int LunarDay { get; set; }

        public DayOfWeek Week { get; set; }

        public bool IsToday { get; set; }

        public bool IsCurrentMonth { get; set; }

        public List<Time> EventList { get; set; }

        public Day(int year, int month, int day)
        {
            YearNo = year;
            MonthNo = month;
            DayNo = day;
        }

        public Day()
        {
            
        }
    }
}
