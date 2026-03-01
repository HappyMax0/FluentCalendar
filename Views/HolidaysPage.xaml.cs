using CalendarWinUI3.Models;
using CalendarWinUI3.Models.Utils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CalendarWinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HolidaysPage : Page
    {
        public HolidaysPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            datePicker.Date = DateTime.Now;
            getHolidayDatas(datePicker.Date.Year);

        }

        private void datePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            if(e.NewDate.Year != e.OldDate.Year)
                getHolidayDatas(e.NewDate.Year);
        }

        private void getHolidayDatas(int year)
        {
            List<Holiday> holidays = new List<Holiday>();
           
            var holidayData = HolidayProvider.HolidayDatas.FirstOrDefault(x => x.Year == year);
            if (holidayData != null)
            {
                foreach (var item in holidayData.Days)
                {
                    var holiday = holidays.FirstOrDefault(x => x.Name == item.Name);
                    if (holiday == null)
                    {
                        holiday = new Holiday();
                        holiday.Name = item.Name;
                        holidays.Add(holiday);
                    }

                    if (item.IsOffDay)
                    {
                        //放假
                        holiday.Holidays.Add(item.Date.ToString("yyyy/MM/dd"));
                    }
                    else
                    {
                        //上班
                        holiday.Workdays.Add(item.Date.ToString("yyyy/MM/dd"));
                    }
                }
            }

            holidayTreeView.ItemsSource = holidays;
        }

        public Visibility SetHeaderVisbility(List<String> list)
        {
            return list.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
