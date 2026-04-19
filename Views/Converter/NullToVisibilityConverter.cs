using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace CalendarWinUI3.Views.Converter
{
    internal class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
           
            return value != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Visibility visibility = (Visibility)value;
            return visibility == Visibility.Visible ? value : null;
        }
    }
}
