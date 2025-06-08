using Microsoft.UI.Xaml.Data;
using System;

namespace CalendarWinUI3.Views.Converter
{
    internal class HeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double height = (double)value;

            return height / 6f;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            double height = (double)value;
            return height * 7f;
        }
    }
}
