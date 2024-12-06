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
using System.Reflection;
using Windows.Globalization;
using Microsoft.UI.Xaml.Media.Animation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CalendarWinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Calendar Time { get; set; } = new();

        public MainPage()
        {
            this.InitializeComponent();
            //Time = new Calendar();
            Time.SetDateTime(DateTime.Now);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            YearTB.Text = Time.GetDateTime().ToString("yyyy/MM");

            if (contentFrame != null)
                contentFrame.Navigate(typeof(MonthPage), Time);
        }

        private void YearRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            YearTB.Text = Time.GetDateTime().ToString("yyyy");

            if (contentFrame != null)
                contentFrame.Navigate(typeof(YearPage), Time);
        }

        private void MonthRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            YearTB.Text = Time.GetDateTime().ToString("yyyy/MM");

            if (contentFrame != null)
                contentFrame.Navigate(typeof(MonthPage), Time, new EntranceNavigationTransitionInfo());
        }

        private void WeekRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            YearTB.Text = Time.GetDateTime().ToString("yyyy/MM");

            if (contentFrame != null)
                contentFrame.Navigate(typeof(WeekPage), Time, new EntranceNavigationTransitionInfo());
        }

        private void DayRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            YearTB.Text = Time.GetDateTime().ToString("yyyy/MM/dd");

            if (contentFrame != null)
                contentFrame.Navigate(typeof(DayPage), Time, new EntranceNavigationTransitionInfo());
        }

        private void preBtn_Click(object sender, RoutedEventArgs e)
        {
            if(MonthRadioBtn != null)
            {
                if (MonthRadioBtn.IsChecked.HasValue)
                {
                    if(MonthRadioBtn.IsChecked.Value)
                    {
                        Time.AddMonths(-1);

                        YearTB.Text = Time.GetDateTime().ToString("yyyy/MM");

                        if (contentFrame != null)
                            contentFrame.Navigate(typeof(MonthPage), Time, new SlideNavigationTransitionInfo()
                            { Effect = SlideNavigationTransitionEffect.FromLeft });

                        return;
                    }               
                }
            }

            if (WeekRadioBtn != null)
            {
                if (WeekRadioBtn.IsChecked.HasValue)
                {
                    if (WeekRadioBtn.IsChecked.Value)
                    {
                        Time.AddDays(-7);

                        YearTB.Text = Time.GetDateTime().ToString("yyyy/MM");

                        if (contentFrame != null)
                            contentFrame.Navigate(typeof(WeekPage), Time, new SlideNavigationTransitionInfo()
                            { Effect = SlideNavigationTransitionEffect.FromLeft });

                        return;
                    }
                }
            }

            if (DayRadioBtn != null)
            {
                if (DayRadioBtn.IsChecked.HasValue)
                {
                    if (DayRadioBtn.IsChecked.Value)
                    {
                        Time.AddDays(-1);

                        YearTB.Text = Time.GetDateTime().ToString("yyyy/MM/dd");

                        if (contentFrame != null)
                            contentFrame.Navigate(typeof(DayPage), Time, new SlideNavigationTransitionInfo()
                            { Effect = SlideNavigationTransitionEffect.FromLeft });

                        return;
                    }
                }
            }
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MonthRadioBtn != null)
            {
                if (MonthRadioBtn.IsChecked.HasValue)
                {
                    if (MonthRadioBtn.IsChecked.Value)
                    {
                        Time.AddMonths(1);

                        YearTB.Text = Time.GetDateTime().ToString("yyyy/MM");

                        if (contentFrame != null)
                            contentFrame.Navigate(typeof(MonthPage), Time, new SlideNavigationTransitionInfo()
                            { Effect = SlideNavigationTransitionEffect.FromRight });

                        return;
                    }
                }
            }

            if (WeekRadioBtn != null)
            {
                if (WeekRadioBtn.IsChecked.HasValue)
                {
                    if (WeekRadioBtn.IsChecked.Value)
                    {
                        Time.AddDays(7);

                        YearTB.Text = Time.GetDateTime().ToString("yyyy/MM");

                        if (contentFrame != null)
                            contentFrame.Navigate(typeof(WeekPage), Time, new SlideNavigationTransitionInfo()
                            { Effect = SlideNavigationTransitionEffect.FromRight });

                        return;
                    }
                }
            }

            if (DayRadioBtn != null)
            {
                if (DayRadioBtn.IsChecked.HasValue)
                {
                    if (DayRadioBtn.IsChecked.Value)
                    {
                        Time.AddDays(1);

                        YearTB.Text = Time.GetDateTime().ToString("yyyy/MM/dd");

                        if (contentFrame != null)
                            contentFrame.Navigate(typeof(DayPage), Time, new SlideNavigationTransitionInfo()
                            { Effect = SlideNavigationTransitionEffect.FromRight });

                        return;
                    }
                }
            }
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            Time = new Calendar();
            Time.SetDateTime(DateTime.Now);

            if (MonthRadioBtn != null)
            {
                if (MonthRadioBtn.IsChecked.HasValue)
                {
                    if (MonthRadioBtn.IsChecked.Value)
                    {
                        YearTB.Text = Time.GetDateTime().ToString("yyyy/MM");

                        if (contentFrame != null)
                            contentFrame.Navigate(typeof(MonthPage), Time, new DrillInNavigationTransitionInfo());

                        return;
                    }
                }
            }

            if (WeekRadioBtn != null)
            {
                if (WeekRadioBtn.IsChecked.HasValue)
                {
                    if (WeekRadioBtn.IsChecked.Value)
                    {
                        YearTB.Text = Time.GetDateTime().ToString("yyyy/MM");

                        if (contentFrame != null)
                            contentFrame.Navigate(typeof(WeekPage), Time, new DrillInNavigationTransitionInfo());

                        return;
                    }
                }
            }

            if (DayRadioBtn != null)
            {
                if (DayRadioBtn.IsChecked.HasValue)
                {
                    if (DayRadioBtn.IsChecked.Value)
                    {
                        YearTB.Text = Time.GetDateTime().ToString("yyyy/MM/dd");

                        if (contentFrame != null)
                            contentFrame.Navigate(typeof(DayPage), Time, new DrillInNavigationTransitionInfo());

                        return;
                    }
                }
            }
        }
    }
}
