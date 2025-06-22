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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CalendarWinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AccountPage : Page
    {
        public ObservableCollection<Subscription> Subscriptions { get; set; } = new ObservableCollection<Subscription>();

        public AccountPage()
        {
            InitializeComponent();

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

            SubscriptionSettingsExpander.ItemsSource = Subscriptions;
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var name = (button.CommandParameter as String);
            
            iCalendarHelper.DeleteSubscription(name);

            var calendar = Subscriptions.FirstOrDefault(c => c.Name == name);
            if (calendar != null)
            {
                Subscriptions.Remove(calendar);
            }
        }

        private void syncBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
