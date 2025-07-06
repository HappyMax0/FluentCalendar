using CalendarWinUI3.Views.Helpers;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Linq;
using Windows.Storage;
using Windows.System;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CalendarWinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public string Version
        {
            get
            {
                var version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
                return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
            }
        }

        private int lastNavigationSelectionMode = 0;

        public SettingsPage()
        {
            this.InitializeComponent();
            Loaded += OnSettingsPageLoaded;

            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values["ShowClockSeconds"] is bool showClockSeconds)
            {
                ShowClockSeconedSwitch.IsOn = showClockSeconds;
            }
            else
            {
                ShowClockSeconedSwitch.IsOn = true; // default value
            }

            if (localSettings.Values["StartDay"] is string startDay)
            {
                firstDayComboBox.SelectedItem = firstDayComboBox.Items.FirstOrDefault(item => ((ComboBoxItem)item).Tag.ToString() == startDay);
            }
            else
            {
                firstDayComboBox.SelectedIndex = 1; // default value
            }

            if (localSettings.Values["ShowWeekNo"] is bool showWeekNo)
            {
                ShowWeekNoSwitch.IsOn = showWeekNo;
            }
            else
            {
                ShowWeekNoSwitch.IsOn = false; // default value
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void OnSettingsPageLoaded(object sender, RoutedEventArgs e)
        {
            var currentTheme = ThemeHelper.RootTheme;
            switch (currentTheme)
            {
                case ElementTheme.Light:
                    themeMode.SelectedIndex = 0;
                    break;
                case ElementTheme.Dark:
                    themeMode.SelectedIndex = 1;
                    break;
                case ElementTheme.Default:
                    themeMode.SelectedIndex = 2;
                    break;
            }
        }

        private void themeMode_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedTheme = ((ComboBoxItem)themeMode.SelectedItem)?.Tag?.ToString();
            var window = WindowHelper.GetWindowForElement(this);
            string color;
            if (selectedTheme != null)
            {
                ThemeHelper.RootTheme = CommonHelper.GetEnum<ElementTheme>(selectedTheme);
                if (selectedTheme == "Dark")
                {
                    TitleBarHelper.SetCaptionButtonColors(window, Colors.White);
                    color = selectedTheme;
                }
                else if (selectedTheme == "Light")
                {
                    TitleBarHelper.SetCaptionButtonColors(window, Colors.Black);
                    color = selectedTheme;                   
                }
                else
                {
                    var uiSettings = new UISettings();
                    var backgroundColor = uiSettings.GetColorValue(UIColorType.Background);
                   
                    color = backgroundColor == Colors.White ? "Dark" : "Light";

                    TitleBarHelper.SetCaptionButtonColors(window, backgroundColor == Colors.White ? Colors.Black : Colors.White);
                }
                // announce visual change to automation
                UIHelper.AnnounceActionForAccessibility(sender as UIElement, $"Theme changed to {color}",
                                                                                "ThemeChangedNotificationActivityId");
            }
        }

        private async void bugRequestCard_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/HappyMax0/FluentCalendar/issues"));

        }

        private void ShowClockSeconedSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            if (sender is ToggleSwitch toggleSwitch)
            {
                localSettings.Values["ShowClockSeconds"] = toggleSwitch.IsOn;
            }
        }

        private void firstDayComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            
            if (sender is ComboBox comboBox)
            {
                if (comboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    localSettings.Values["StartDay"] = selectedItem.Tag.ToString();
                }                
            }
        }

        private void ShowWeekNoSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            if (sender is ToggleSwitch toggleSwitch)
            {
                localSettings.Values["ShowWeekNo"] = toggleSwitch.IsOn;
            }
        }
    }
}
