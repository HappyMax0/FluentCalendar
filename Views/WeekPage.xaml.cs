using CalendarWinUI3.Models.Utils;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
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
        public WeekPage()
        {
            this.InitializeComponent();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is DateTime time)
            {
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

                weekGridView.ItemsSource = Helper.GetWeeks(time, dayOfWeek, isShowWeekNo);
            }

        }
    }
}
