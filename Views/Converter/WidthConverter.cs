﻿using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarWinUI3.Views.Converter
{
    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double width = (double)value;
            
            return width - 250;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            double width = (double)value;
            return width + 250;
        }
    }
}
