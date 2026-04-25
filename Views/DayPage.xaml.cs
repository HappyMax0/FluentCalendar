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
using CalendarWinUI3.Models;
using CalendarWinUI3.Models.Utils;
using Windows.Globalization;
using CalendarWinUI3.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CalendarWinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DayPage : Page
    {
        private MainViewModel viewModel;

        public DayPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if(e.Parameter is MainViewModel mainViewModel)
            {
                viewModel = mainViewModel;
                var selectedDay = mainViewModel.SelectedDay;
                var chineseDay = Helper.GetChineseDay(new DateTime(selectedDay.Year, selectedDay.Month, selectedDay.Day), DateTime.Today, System.DayOfWeek.Monday, false);
                ChineseAlmanacControl.DataContext = chineseDay;
            }
            
        }

        
    }
}
