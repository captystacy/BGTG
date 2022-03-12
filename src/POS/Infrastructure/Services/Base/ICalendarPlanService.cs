using POS.ViewModels;

namespace POS.Infrastructure.Services.Base;

public interface ICalendarPlanService
{
    CalendarPlanViewModel GetCalendarPlanViewModel(CalendarPlanCreateViewModel viewModel);
    MemoryStream Write(CalendarPlanViewModel viewModel);
    IEnumerable<decimal> GetTotalPercentages(CalendarPlanViewModel viewModel);
}