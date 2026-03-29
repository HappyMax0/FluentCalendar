using CalendarWinUI3.Models;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CalendarWinUI3.Views.UserControls
{
    public sealed partial class ChineseAlmanac : UserControl
    {
        public Week ViewModel { get; } = new();
        public TextBlock _SolarDayTB => SolarDayTB;
        public TextBlock _SolarFestivalNameTB => SolarFestivalNameTB;
        public TextBlock _LunarFestivalNameTB => LunarFestivalNameTB;
        public TextBlock _lunarTB => lunarTB;
        public TextBlock _ganzhiTB => ganzhiTB;
        public TextBlock _jieqiTB => jieqiTB;
        public TextBlock _recommendsTB => recommendsTB;
        public TextBlock _avoidsTB => avoidsTB;

        public ChineseAlmanac()
        {
            InitializeComponent();
        }
    }
}
