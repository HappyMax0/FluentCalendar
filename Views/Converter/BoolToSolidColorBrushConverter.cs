using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.UI.ViewManagement;

namespace CalendarWinUI3.Views.Converter
{
    class BoolToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolValue = (bool)value;
            if(boolValue)
            {
                UISettings uiSettings = new UISettings();
                return new SolidColorBrush(uiSettings.GetColorValue(UIColorType.Accent));
            }
            else
            {
                return new SolidColorBrush(Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            object colorValue = value as SolidColorBrush;
            return !colorValue.Equals(new SolidColorBrush(Colors.Transparent));
        }
    }
}
