using Calabonga.OperationResults;
using POS.Models.CalendarPlanModels;

namespace POS.Infrastructure.Calculators.Base
{
    public interface IConstructionMonthsCalculator
    {
        Task<OperationResult<IEnumerable<ConstructionMonth>>> Calculate(decimal totalCost, decimal totalCostIncludingCAIW, DateTime constructionStartDate, IReadOnlyList<decimal> percentages);
    }
}