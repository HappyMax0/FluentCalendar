using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using tyme.culture;
using tyme.festival;
using tyme.lunar;
using tyme.solar;
using Calendar = Windows.Globalization.Calendar;
using String = System.String;

namespace CalendarWinUI3.Models.Utils
{
    public static class Helper
    {
        public static List<Day> GetDayList(DateTime time, DayOfWeek firstDayOfWeek = DayOfWeek.Sunday, bool isShowWeekNo = false)
        {         
            List<Day> dayList = new List<Day>();       

            int currentMonth = time.Month;
            int currentYear = time.Year;
            int daysInCurrentMonth = DateTime.DaysInMonth(currentYear, currentMonth);
            DateTime today = DateTime.Today;

            //当前月的第一天
            DateTime firstDayOfCurrentMonth = new DateTime(currentYear, currentMonth, 1);
            DayOfWeek startDayOfWeek = firstDayOfCurrentMonth.DayOfWeek;

            // 计算需要补齐的上一月天数
            int daysToFirstDay = ((int)startDayOfWeek - (int)firstDayOfWeek + 7) % 7;
            int previousMonth = currentMonth == 1 ? 12 : currentMonth - 1;
            int previousYear = currentMonth == 1 ? currentYear - 1 : currentYear;
            int daysInPreviousMonth = DateTime.DaysInMonth(previousYear, previousMonth);

            // 上一月补齐天数
            for (int i = daysInPreviousMonth - daysToFirstDay + 1; i <= daysInPreviousMonth; i++)
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

                int weekNo = new GregorianCalendar().GetWeekOfYear(
            dateTime,
            CalendarWeekRule.FirstDay, // 定义一年第一周的规则
            firstDayOfWeek          // 定义每周的第一天
        );

                var showWeekNo = (dateTime.DayOfWeek == firstDayOfWeek) && isShowWeekNo;

                var singleDay = new Day(dateTime.Year, dateTime.Month, dateTime.Day) { WeekNo = weekNo, ShowWeekNo = showWeekNo, LunarDay = lunarDayStr };

                bool isHoliday = false;
                bool isOverrideWorkday = false;
                string name = string.Empty;
                var holidayData = HolidayProvider.HolidayDatas.FirstOrDefault(h => h.Year == dateTime.Year);
                if (holidayData != null)
                {
                    var holidayDay = holidayData.Days.FirstOrDefault(x=>x.Date == dateTime);
                    if(holidayDay != null)
                    {
                        isHoliday = holidayDay.IsOffDay;
                        isOverrideWorkday = !holidayDay.IsOffDay;
                        name = holidayDay.Name;
                    }
                }
                
                singleDay.IsHoliday = isHoliday;
                if(isHoliday)
                    singleDay.EventList.Add(new Time() { Summary = name });

                singleDay.IsWorkdayOverride = isOverrideWorkday && !string.IsNullOrEmpty(name);
                if(isOverrideWorkday)
                    singleDay.EventList.Add(new Time() { Summary = name });

                dayList.Add(singleDay);
            }

            // 当前月
            for (int day = 1; day <= daysInCurrentMonth; day++)
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

                int weekNo = new GregorianCalendar().GetWeekOfYear(
         dateTime,
         CalendarWeekRule.FirstDay, // 定义一年第一周的规则
         firstDayOfWeek          // 定义每周的第一天
     );

                var showWeekNo = (dateTime.DayOfWeek == firstDayOfWeek) && isShowWeekNo;

                var singleDay = new Day(currentYear, currentMonth, day) { Week = week, WeekNo = weekNo, ShowWeekNo = showWeekNo, IsToday = isToday, IsCurrentMonth = true, LunarDay = lunarDayStr };

                bool isHoliday = false;
                bool isOverrideWorkday = false;
                string name = string.Empty;
                var holidayData = HolidayProvider.HolidayDatas.FirstOrDefault(h => h.Year == dateTime.Year);
                if (holidayData != null)
                {
                    var holidayDay = holidayData.Days.FirstOrDefault(x => x.Date == dateTime);
                    if (holidayDay != null)
                    {
                        isHoliday = holidayDay.IsOffDay;
                        isOverrideWorkday = !holidayDay.IsOffDay;
                        name = holidayDay.Name;
                    }
                }

                singleDay.IsHoliday = isHoliday;
                if (isHoliday)
                    singleDay.EventList.Add(new Time() { Summary = name });

               
                singleDay.IsWorkdayOverride = isOverrideWorkday && !string.IsNullOrEmpty(name);
                if (isOverrideWorkday)
                    singleDay.EventList.Add(new Time() { Summary = name });

                dayList.Add(singleDay);
            }

            // 下一月补齐天数（确保总共 42 天）
            int nextMonth = currentMonth == 12 ? 1 : currentMonth + 1;
            int nextYear = currentMonth == 12 ? currentYear + 1 : currentYear;   
            int remainingDays = 42 - dayList.Count;

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

                int weekNo = new GregorianCalendar().GetWeekOfYear(
        dateTime,
        CalendarWeekRule.FirstDay, // 定义一年第一周的规则
        firstDayOfWeek          // 定义每周的第一天
    );

                var showWeekNo = (dateTime.DayOfWeek == firstDayOfWeek) && isShowWeekNo;

