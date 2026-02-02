using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace CalendarWinUI3.Views.Converter
{
    internal class HolidayForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolValue = (bool)value;
            if (boolValue)
            {
                UISettings uiSettings = new UISettings();
                return new SolidColorBrush(Colors.Green);
            }
            else
            {
                UISettings uiSettings = new UISettings();
                return new SolidColorBrush(uiSettings.GetColorValue(UIColorType.Accent));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
