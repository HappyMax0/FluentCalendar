using CalendarWinUI3.Models;
using CalendarWinUI3.Models.Utils;
using CalendarWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CalendarWinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        private DispatcherTimer _timer;

        private bool showClockSeconds = true;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            this.Unloaded += MainPage_Unloaded;

            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if(localSettings.Values["ShowClockSeconds"] is bool showClockSeconds)
            {
                _timer = new DispatcherTimer();
                _timer.Interval = showClockSeconds ? TimeSpan.FromSeconds(1) : TimeSpan.FromMinutes(1);
                _timer.Tick += _timer_Tick;
                _timer.Start();

                currentTimeTb.Text = showClockSeconds ? DateTime.Now.ToString("HH:mm:ss") :
               DateTime.Now.ToString("HH:mm");
            }

            if (localSettings.Values["StartDay"] is string startDay)
            {
                calendarDatePicker.FirstDayOfWeek = startDay.Equals("Monday") ? Windows.Globalization.DayOfWeek.Monday : Windows.Globalization.DayOfWeek.Sunday;              
            }
            else
            {
                calendarDatePicker.FirstDayOfWeek = Windows.Globalization.DayOfWeek.Sunday; // default value
            }
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _timer?.Stop();
        }

        private void _timer_Tick(object sender, object e)
        {
            currentTimeTb.Text = showClockSeconds? DateTime.Now.ToString("HH:mm:ss") :
                DateTime.Now.ToString("HH:mm");
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavView_Navigate(typeof(MonthPage));
        }

        private void preBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = viewComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                var time = ViewModel.SelectedDay;
                switch (selectedItem.Tag.ToString())
                {
                    case "Year":
                        break;
                    case "Month":
                        if (time.Month == 1)
                        {
                            ViewModel.SelectedDay = new DateTime(time.Year - 1, 12, 1);
                        }
                        else
                        {
                            ViewModel.SelectedDay = new DateTime(time.Year, time.Month - 1, 1);
                        }

                        contentFrame?.Navigate(typeof(MonthPage), ViewModel, new SlideNavigationTransitionInfo()
                        { Effect = SlideNavigationTransitionEffect.FromLeft });

                        break;
                    case "Week":
                        // 计算周日的日期
                        int daysToSunday = (int)time.DayOfWeek;

                        ViewModel.SelectedDay = time.AddDays(-7).Date;

                        contentFrame?.Navigate(typeof(WeekPage), ViewModel, new SlideNavigationTransitionInfo()
                        { Effect = SlideNavigationTransitionEffect.FromLeft });

                        break;
                    case "Day":
                        ViewModel.SelectedDay = time.AddDays(-1);

                        contentFrame?.Navigate(typeof(DayPage), ViewModel, new SlideNavigationTransitionInfo()
                        { Effect = SlideNavigationTransitionEffect.FromLeft });

                        break;
                }
            }

        }

        private async void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = viewComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                var time = ViewModel.SelectedDay;

                switch (selectedItem.Tag.ToString())
                {
                    case "Year":
                        break;
                    case "Month":
                        if (time.Month == 12)
                        {
                            //当前为12月，年份+1
                            ViewModel.SelectedDay = new DateTime(time.Year + 1, 1, 1);

                            await HolidayProvider.GetHolidayData(time.Year);

                        }
                        else
                        {
                            ViewModel.SelectedDay = new DateTime(time.Year, time.Month + 1, 1);
                        }

                        contentFrame?.Navigate(typeof(MonthPage), ViewModel, new SlideNavigationTransitionInfo()
                        { Effect = SlideNavigationTransitionEffect.FromRight });

                        break;
                    case "Week":
                        // 计算周日的日期
                        /*int daysToSunday = (int)Time.DayOfWeek; // DayOfWeek.Sunday = 0
                        Time = Time.AddDays(-daysToSunday).Date;*/

                        var newTime = time.AddDays(7).Date;

                        if (newTime.Year != time.Year)
                            await HolidayProvider.GetHolidayData(newTime.Year);
                        else if(newTime.AddDays(7).Year != newTime.Year)
                            await HolidayProvider.GetHolidayData(newTime.AddDays(7).Year);

                        ViewModel.SelectedDay = newTime;

                        contentFrame?.Navigate(typeof(WeekPage), ViewModel, new SlideNavigationTransitionInfo()
                        { Effect = SlideNavigationTransitionEffect.FromRight });
                        break;
                    case "Day":
                        ViewModel.SelectedDay = time.AddDays(1);

                        contentFrame?.Navigate(typeof(DayPage), ViewModel, new SlideNavigationTransitionInfo()
                        { Effect = SlideNavigationTransitionEffect.FromRight });

                        break;
                }
            }

        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedDay = DateTime.Now;

            var selectedItem = viewComboBox.SelectedItem as ComboBoxItem;
            if(selectedItem != null)
            {
                switch (selectedItem.Tag.ToString())
                {
                    case "Year":
                        contentFrame?.Navigate(typeof(YearPage), ViewModel, new DrillInNavigationTransitionInfo());
                        break;
                    case "Month":
                        contentFrame?.Navigate(typeof(MonthPage), ViewModel, new DrillInNavigationTransitionInfo());
                        break;
                    case "Week":
                        contentFrame?.Navigate(typeof(WeekPage), ViewModel, new DrillInNavigationTransitionInfo());
                        break;
                    case "Day":
                        contentFrame?.Navigate(typeof(DayPage), ViewModel, new DrillInNavigationTransitionInfo());
                        break;
                }
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

        private void viewComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selectedItem = comboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                switch (selectedItem.Tag.ToString())
                {
                    case "Year":
                        NavView_Navigate(typeof(YearPage));
                        break;
                    case "Month":
                        NavView_Navigate(typeof(MonthPage));
                        break;
                    case "Week": 
                        NavView_Navigate(typeof(WeekPage));
                        break;
                    case "Day":
                        NavView_Navigate(typeof(DayPage));
                        break;
                }
            }
        }

        private async void calendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            // 检查是否选择了有效日期
            if (args.NewDate.HasValue)
            {
                ViewModel.SelectedDay = args.NewDate.Value.Date; // 获取选择的日期

                if(args.OldDate == null || args.NewDate.Value.Year != args.OldDate.Value.Year)
                    await HolidayProvider.GetHolidayData(args.NewDate.Value.Year);

                if (viewComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    switch (selectedItem.Tag.ToString())
                    {
                        case "Year":
                            NavView_Navigate(typeof(YearPage));
                            break;
                        case "Month":
                            NavView_Navigate(typeof(MonthPage));
                            break;
                        case "Week":
                            NavView_Navigate(typeof(WeekPage));
                            break;
                        case "Day":
                            NavView_Navigate(typeof(DayPage));
                            break;
                    }
                }             
            }
        }
    }
}
