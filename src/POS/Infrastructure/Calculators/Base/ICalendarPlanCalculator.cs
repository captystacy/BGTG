using Calabonga.OperationResults;
using POS.Models.CalendarPlanModels;
using POS.Models.EstimateModels;

namespace POS.Infrastructure.Calculators.Base
{
    public interface ICalendarPlanCalculator
    {
        Task<OperationResult<CalendarPlan>> CalculatePreparatory(Estimate estimate, IReadOnlyList<decimal> preparatoryPercentages, IReadOnlyList<decimal> temporaryBuildingsPercentages);
        Task<OperationResult<CalendarPlan>> CalculateMain(Estimate estimate, CalendarWork totalPreparatoryWork,
            IReadOnlyList<decimal> otherExpensesPercentages);
    }
}