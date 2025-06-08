using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarWinUI3.Views.Converter
{
    internal class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int intValue = (int)value;
            if (intValue > 0)
            {
                return Microsoft.UI.Xaml.Visibility.Visible;
            }
            else
            {
                return Microsoft.UI.Xaml.Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Visibility visibility = (Visibility)value;
            return visibility == Microsoft.UI.Xaml.Visibility.Visible ? 1 : 0;
        }
    }
}
