using System.Collections.Generic;
using BGTG.POS.CalendarPlanTool;
using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;

namespace BGTG.Web.Infrastructure.Services.POSServices.Base
{
    public interface ICalendarPlanService : ISavable
    {
        CalendarPlanCreateViewModel GetCalendarPlanCreateViewModel(CalendarPlanPreCreateViewModel viewModel);
        CalendarPlan Write(CalendarPlanCreateViewModel viewModel);
        CalendarPlan Write(CalendarPlan calendarPlan);
        IEnumerable<decimal> GetTotalPercentages(CalendarPlanCreateViewModel viewModel);
    }
}