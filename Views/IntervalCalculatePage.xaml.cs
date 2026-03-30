using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CalendarWinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IntervalCalculatePage : Page
    {
        public IntervalCalculatePage()
        {
            InitializeComponent();
        }

        private async void CalculateBtn_Click(object sender, RoutedEventArgs e)
        {
            var timeSpan = datePicker2.Date - datePicker1.Date;

            var loader = new ResourceLoader();
            string dayText = loader.GetString("Interval_Days");

            string interval = $"{timeSpan.Days} {dayText}";

            var dialog = new ContentDialog
            {
                Title = loader.GetString("Interval_Title"),
                Content = interval,
                PrimaryButtonText = loader.GetString("Interval_OK"),
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}
