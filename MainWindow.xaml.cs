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
using System.Linq;
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

            GetSubscriptions();
        }

        public async void GetSubscriptions()
        {      
            List<StorageFile> files = await iCalendarHelper.GetLocalFolderFilesAsync();
            foreach (StorageFile file in files)
            {
                Subscription subscription = new Subscription
                {
                    Name = file.Name.Split(".").FirstOrDefault(),
                };
                Subscriptions.Add(subscription);
            }

            subscriptionListView.ItemsSource = Subscriptions;
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

        private async void navigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
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
                    else if (navigationViewItem.Tag.Equals("Add"))
                    {
                        ContentDialog dialog = new ContentDialog();

                        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                        dialog.XamlRoot = this.Content.XamlRoot;
                        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                        dialog.Title = "Add Subscription";
                        dialog.PrimaryButtonText = "Add";
                        dialog.CloseButtonText = "Cancel";
                        dialog.DefaultButton = ContentDialogButton.Primary;
                        dialog.Content = new AddSubscriptionDialogControl();

                        var result = await dialog.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                        {
                            // Handle the addition of the subscription here
                            // You can access the dialog's content and retrieve the necessary data
                            var addSubscriptionControl = dialog.Content as AddSubscriptionDialogControl;
                            if (addSubscriptionControl != null)
                            {
                                // Example: addSubscriptionControl.SubscriptionName
                                string url = addSubscriptionControl.SubscriptionUrl;
                                if (!string.IsNullOrEmpty(url))
                                {
                                    string fileName = iCalendarHelper.DownloadAndSaveFileAsync(url).Result;
                                    if (!string.IsNullOrEmpty(fileName))
                                    {
                                        // Successfully downloaded and saved the file
                                        // You can create a new Subscription object and add it to your list or database
                                        Subscription newSubscription = new Subscription
                                        {
                                            Name = fileName,
                                            Url = url // Assuming you have a Url property in your Subscription model
                                        };
                                        Subscriptions.Add(newSubscription);

                                        iCalendarHelper.AddCalendar(fileName);
                                    }
                                    else
                                    {
                                        // Handle the case where the file could not be saved
                                        // Show an error message or take appropriate action
                                    }
                                }

                            }
                        }

                        navigationView.SelectedItem = navigationView.MenuItems.FirstOrDefault();
                    }
                    else if (navigationViewItem.Tag.Equals("Sync"))
                    {

                        navigationView.SelectedItem = navigationView.MenuItems.FirstOrDefault();
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
                else if ("Account".Equals(args.InvokedItemContainer.Tag))
                {
                    NavView_Navigate(typeof(AccountPage), args.RecommendedNavigationTransitionInfo);
                }
            }
        }

        private void NavView_Navigate(
    Type navPageType,
    NavigationTransitionInfo transitionInfo)
        {
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            Type preNavPageType = contentFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (navPageType is not null && !Type.Equals(preNavPageType, navPageType))
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
