using Calabonga.OperationResults;
using POS.Models.CalendarPlanModels;
using POS.Models.EstimateModels;

namespace POS.Infrastructure.Calculators.Base
{
    public interface ICalendarWorkCalculator
    {
        Task<OperationResult<CalendarWork>> Calculate(EstimateWork estimateWork, DateTime constructionStartDate);
        Task<OperationResult<CalendarWork>> Calculate(string workName, decimal totalCost, decimal totalCostIncludingCAIW, DateTime constructionStartDate, IReadOnlyList<decimal> percentages, int estimateChapter);
    }
}