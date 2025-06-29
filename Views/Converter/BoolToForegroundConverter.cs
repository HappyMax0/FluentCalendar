using CalendarWinUI3.Views.Helpers;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.UI;

namespace CalendarWinUI3.Views.Converter
{
    internal class BoolToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolValue = (bool)value;
            if (boolValue)
            {
                return new SolidColorBrush(Colors.White);
            }
            else
            {
                //if (ThemeHelper.RootTheme == ElementTheme.Light)
                //{
                //    return new SolidColorBrush(Colors.White);
                //}
                //else
                //{
                //    return new SolidColorBrush(Colors.Black);
                //}

                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            object colorValue = value as SolidColorBrush;
            return colorValue.Equals(new SolidColorBrush(Colors.White));
        }
    }
}
