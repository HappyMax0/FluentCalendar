using Microsoft.UI.Xaml.Data;
using System;
using System.Text;
using Microsoft.Windows.ApplicationModel.Resources;

namespace CalendarWinUI3.Views.Converter
{
    class WeekNoToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DayOfWeek dayOfWeek = (DayOfWeek)value;
            StringBuilder stringBuilder = new StringBuilder();
            
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    stringBuilder.Append("Week_Sunday");
                    break;
                case DayOfWeek.Monday:
                    stringBuilder.Append("Week_Monday");
                    break;
                case DayOfWeek.Tuesday:
                    stringBuilder.Append("Week_Tuesday");
                    break;
                case DayOfWeek.Wednesday:
                    stringBuilder.Append("Week_Wednesday");
                    break;
                case DayOfWeek.Thursday:
                    stringBuilder.Append("Week_Thursday");
                    break;
                case DayOfWeek.Friday:
                    stringBuilder.Append("Week_Friday");
                    break;
                case DayOfWeek.Saturday:
                    stringBuilder.Append("Week_Saturday");
                    break;
                default:
                    break;
            }

            var loader = new ResourceLoader();
            string text = loader.GetString(stringBuilder.ToString());
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
