using CalendarWinUI3.Models;
using CalendarWinUI3.Models.Utils;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

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

        private void MonthGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is GridView gridView) 
            {
                var selectedDay = gridView.SelectedValue as Day;
                eventListView.ItemsSource = selectedDay.EventList;
            }
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is DateTime time)
            {               
                //weekGridView.ItemsSource = Enum.GetValues(typeof(System.DayOfWeek)).Cast<System.DayOfWeek>().ToList();

                weekGridView.ItemsSource = Helper.GetWeeks(time);

                monthGridView.ItemsSource = Helper.GetDayList(time);

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
