using Ical.Net.DataTypes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Calendar = Windows.Globalization.Calendar;

namespace CalendarWinUI3.Models.Utils
{
    public static class Helper
    {
        public static List<Day> GetDayList(DateTime time)
        {         
            List<Day> dayList = new List<Day>();

            int currentMonth = time.Month;
            int currentYear = time.Year;
            int daysCount = DateTime.DaysInMonth(currentYear, currentMonth);

            //LastMonth
            DateTime firstDayOfCurrentMonth = new DateTime(currentYear, currentMonth, 1);
            DayOfWeek startDayOfWeek = firstDayOfCurrentMonth.DayOfWeek;
            int previousMonth = currentMonth == 1 ? 12 : currentMonth - 1;
            int previousYear = currentMonth == 1 ? currentYear - 1 : currentYear;
            int daysInPreviousMonth = DateTime.DaysInMonth(previousYear, previousMonth);
  
            for (int i = daysInPreviousMonth - (int)startDayOfWeek + 1; i <= daysInPreviousMonth; i++)
            {
                Calendar cal = new Calendar();
                cal.SetDateTime(new DateTime(previousYear, previousMonth, i));
                DateTime dateTime = cal.GetDateTime().DateTime;
                ChineseLunisolarCalendar chineseCalendar = new();
                int lunarYear = chineseCalendar.GetYear(dateTime);
                int lunarMonth = chineseCalendar.GetMonth(dateTime);
                int lunarDay = chineseCalendar.GetDayOfMonth(dateTime);

                string lunarDayStr = Helper.GetLunarFestival(dateTime);
                if (string.IsNullOrEmpty(lunarDayStr))
                    lunarDayStr = Helper.ConvertToLunarDay(lunarDay);

                var singleDay = new Day(dateTime.Year, dateTime.Month, dateTime.Day) { LunarDay = lunarDayStr };

                foreach (var calendar in iCalendarHelper.Calendars)
                {
                    var evetItems = calendar.Events.Where(it => it.Start != null && it.End != null && it.Start.AsSystemLocal >= dateTime && it.End.AsSystemLocal <= dateTime.AddDays(1));
                    if (evetItems != null && evetItems.Count() > 0)
                    {
                        foreach (var evetItem in evetItems)
                        {
                            singleDay.EventList.Add(new Time() { StartTime = evetItem.Start.AsSystemLocal, Summary = evetItem.Summary, Description = evetItem.Description });
                        }
                    }
                }

                dayList.Add(singleDay);
            }

            //CurrentMonth
            DateTime today = DateTime.Today;
            for (int day = 1; day <= daysCount; day++)
            {
                var dateTime = new DateTime(currentYear, currentMonth, day);
                
                DayOfWeek week = dateTime.DayOfWeek;

                bool isToday = day == today.Day && currentMonth == today.Month;

                // 创建农历日历实例
                ChineseLunisolarCalendar chineseCalendar = new();
                int lunarYear = chineseCalendar.GetYear(dateTime);
                int lunarMonth = chineseCalendar.GetMonth(dateTime);
                int lunarDay = chineseCalendar.GetDayOfMonth(dateTime);

                string lunarDayStr = Helper.GetLunarFestival(dateTime);
                if (string.IsNullOrEmpty(lunarDayStr))
                    lunarDayStr = Helper.ConvertToLunarDay(lunarDay);

                var singleDay = new Day(currentYear, currentMonth, day) { Week = week, IsToday = isToday, IsCurrentMonth = true, LunarDay = lunarDayStr };
            
                foreach (var calendar in iCalendarHelper.Calendars)
                {
                    var evetItems = calendar.Events.Where(it => it.Start != null && it.End != null && it.Start.AsSystemLocal >= dateTime && it.End.AsSystemLocal <= dateTime.AddDays(1));
                    if (evetItems != null && evetItems.Count() > 0)
                    {
                        foreach (var evetItem in evetItems)
                        {
                            singleDay.EventList.Add(new Time() { StartTime = evetItem.Start.AsSystemLocal, Summary = evetItem.Summary, Description = evetItem.Description });
                        }
                    }
                }

                dayList.Add(singleDay);
            }

            //NextMonth
            int nextMonth = currentMonth == 12 ? 1 : currentMonth + 1;
            int nextYear = currentMonth == 12 ? currentYear + 1 : currentYear;
            int daysInCurrentMonth = DateTime.DaysInMonth(nextYear, nextMonth);
            int remainingDays = 42 - (dayList.Count);
            for (int i = 1; i <= remainingDays; i++)
            {
                DateTime dateTime = new DateTime(nextYear, nextMonth, i);
                ChineseLunisolarCalendar chineseCalendar = new();
                int lunarYear = chineseCalendar.GetYear(dateTime);
                int lunarMonth = chineseCalendar.GetMonth(dateTime);
                int lunarDay = chineseCalendar.GetDayOfMonth(dateTime);
                string str = chineseCalendar.ToString();

                string lunarDayStr = Helper.GetLunarFestival(dateTime);
                if (string.IsNullOrEmpty(lunarDayStr))
                    lunarDayStr = Helper.ConvertToLunarDay(lunarDay);

                Day singleDay = new Day(dateTime.Year, dateTime.Month, dateTime.Day) { LunarDay = lunarDayStr };

                foreach (var calendar in iCalendarHelper.Calendars)
                {
                    var evetItems = calendar.Events.Where(it => it.Start != null && it.End != null && it.Start.AsSystemLocal >= dateTime && it.End.AsSystemLocal <= dateTime.AddDays(1));
                    if (evetItems != null && evetItems.Count() > 0)
                    {
                        foreach (var evetItem in evetItems)
                        {
                            singleDay.EventList.Add(new Time() { StartTime = evetItem.Start.AsSystemLocal, Summary = evetItem.Summary, Description = evetItem.Description });
                        }
                    }
                }

                dayList.Add(singleDay);
            }

            return dayList;
        }


