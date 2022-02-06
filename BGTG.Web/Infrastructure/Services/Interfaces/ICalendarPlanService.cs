using System.Collections.Generic;
using BGTG.POS.CalendarPlanTool;
using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;

namespace BGTG.Web.Infrastructure.Services.Interfaces
{
    public interface ICalendarPlanService : ISavable
    {
        CalendarPlanCreateViewModel GetCalendarPlanCreateViewModel(CalendarPlanPreCreateViewModel viewModel);
        CalendarPlan Write(CalendarPlanCreateViewModel viewModel, string windowsName);
        CalendarPlan Write(CalendarPlan calendarPlan, string windowsName);
        IEnumerable<decimal> GetTotalPercentages(CalendarPlanCreateViewModel viewModel);
    }
}