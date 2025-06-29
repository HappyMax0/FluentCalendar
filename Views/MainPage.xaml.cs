using CalendarWinUI3.Models.Utils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
        private DispatcherTimer _timer;

        public DateTime Time { get; set; } = DateTime.Now;

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

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await iCalendarHelper.GetICS();

            calendarDatePicker.Date = Time;

            NavView_Navigate(typeof(MonthPage));
        }

        private void preBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = viewComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                switch (selectedItem.Tag.ToString())
                {
                    case "Year":
                        break;
                    case "Month":
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

                        break;
                    case "Week":
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

                        break;
                    case "Day":
                        Time = Time.AddDays(-1);

                        calendarDatePicker.Date = Time;

                        contentFrame?.Navigate(typeof(DayPage), Time, new SlideNavigationTransitionInfo()
                        { Effect = SlideNavigationTransitionEffect.FromLeft });

                        break;
                }
            }

        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = viewComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                switch (selectedItem.Tag.ToString())
                {
                    case "Year":
                        break;
                    case "Month":
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

                        break;
                    case "Week":
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
                        break;
                    case "Day":
                        Time = Time.AddDays(1);

                        calendarDatePicker.Date = Time;

                        contentFrame?.Navigate(typeof(DayPage), Time, new SlideNavigationTransitionInfo()
                        { Effect = SlideNavigationTransitionEffect.FromRight });

                        break;
                }
            }

        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            Time = DateTime.Now;

            calendarDatePicker.Date = Time;

            var selectedItem = viewComboBox.SelectedItem as ComboBoxItem;
            if(selectedItem != null)
            {
                switch (selectedItem.Tag.ToString())
                {
                    case "Year":
                        contentFrame?.Navigate(typeof(YearPage), Time, new DrillInNavigationTransitionInfo());
                        break;
                    case "Month":
                        contentFrame?.Navigate(typeof(MonthPage), Time, new DrillInNavigationTransitionInfo());
                        break;
                    case "Week":
                        contentFrame?.Navigate(typeof(WeekPage), Time, new DrillInNavigationTransitionInfo());
                        break;
                    case "Day":
                        contentFrame?.Navigate(typeof(DayPage), Time, new DrillInNavigationTransitionInfo());
                        break;
                }
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

        private void viewComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selectedItem = comboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                calendarDatePicker.Date = Time;
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
