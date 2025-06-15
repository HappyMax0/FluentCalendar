using CalendarWinUI3.Models;
using CalendarWinUI3.Models.Utils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using Windows.Globalization;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CalendarWinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public DateTime Time { get; set; } = DateTime.Now;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await iCalendarHelper.GetICS();

            calendarDatePicker.Date = Time;

            NavView_Navigate(typeof(MonthPage));
        }

        private void YearRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            calendarDatePicker.Date = Time;

            NavView_Navigate(typeof(YearPage));
        }

        private void MonthRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            calendarDatePicker.Date = Time;

            NavView_Navigate(typeof(MonthPage));
        }

        private void WeekRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            calendarDatePicker.Date = Time;

            NavView_Navigate(typeof(WeekPage));
        }

        private void DayRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            calendarDatePicker.Date = Time;

            NavView_Navigate(typeof(DayPage));
        }

        private void preBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MonthRadioBtn.IsChecked.HasValue && MonthRadioBtn.IsChecked.Value)
            {
                if (Time.Month == 1)
                {
                    Time = new DateTime(Time.Year - 1, 12, 1);
                }
                else
                {
                    Time = new DateTime(Time.Year, Time.Month - 1, 1);
                }

                calendarDatePicker.Date = Time;

                contentFrame?.Navigate(typeof(MonthPage), Time, new SlideNavigationTransitionInfo()
                    { Effect = SlideNavigationTransitionEffect.FromLeft });

                return;
            }

            if (WeekRadioBtn.IsChecked.HasValue && WeekRadioBtn.IsChecked.Value)
            {           
                if (Time.Day - 7 < 1)
                {
                    if (Time.Month == 1)
                    {
                        Time = new DateTime(Time.Year - 1, 12, 28);
                    }
                    else
                    {
                        Time = new DateTime(Time.Year, Time.Month - 1, 28);
                    }               
                }
                else
                {
                    Time = new DateTime(Time.Year, Time.Month, Time.Day - 7);
                }

                calendarDatePicker.Date = Time;

                contentFrame?.Navigate(typeof(WeekPage), Time, new SlideNavigationTransitionInfo()
                    { Effect = SlideNavigationTransitionEffect.FromLeft });

                return;
            }

            if (DayRadioBtn.IsChecked.HasValue && DayRadioBtn.IsChecked.Value)
            {
                Time = Time.AddDays(-1);

                calendarDatePicker.Date = Time;

                contentFrame?.Navigate(typeof(DayPage), Time, new SlideNavigationTransitionInfo()
                    { Effect = SlideNavigationTransitionEffect.FromLeft });

                return;
            }
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MonthRadioBtn.IsChecked.HasValue && MonthRadioBtn.IsChecked.Value)
            {
                if (Time.Month == 12)
                {
                    Time = new DateTime(Time.Year + 1, 1, 1);
                }
                else
                {
                    Time = new DateTime(Time.Year, Time.Month + 1, 1);
                }

                calendarDatePicker.Date = Time;

                contentFrame?.Navigate(typeof(MonthPage), Time, new SlideNavigationTransitionInfo()
                    { Effect = SlideNavigationTransitionEffect.FromRight });

                return;
            }

            if (WeekRadioBtn.IsChecked.HasValue && WeekRadioBtn.IsChecked.Value)
            {
                int daysCount = DateTime.DaysInMonth(Time.Year, Time.Month);
                if (Time.Day + 7 > daysCount)
                {
                    if (Time.Month == 12)
                    {
                        Time = new DateTime(Time.Year + 1, 1, 1);
                    }
                    else
                    {
                        Time = new DateTime(Time.Year, Time.Month + 1, 1);
                    }
                }
                else
                {
                    Time = new DateTime(Time.Year, Time.Month, Time.Day + 7);
                }

                calendarDatePicker.Date = Time;

                contentFrame?.Navigate(typeof(WeekPage), Time, new SlideNavigationTransitionInfo()
                    { Effect = SlideNavigationTransitionEffect.FromRight });

                return;
            }

            if (DayRadioBtn.IsChecked.HasValue && DayRadioBtn.IsChecked.Value)
            {
                Time = Time.AddDays(1);

                calendarDatePicker.Date = Time;

                contentFrame?.Navigate(typeof(DayPage), Time, new SlideNavigationTransitionInfo()
                    { Effect = SlideNavigationTransitionEffect.FromRight });

                return;
            }

        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            Time = DateTime.Now;

            calendarDatePicker.Date = Time;

            if (MonthRadioBtn.IsChecked.HasValue && MonthRadioBtn.IsChecked.Value)
            {         
                contentFrame?.Navigate(typeof(MonthPage), Time, new DrillInNavigationTransitionInfo());

                return;
            }

            if (WeekRadioBtn.IsChecked.HasValue && WeekRadioBtn.IsChecked.Value)
            {
                contentFrame?.Navigate(typeof(WeekPage), Time, new DrillInNavigationTransitionInfo());

                return;
            }

            if (DayRadioBtn.IsChecked.HasValue && DayRadioBtn.IsChecked.Value)
            {
                contentFrame?.Navigate(typeof(DayPage), Time, new DrillInNavigationTransitionInfo());

                return;
            }

        }

        private void NavView_Navigate(
  Type navPageType)
        {
            if (contentFrame == null) return;
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            Type preNavPageType = contentFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (navPageType is not null && !Type.Equals(preNavPageType, navPageType))
            {
                contentFrame.Navigate(navPageType, Time, new EntranceNavigationTransitionInfo());
            }
        }
    }
}
