using System;
using System.Collections.Generic;

namespace CalendarWinUI3.Models
{
    public class Day
    {
        public int YearNo { get; set; }
        public int MonthNo { get; set; }
        public int DayNo { get; set; }
        public int WeekNo { get; set; }
        public bool ShowWeekNo { get; set; }

        public int LunarYear {  get; set; }
        public int LunarMonth { get; set; }
        public string LunarDay { get; set; }

        public DayOfWeek Week { get; set; }

        public bool IsToday { get; set; }


        public bool IsCurrentMonth { get; set; }

        public List<Time> EventList { get; set; } = new List<Time>();

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
