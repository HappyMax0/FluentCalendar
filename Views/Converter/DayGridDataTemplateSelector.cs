using CalendarWinUI3.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarWinUI3.Views.Converter
{
    internal class DayGridDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CurrentMonth { get; set; }
        public DataTemplate NotCurrentMonth { get; set; }
        public DataTemplate CurrentDay { get; set; }

        protected override DataTemplate SelectTemplateCore(object obj)
        {
            if (obj is Day day)
            {
                if (day.IsToday)
                    return CurrentDay;
                else
                    return day .IsCurrentMonth? CurrentMonth : NotCurrentMonth;
            }
            else
            {
                return NotCurrentMonth;
            }
        }
    }
}
