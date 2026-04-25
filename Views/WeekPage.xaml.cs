using CalendarWinUI3.Models;
using CalendarWinUI3.Models.Utils;
using CalendarWinUI3.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using Windows.Globalization;
using Windows.Storage;
using DayOfWeek = System.DayOfWeek;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CalendarWinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeekPage : Page
    {
        private MainViewModel viewModel;

        public WeekPage()
        {
            this.InitializeComponent();

            weekGridView.SelectionChanged += WeekGridView_SelectionChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is MainViewModel mainViewModel)
            {
                viewModel = mainViewModel;

                var time = viewModel.SelectedDay;

                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

                DayOfWeek dayOfWeek = DayOfWeek.Sunday;
                if (localSettings.Values["StartDay"] is string startDay)
                {
                    if (startDay == "Monday")
                    {
                        dayOfWeek = DayOfWeek.Monday;
                    }
                }

                bool isShowWeekNo = localSettings.Values["ShowWeekNo"] is bool;

                List<ChineseDay> chineseDayList = Helper.GetChineseDays(time, dayOfWeek, isShowWeekNo);

                weekGridView.ItemsSource = chineseDayList;
            }

        }

        private void WeekGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is GridView gridView)
            {
                var selectedDay = gridView.SelectedValue as ChineseDay;
                
                viewModel.IsUpdatingDateFromCode = true;

                viewModel.SelectedDay = new DateTimeOffset(selectedDay.YearNo, selectedDay.MonthNo, selectedDay.DayNo, 0, 0, 0, TimeSpan.Zero);

                viewModel.IsUpdatingDateFromCode = false;
            }
        }
    }
}
