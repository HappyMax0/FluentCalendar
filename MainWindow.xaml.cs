using CalendarWinUI3.Views;
using CalendarWinUI3.Views.Helpers;
using Microsoft.UI;
using Microsoft.UI.Composition;
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
                if (contentFrame != null)
                {
                    contentFrame.Navigate(typeof(SettingsPage));
                }
            }
            else
            {
                if (args.SelectedItem != null)
                {
                    if (args.SelectedItem is NavigationViewItem navigationViewItem)
                    {
                        if (navigationViewItem.Tag.Equals("Calendar"))
                        {
                            if (contentFrame != null)
                            {
                                contentFrame.Navigate(typeof(MainPage));
                            }
                        }
                    }
                }
            }
        }
    }
}
