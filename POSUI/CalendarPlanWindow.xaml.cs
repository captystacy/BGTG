using POSUI.Services.Interfaces;
using System.Windows;

namespace POSUI
{
    public partial class CalendarPlanWindow : Window
    {
        private readonly ICalendarPlanService _calendarPlanService;

        public CalendarPlanWindow(ICalendarPlanService calendarPlanService)
        {
            InitializeComponent();
            _calendarPlanService = calendarPlanService;
        }
    }
}
