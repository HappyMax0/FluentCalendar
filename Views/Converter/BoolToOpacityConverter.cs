using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace CalendarWinUI3.Views.Converter
{
    class BoolToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolValue = (bool)value;

            return (bool)value? 1.0 : 0.5;        
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            double opacityValue = (double)value;
            return opacityValue == 1.0 ? true : false;
        }
    }
}
