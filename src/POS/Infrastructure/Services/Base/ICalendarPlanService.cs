using POS.ViewModels;

namespace POS.Infrastructure.Services.Base;

public interface ICalendarPlanService
{
    CalendarPlanViewModel GetCalendarPlanViewModel(CalendarPlanCreateViewModel dto);
    MemoryStream Write(CalendarPlanViewModel dto);
    IEnumerable<decimal> GetTotalPercentages(CalendarPlanViewModel dto);
}