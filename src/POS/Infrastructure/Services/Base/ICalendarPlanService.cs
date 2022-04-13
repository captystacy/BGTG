using Calabonga.OperationResults;
using POS.ViewModels;

namespace POS.Infrastructure.Services.Base
{
    public interface ICalendarPlanService
    {
        Task<OperationResult<CalendarPlanViewModel>> GetCalendarPlanViewModel(CalendarPlanCreateViewModel viewModel);
        Task<OperationResult<IEnumerable<decimal>>> GetTotalPercentages(CalendarPlanViewModel viewModel);
        Task<OperationResult<MemoryStream>> GetCalendarPlanStream(CalendarPlanViewModel viewModel);
    }
}