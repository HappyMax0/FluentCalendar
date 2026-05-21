using CalendarWinUI3.Models;
using CalendarWinUI3.Models.Utils;
using CalendarWinUI3.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using tyme.culture;
using tyme.festival;
using tyme.lunar;
using tyme.sixtycycle;
using tyme.solar;
using Windows.Storage;
using static CommunityToolkit.WinUI.Controls.GridSplitter;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CalendarWinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MonthPage : Page
    {
        private MainViewModel viewModel;

        DayOfWeek dayOfWeek = DayOfWeek.Sunday;
        bool isShowWeekNo = false;

        public MonthPage()
        {
            this.InitializeComponent();
            
            monthGridView.SelectionChanged += MonthGridView_SelectionChanged;
         
            this.SizeChanged += MonthPage_SizeChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is MainViewModel mainViewModel)
            {
                viewModel = mainViewModel;

                var time = viewModel.SelectedDay;

                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

                if (localSettings.Values["StartDay"] is string startDay)
                {
                    if (startDay == "Monday")
                    {
                        dayOfWeek = DayOfWeek.Monday;
                    }
                }

                isShowWeekNo = localSettings.Values["ShowWeekNo"] is bool;

                weekGridView.ItemsSource = Helper.GetChineseDays(time, dayOfWeek, isShowWeekNo);

                var dayList = Helper.GetDayList(time, dayOfWeek, isShowWeekNo);
                monthGridView.ItemsSource = dayList;

                var selectedDay = dayList.FirstOrDefault(it => it.YearNo == time.Year && it.MonthNo == time.Month && it.DayNo == time.Day);
                monthGridView.SelectedItem = selectedDay;
            }
        }

        private async void MonthGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is GridView gridView)
            {
                var selectedDay = gridView.SelectedValue as Day;
                var chineseDay = Helper.GetChineseDay(new DateTime(selectedDay.YearNo, selectedDay.MonthNo, selectedDay.DayNo), DateTime.Today, dayOfWeek, isShowWeekNo);
                ChineseAlmanacControl.DataContext = chineseDay;

                viewModel.IsUpdatingDateFromCode = true;

                viewModel.SelectedDay = new DateTimeOffset(selectedDay.YearNo, selectedDay.MonthNo, selectedDay.DayNo, 0, 0, 0, TimeSpan.Zero);

                viewModel.IsUpdatingDateFromCode = false;
            }
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

            // Ensure layout switches correctly on resize (fallback to code-behind if VisualState triggers don't apply)
            if (this.ActualWidth < 720)
            {
                // Narrow layout: splitter below monthGridView, ChineseAlmanac below splitter
                Grid.SetRow(GridSplitter, 2);
                Grid.SetColumn(GridSplitter, 0);
                Grid.SetRowSpan(GridSplitter, 1);
                Grid.SetColumnSpan(GridSplitter, 3);
                GridSplitter.ResizeDirection = GridResizeDirection.Rows;
                GridSplitter.Height = 8;
                GridSplitter.VerticalAlignment = VerticalAlignment.Center;
                GridSplitter.HorizontalAlignment = HorizontalAlignment.Stretch;
                GridSplitter.ResizeBehavior = GridResizeBehavior.PreviousAndNext;

                // Make monthGridView row and ChineseAlmanac row resizable (star sizing)
                if (rootGrid.RowDefinitions.Count > 3)
                {
                    rootGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                    rootGrid.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);
                }

                Grid.SetRow(ChineseAlmanacControl, 3);
                Grid.SetColumn(ChineseAlmanacControl, 0);
                Grid.SetRowSpan(ChineseAlmanacControl, 1);
                Grid.SetColumnSpan(ChineseAlmanacControl, 1);
                // show horizontal splitter, hide vertical splitter
                if (HorizontalSplitter != null)
                {
                    HorizontalSplitter.Visibility = Visibility.Visible;
                    HorizontalSplitter.IsEnabled = true;
                    Canvas.SetZIndex(HorizontalSplitter, 100);
                }
                if (GridSplitter != null)
                {
                    GridSplitter.Visibility = Visibility.Collapsed;
                    GridSplitter.IsEnabled = false;
                }
            }
            else
            {
                // Wide layout: splitter as vertical divider, almanac on the right
                Grid.SetRow(GridSplitter, 0);
                Grid.SetColumn(GridSplitter, 1);
                Grid.SetRowSpan(GridSplitter, 4);
                Grid.SetColumnSpan(GridSplitter, 1);
                GridSplitter.ResizeDirection = GridResizeDirection.Columns;
                GridSplitter.Height = double.NaN;
                GridSplitter.VerticalAlignment = VerticalAlignment.Stretch;
                GridSplitter.HorizontalAlignment = HorizontalAlignment.Center;
                GridSplitter.ResizeBehavior = GridResizeBehavior.PreviousAndNext;

                // Restore original row sizing: month row auto, bottom row star
                if (rootGrid.RowDefinitions.Count > 3)
                {
                    rootGrid.RowDefinitions[1].Height = GridLength.Auto;
                    rootGrid.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);
                }

                Grid.SetRow(ChineseAlmanacControl, 0);
                Grid.SetColumn(ChineseAlmanacControl, 2);
                Grid.SetRowSpan(ChineseAlmanacControl, 4);
                Grid.SetColumnSpan(ChineseAlmanacControl, 1);
                // restore ChineseAlmanac width in wide mode (use explicit width from column)
                ChineseAlmanacControl.Width = double.NaN;
                // show vertical splitter, hide horizontal splitter
                if (GridSplitter != null)
                {
                    GridSplitter.Visibility = Visibility.Visible;
                    GridSplitter.IsEnabled = true;
                    Canvas.SetZIndex(GridSplitter, 100);
                }
                if (HorizontalSplitter != null)
                {
                    HorizontalSplitter.Visibility = Visibility.Collapsed;
                    HorizontalSplitter.IsEnabled = false;
                }
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
