using POS.ViewModels;
using Xceed.Words.NET;

namespace POS.Infrastructure.Services.Base;

public interface ICalendarPlanService
{
    CalendarPlanViewModel GetCalendarPlanViewModel(CalendarPlanCreateViewModel viewModel);
    FileStream Write(CalendarPlanViewModel viewModel);
    IEnumerable<decimal> GetTotalPercentages(CalendarPlanViewModel viewModel);
}