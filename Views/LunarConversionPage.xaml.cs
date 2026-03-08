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
using tyme.lunar;
using tyme.solar;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CalendarWinUI3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LunarConversionPage : Page
    {
        public LunarConversionPage()
        {
            InitializeComponent();
            this.Loaded += LunarConversionPage_Loaded;
            
        }

        private void LunarConversionPage_Loaded(object sender, RoutedEventArgs e)
        {
            //LunarPicker.CalendarIdentifier = CalendarIdentifiers.ChineseLunar;
        }

        private void GregorianPicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            var date = e.NewDate;
            SolarDay solarDay = SolarDay.FromYmd(date.Year, date.Month, date.Day);
            LunarDay lunarDay = solarDay.GetLunarDay();
            LunarPicker.Date = new DateTime(lunarDay.Year, lunarDay.Month, lunarDay.Day);
        }

        private void LunarPicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            LunarDay lunarDay = LunarDay.FromYmd((int)e.NewDate.Year, (int)e.NewDate.Month, (int)e.NewDate.Day);
            SolarDay solarDay = lunarDay.GetSolarDay();
            GregorianPicker.Date = new DateTime(solarDay.Year, solarDay.Month, solarDay.Day);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
