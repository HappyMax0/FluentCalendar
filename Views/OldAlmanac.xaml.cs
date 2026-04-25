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

            var chineseDay = Helper.GetChineseDay(new DateTime(selectedDay.Year, selectedDay.Month, selectedDay.Day), DateTime.Today, System.DayOfWeek.Monday, false);
            ViewModel.SolarDate = $"{chineseDay.YearNo}年{chineseDay.MonthNo}月{chineseDay.DayNo}日";
            ViewModel.LunarDate = chineseDay.LunarDay;
            ViewModel.SixtyCycleDate = chineseDay.SixtyCycle;
            ViewModel.Recommends = chineseDay.Recommends;
            ViewModel.Avoids = chineseDay.Avoids;
            ViewModel.GoodGods = chineseDay.GoodGods;
            ViewModel.BadGods = chineseDay.BadGods;
            ViewModel.PengZu = chineseDay.PengZu;
            ViewModel.FetusDay = chineseDay.FetusDay;
            ViewModel.NineStar = chineseDay.NineStar;
        }
    }
}
