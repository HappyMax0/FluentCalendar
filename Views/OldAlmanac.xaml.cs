using CalendarWinUI3.Models.Utils;
using CalendarWinUI3.ViewModels;
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

namespace CalendarWinUI3.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class OldAlmanac : Page
{
    public OldAlmanacViewModel ViewModel { get; set; }

    public OldAlmanac()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is OldAlmanacViewModel oldAlmanacViewModel)
        {
            var selectedDay = oldAlmanacViewModel.SelectedDay;

            ViewModel = oldAlmanacViewModel;

            var week = Helper.GetWeek(new DateTime(selectedDay.Year, selectedDay.Month, selectedDay.Day), DateTime.Today, System.DayOfWeek.Monday, false);
            ViewModel.SolarDate = $"{week.YearNo}年{week.MonthNo}月{week.DayNo}日";
            ViewModel.LunarDate = week.LunarDay;
            ViewModel.SixtyCycleDate = week.SixtyCycle;
            ViewModel.Recommends = week.Recommends;
            ViewModel.Avoids = week.Avoids;
            ViewModel.GoodGods = week.GoodGods;
            ViewModel.BadGods = week.BadGods;
            ViewModel.PengZu = week.PengZu;
            ViewModel.FetusDay = week.FetusDay;
            ViewModel.NineStar = week.NineStar;
        }
    }
}
