using CalendarWinUI3.Models;
using CalendarWinUI3.Models.Utils;
using CalendarWinUI3.Views;
using CalendarWinUI3.Views.Helpers;
using CalendarWinUI3.Views.Styles;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using Windows.Storage;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CalendarWinUI3
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private UISettings _settings;
        public Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue;

        public ObservableCollection<Subscription> Subscriptions { get; set; } = new ObservableCollection<Subscription>();

        public MainWindow()
        {
            this.InitializeComponent();

            dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();

            this.ExtendsContentIntoTitleBar = true;

            _settings = new UISettings();
            _settings.ColorValuesChanged += _settings_ColorValuesChanged; // cannot use FrameworkElement.ActualThemeChanged event because the triggerTitleBarRepaint workaround no longer works

            if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
            {
                TitleBarHelper.SetCaptionButtonColors(this, Colors.White);
            }
            else
            {
                TitleBarHelper.SetCaptionButtonColors(this, Colors.Black);
            }

            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (localSettings.Values["StartDay"] is string startDay)
            {
                calendarView.FirstDayOfWeek = startDay.Equals("Monday") ? Windows.Globalization.DayOfWeek.Monday : Windows.Globalization.DayOfWeek.Sunday;
            }
            else
            {
                calendarView.FirstDayOfWeek = Windows.Globalization.DayOfWeek.Sunday; // default value
            }
        }


        // this handles updating the caption button colors correctly when indows system theme is changed
        // while the app is open
        private void _settings_ColorValuesChanged(UISettings sender, object args)
        {
            // This calls comes off-thread, hence we will need to dispatch it to current app's thread
            dispatcherQueue.TryEnqueue(() =>
            {
                _ = TitleBarHelper.ApplySystemThemeToCaptionButtons(this);
            });
        }

        private void navigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                NavView_Navigate(typeof(SettingsPage), args.RecommendedNavigationTransitionInfo);
            }
            else
            {
                if (args.SelectedItem != null && args.SelectedItem is NavigationViewItem navigationViewItem && navigationViewItem.Tag != null)
                {
                    if (navigationViewItem.Tag.Equals("Calendar"))
                    {
                        NavView_Navigate(typeof(MainPage), args.RecommendedNavigationTransitionInfo);

                    }
                  
                }
            }
        }

        private void navigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked == true)
            {
                NavView_Navigate(typeof(SettingsPage), args.RecommendedNavigationTransitionInfo);
            }
            else if (args.InvokedItemContainer != null && args.InvokedItemContainer.Tag != null)
            {
                if("Calendar".Equals(args.InvokedItemContainer.Tag))
                {
                    NavView_Navigate(typeof(MainPage), args.RecommendedNavigationTransitionInfo);
                }
                else if ("Holidays".Equals(args.InvokedItemContainer.Tag))
                {
                    NavView_Navigate(typeof(HolidaysPage), args.RecommendedNavigationTransitionInfo);
                }
                else if ("OldHuangCalendar".Equals(args.InvokedItemContainer.Tag))
                {
                    NavView_Navigate(typeof(OldHuangCalendarPage), args.RecommendedNavigationTransitionInfo);
                }
            }
        }

        private void NavView_Navigate(
    Type navPageType,
    NavigationTransitionInfo transitionInfo)
        {
           
            // Only navigate if the selected page isn't currently loaded.
            if (navPageType is not null)
            {
                contentFrame.Navigate(navPageType, null, transitionInfo);
            }
        }

        private void navigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            TryGoBack();
        }

        private bool TryGoBack()
        {
            if (!contentFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed.
            if (navigationView.IsPaneOpen &&
                (navigationView.DisplayMode == NavigationViewDisplayMode.Compact ||
                 navigationView.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            contentFrame.GoBack();
            return true;
        }
       
    }
}
