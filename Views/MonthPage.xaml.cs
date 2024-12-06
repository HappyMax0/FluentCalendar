using CalendarWinUI3.Models;
using CalendarWinUI3.Models.Utils;
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
using Windows.Globalization;
using DayOfWeek = System.DayOfWeek;

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
            this.SizeChanged += MonthPage_SizeChanged;
        }

        private void MonthPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ItemsWrapGrid weekGridViewItemsWrapGrid = Helper.FindVisualChild<ItemsWrapGrid>(weekGridView);
            if (weekGridViewItemsWrapGrid != null)
            {
                weekGridViewItemsWrapGrid.ItemWidth = weekGridView.ActualWidth / 7f;
                //weekGridViewItemsWrapGrid.ItemHeight = 30f;
            }

            ItemsWrapGrid monthGridViewItemsWrapGrid = Helper.FindVisualChild<ItemsWrapGrid>(monthGridView);
            if (monthGridViewItemsWrapGrid != null)
            {
                monthGridViewItemsWrapGrid.ItemWidth = this.ActualWidth / 7f;
                monthGridViewItemsWrapGrid.ItemHeight = monthGridView.ActualHeight / 5f;
            }
       
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Calendar calendar)
            {
                
                //weekGridView.ItemsSource = Enum.GetValues(typeof(System.DayOfWeek)).Cast<System.DayOfWeek>().ToList();

                weekGridView.ItemsSource = Helper.GetWeeks(calendar);

                monthGridView.ItemsSource = Helper.GetDayList(calendar);
            }
              
        }

       
        
    }
}