        public static List<Week> GetWeeks(DateTime time)
        {
            DateTime today = DateTime.Today;

            int timeDay = time.Day;

            int dayOfWeek = (int)time.DayOfWeek;

            int startDay = timeDay - dayOfWeek;

            if(startDay < 1)
            {
                startDay = 1;
            }

            int daysCount = DateTime.DaysInMonth(time.Year, time.Month);
            int nextMonthDay = 1;

            List<Week> weekList = new List<Week>();
            for (int i = 0; i < 7; i++)
            {
                DayOfWeek week = (DayOfWeek)i;

                int day = startDay + i;

                DateTime dateTime;

                if (day > daysCount)
                {
                    if (time.Month < 12)
                    {
                        dateTime = new DateTime(time.Year, time.Month + 1, nextMonthDay);
                    }
                    else
                    {
                        dateTime = new DateTime(time.Year + 1, 1, nextMonthDay);
                    }

                    nextMonthDay++;
                }
                else
                    dateTime = new DateTime(time.Year, time.Month, day);

                var weekObj = new Week() { WeekNo = week, DayNo = dateTime.Day, IsToday = (dateTime.Day == today.Day && dateTime.Month == today.Month && dateTime.Year == today.Year) };
                weekObj.Events = new();
        
                foreach (var calendar in iCalendarHelper.Calendars)
                {
                    var evetItems = calendar.Events.Where(it => it.Start != null && it.End != null && it.Start.AsSystemLocal >= dateTime && it.End.AsSystemLocal <= dateTime.AddDays(1));
                    if (evetItems != null && evetItems.Count() > 0)
                    {
                        foreach (var evetItem in evetItems)
                        {
                            weekObj.Events.Add(new Time() { StartTime = evetItem.Start.AsSystemLocal, Summary = evetItem.Summary, Description = evetItem.Description });
                        }
                    }
                }

                weekList.Add(weekObj);
            }
            return weekList;
        }

        public static Day GetDay(DateTime time)
        {      
            Day day = new Day();

            day.YearNo = time.Year;
            day.MonthNo = time.Month;
            day.DayNo = time.Day;
            day.Week = time.DayOfWeek;

            List<Time> timeList = new List<Time>();

            DateTime dateTime = new DateTime(time.Year, time.Month, time.Day);

            for (int i = 0; i < 24; i++)
            {
                timeList.Add(new Time() { StartTime = dateTime.AddHours(i), Description = String.Empty });
            }

            day.EventList = timeList;

            return day;
        }

        // 辅助函数：查找 Visual Tree 中的子元素
        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        public static string ConvertToLunarDate(DateTime date)
        {
            ChineseLunisolarCalendar chineseCalendar = new ChineseLunisolarCalendar();
            int year = chineseCalendar.GetYear(date);
            int month = chineseCalendar.GetMonth(date);
            int day = chineseCalendar.GetDayOfMonth(date);
            bool isLeapMonth = chineseCalendar.IsLeapMonth(year, month);
            string[] chineseMonths = { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "冬", "腊" };
            string[] chineseDays = { "初一", "初二", "初三", "初四", "初五", "初六", "初七", "初八", "初九", "初十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十", "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十" };
            string lunarMonth = chineseMonths[month - 1];
            string lunarDay = chineseDays[day - 1];
            if (isLeapMonth) { lunarMonth = "闰" + lunarMonth; }
            return $"农历{year}年{lunarMonth}月{lunarDay}";
        }

        public static string ConvertToLunarDay(int day)
        {
            ChineseLunisolarCalendar chineseCalendar = new ChineseLunisolarCalendar();
           
            string[] chineseMonths = { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "冬", "腊" };
            string[] chineseDays = { "初一", "初二", "初三", "初四", "初五", "初六", "初七", "初八", "初九", "初十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十", "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十" };
          
            string lunarDay = chineseDays[day - 1];
           
            return lunarDay;
        }

        public static string GetLunarFestival(DateTime date)
        {
            ChineseLunisolarCalendar chineseCalendar = new ChineseLunisolarCalendar(); 
            int year = chineseCalendar.GetYear(date); 
            int month = chineseCalendar.GetMonth(date); 
            int day = chineseCalendar.GetDayOfMonth(date); 
            bool isLeapMonth = chineseCalendar.IsLeapMonth(year, month); 
            // 农历节日判断逻辑
            if (month == 1 && day == 1) { return "春节"; } 
            else if (month == 1 && day == 15) { return "元宵节"; } 
            else if (month == 5 && day == 5) { return "端午节"; } 
            else if (month == 8 && day == 15) { return "中秋节"; } 
            else if (month == 7 && day == 7) { return "七夕节"; } 
            else if (month == 9 && day == 9) { return "重阳节"; } 
            else if (month == 12 && day == 8) { return "腊八节"; } 
            else if (month == 12 && day == 23) { return "小年"; } 
            else { return string.Empty; } }
    }
}
