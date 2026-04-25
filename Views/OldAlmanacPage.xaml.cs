using CalendarWinUI3.Models.Utils;
using CalendarWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CalendarWinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OldAlmanacPage : Page
    {
        public OldAlmanacViewModel ViewModel { get; } = new OldAlmanacViewModel();


        public OldAlmanacPage()
        {
            InitializeComponent();
            this.Loaded += OldAlmanacPage_Loaded;

            var now = DateTime.Now;
            ViewModel.SelectedDay = new DateTimeOffset(now);

            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values["StartDay"] is string startDay)
            {
                calendarDatePicker.FirstDayOfWeek = startDay.Equals("Monday") ? Windows.Globalization.DayOfWeek.Monday : Windows.Globalization.DayOfWeek.Sunday;
            }
            else
            {
                calendarDatePicker.FirstDayOfWeek = Windows.Globalization.DayOfWeek.Sunday; // default value
            }
        }

        private void OldAlmanacPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavView_Navigate(typeof(OldAlmanac));
        }

        private void preBtn_Click(object sender, RoutedEventArgs e)
        {
            var time = ViewModel.SelectedDay;

            ViewModel.SelectedDay = time.AddDays(-1);

            NavView_Navigate(typeof(OldAlmanac));
        }

        private async void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            var time = ViewModel.SelectedDay;

            ViewModel.SelectedDay = time.AddDays(1);

            NavView_Navigate(typeof(OldAlmanac));

        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedDay = DateTime.Now;

            NavView_Navigate(typeof(OldAlmanac));

        }

        private async void calendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            // 检查是否选择了有效日期
            if (args.NewDate.HasValue && !ViewModel.IsUpdatingDateFromCode)
            {
                ViewModel.SelectedDay = args.NewDate.Value.Date; // 获取选择的日期

                if (args.OldDate == null || args.NewDate.Value.Year != args.OldDate.Value.Year)
                    await HolidayProvider.GetHolidayData(args.NewDate.Value.Year);

                NavView_Navigate(typeof(OldAlmanac));
            }
        }

        private void NavView_Navigate(
 Type navPageType)
        {
            if (contentFrame == null) return;

            if (navPageType is not null)
            {
                contentFrame.Navigate(navPageType, ViewModel, new EntranceNavigationTransitionInfo());
            }
        }

    }
}
