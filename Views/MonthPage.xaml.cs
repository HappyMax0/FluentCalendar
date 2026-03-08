using CalendarWinUI3.Models;
using CalendarWinUI3.Models.Utils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using tyme.culture;
using tyme.lunar;
using tyme.sixtycycle;
using tyme.solar;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CalendarWinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MonthPage : Page
    {
        public MonthPage()
        {
            this.InitializeComponent();
            
            monthGridView.SelectionChanged += MonthGridView_SelectionChanged;
         
            this.SizeChanged += MonthPage_SizeChanged;
        }

        private void MonthPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ItemsWrapGrid weekGridViewItemsWrapGrid = Helper.FindVisualChild<ItemsWrapGrid>(weekGridView);
            if (weekGridViewItemsWrapGrid != null)
            {
                weekGridViewItemsWrapGrid.ItemWidth = weekGridView.ActualWidth / 7.5f;
                //weekGridViewItemsWrapGrid.ItemHeight = 30f;
            }

            //monthGridView.ItemContainerStyle?.Setters.Add(new Setter(WidthProperty, monthGridView.ActualWidth / 7f));
            //monthGridView.ItemContainerStyle?.Setters.Add(new Setter(HeightProperty, monthGridView.ActualHeight / 6f));

            ItemsWrapGrid monthGridViewItemsWrapGrid = Helper.FindVisualChild<ItemsWrapGrid>(monthGridView);
            if (monthGridViewItemsWrapGrid != null)
            {
                monthGridViewItemsWrapGrid.ItemWidth = monthGridView.ActualWidth / 7.5f;
                //monthGridViewItemsWrapGrid.ItemHeight = monthGridView.ActualHeight / 6f;
            }
        }

        private async void MonthGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is GridView gridView) 
            {
                var selectedDay = gridView.SelectedValue as Day;
                //eventListView.ItemsSource = selectedDay.EventList;
                if (selectedDay.EventList.Count > 0)
                    holidayNameTB.Text = selectedDay.EventList.FirstOrDefault().Summary;
                else
                    holidayNameTB.Text = string.Empty;

                //公历
                SolarDay solarDay = SolarDay.FromYmd(selectedDay.YearNo, selectedDay.MonthNo, selectedDay.DayNo);
                LunarDay lunarDay = solarDay.GetLunarDay();
               
                // 宜：嫁娶, 祭祀, 理发, 作灶, 修饰垣墙, 平治道涂, 整手足甲, 沐浴, 冠笄
                List<Taboo> recommends = lunarDay.Recommends;
                // 忌：破土, 出行, 栽种
                List<Taboo> avoids = lunarDay.Avoids;

                lunarTB.Text = lunarDay.GetName();
                ganzhiTB.Text = lunarDay.SixtyCycle.GetName();       
                jieqiTB.Text = lunarDay.GetSolarDay().Term.GetName();
                recommendsTB.Text = string.Join("、", recommends.Select(x => x.GetName())).TrimEnd('、');
                avoidsTB.Text = string.Join("、", avoids.Select(x => x.GetName())).TrimEnd('、');

            }
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is DateTime time)
            {
                //weekGridView.ItemsSource = Enum.GetValues(typeof(System.DayOfWeek)).Cast<System.DayOfWeek>().ToList();

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

                weekGridView.ItemsSource = Helper.GetWeeks(time, dayOfWeek);

                var dayList = Helper.GetDayList(time, dayOfWeek, isShowWeekNo);
                monthGridView.ItemsSource = dayList;

                var selectedDay = dayList.FirstOrDefault(it => it.YearNo == time.Year && it.MonthNo == time.Month && it.DayNo == time.Day);
                monthGridView.SelectedItem = selectedDay;
            }
              
        }

        private void monthGridView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ItemsWrapGrid monthGridViewItemsWrapGrid = Helper.FindVisualChild<ItemsWrapGrid>(monthGridView);
            if (monthGridViewItemsWrapGrid != null)
            {
                monthGridViewItemsWrapGrid.ItemWidth = monthGridView.ActualWidth / 7.5f;
                //monthGridViewItemsWrapGrid.ItemHeight = monthGridView.ActualHeight / 6f;
            }
        }

        private void weekGridView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ItemsWrapGrid weekGridViewItemsWrapGrid = Helper.FindVisualChild<ItemsWrapGrid>(weekGridView);
            if (weekGridViewItemsWrapGrid != null)
            {
                weekGridViewItemsWrapGrid.ItemWidth = weekGridView.ActualWidth / 7.5f;
                //weekGridViewItemsWrapGrid.ItemHeight = 30f;
            }
        }
    }
}
