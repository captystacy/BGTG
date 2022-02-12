using System.Collections.Generic;
using BGTG.POS.CalendarPlanTool;
using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;

namespace BGTG.Web.Infrastructure.Services.POSServices.Interfaces
{
    public interface ICalendarPlanService : ISavable
    {
        CalendarPlanCreateViewModel GetCalendarPlanCreateViewModel(CalendarPlanPreCreateViewModel viewModel);
        CalendarPlan Write(CalendarPlanCreateViewModel viewModel, string identityName);
        CalendarPlan Write(CalendarPlan calendarPlan, string identityName);
        IEnumerable<decimal> GetTotalPercentages(CalendarPlanCreateViewModel viewModel);
    }
}