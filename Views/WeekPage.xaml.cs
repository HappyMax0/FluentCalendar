using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using CalendarWinUI3.Models.Utils;
using CalendarWinUI3.Models;
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
        public WeekPage()
        {
            this.InitializeComponent();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is DateTime time)
            {
                DayOfWeek dayOfWeek = DayOfWeek.Sunday;
                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values["StartDay"] is string startDay)
                {
                    if (startDay == "Monday")
                    {
                        dayOfWeek = DayOfWeek.Monday;
                    }
                }


                weekGridView.ItemsSource = Helper.GetWeeks(time, dayOfWeek);
            }

        }
    }
}