                Day singleDay = new Day(dateTime.Year, dateTime.Month, dateTime.Day) { WeekNo = weekNo, ShowWeekNo = showWeekNo, LunarDay = lunarDayStr };

                bool isHoliday = false;
                bool isOverrideWorkday = false;
                string name = string.Empty;
                var holidayData = HolidayProvider.HolidayDatas.FirstOrDefault(h => h.Year == dateTime.Year);
                if (holidayData != null)
                {
                    var holidayDay = holidayData.Days.FirstOrDefault(x => x.Date == dateTime);
                    if (holidayDay != null)
                    {
                        isHoliday = holidayDay.IsOffDay;
                        isOverrideWorkday = !holidayDay.IsOffDay;
                        name = holidayDay.Name;
                    }
                }

                singleDay.IsHoliday = isHoliday;
                if(isHoliday)
                    singleDay.EventList.Add(new Time() { Summary = name });
                
                singleDay.IsWorkdayOverride = isOverrideWorkday && !string.IsNullOrEmpty(name); ;
                if (isOverrideWorkday)
                    singleDay.EventList.Add(new Time() { Summary = name });

                dayList.Add(singleDay);
            }

            return dayList;
        }


        public static List<Week> GetWeeks(DateTime time, DayOfWeek firstDayOfWeek = DayOfWeek.Sunday, bool isShowWeekNo = false)
        {
            DateTime today = DateTime.Today;

            DateTime startDay;
           if(firstDayOfWeek == DayOfWeek.Sunday)
           {
                // 计算周日的日期
                int daysToSunday = (int)time.DayOfWeek; // DayOfWeek.Sunday = 0
                startDay = time.AddDays(-daysToSunday).Date;
           }
           else
           {
                // 计算周一的日期
                int daysToMonday = (int)time.DayOfWeek - (int)DayOfWeek.Monday;
                if (daysToMonday < 0) daysToMonday += 7; // 处理周日（DayOfWeek.Sunday = 0）
                startDay = time.AddDays(-daysToMonday).Date;
            }

            // 生成一周的日期列表（周日到周六）
            List<Week> weekList = new List<Week>();
            for (int i = 0; i < 7; i++)
            {
                var weekDate = startDay.AddDays(i);

                var week = GetWeek(weekDate, today, firstDayOfWeek, isShowWeekNo);
               
                weekList.Add(week);
            }

            return weekList;
        }

        public static Week GetWeek(DateTime weekDate, DateTime today, DayOfWeek firstDayOfWeek = DayOfWeek.Sunday, bool isShowWeekNo = false)
        {
            int weeks = new GregorianCalendar().GetWeekOfYear(
           weekDate,
           CalendarWeekRule.FirstDay, // 定义一年第一周的规则
           firstDayOfWeek          // 定义每周的第一天
       );

            var showWeekNo = (weekDate.DayOfWeek == firstDayOfWeek) && isShowWeekNo;

            var week = new Week() { WeekNo = weekDate.DayOfWeek, DayNo = weekDate.Day, Weeks = weeks, ShowWeekNo = showWeekNo, IsToday = (weekDate.Day == today.Day && weekDate.Month == today.Month && weekDate.Year == today.Year) };
            week.YearNo = weekDate.Year;
            week.MonthNo = weekDate.Month;
            week.Events = new();

            var holidayData = HolidayProvider.HolidayDatas.FirstOrDefault(h => h.Year == weekDate.Year);
            if (holidayData != null)
            {
                var holidayDateTime = holidayData.Days.FirstOrDefault(x => x.Date == weekDate.Date);
                if (holidayDateTime != null)
                    week.Events.Add(new Time() { Summary = holidayDateTime.Name, Description = holidayDateTime.IsOffDay ? "放假" : "补班" });
            }

            //公历
            SolarDay solarDay = SolarDay.FromYmd(week.YearNo, week.MonthNo, week.DayNo);
            LunarDay lunarDay = solarDay.GetLunarDay();
            SolarFestival solarFestival = solarDay.Festival;
            if (solarFestival != null)
            {
                week.SolarFestival = solarFestival.GetName();
            }

            LunarFestival lunarFestival = lunarDay.Festival;
            if (lunarFestival != null)
            {
                week.LunarFestival = lunarFestival.GetName();
            }
            // 宜：嫁娶, 祭祀, 理发, 作灶, 修饰垣墙, 平治道涂, 整手足甲, 沐浴, 冠笄
            List<Taboo> recommends = lunarDay.Recommends;
            // 忌：破土, 出行, 栽种
            List<Taboo> avoids = lunarDay.Avoids;

            week.SolarDay = $"{solarDay.Year}/{solarDay.Month}/{solarDay.Day}";
            week.LunarDay = $"{lunarDay.LunarMonth.GetName()} {lunarDay.GetName()}";
            week.StemsAndBranches = lunarDay.SixtyCycle.GetName();
            week.SolarTerms = lunarDay.GetSolarDay().Term.GetName();
            week.Recommends = string.Join("、", recommends.Select(x => x.GetName())).TrimEnd('、');
            week.Avoids = string.Join("、", avoids.Select(x => x.GetName())).TrimEnd('、');

            return week;
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
            SolarDay solarDay = SolarDay.FromYmd(date.Year, date.Month, date.Day);
            LunarFestival lunarFestival = solarDay.GetLunarDay().Festival;
            if (lunarFestival != null)
            {
                return lunarFestival.GetName();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
