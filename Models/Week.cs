
using Windows.Globalization;

namespace CalendarWinUI3.Models
{
    public class Week
    {
        public DayOfWeek WeekNo { get; set; }

        public bool IsToday { get; set; }

        public int DayNo { get; set; }
    }
}
