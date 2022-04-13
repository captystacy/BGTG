using Calabonga.OperationResults;
using POS.Infrastructure.Calculators.Base;
using POS.Models.CalendarPlanModels;
using POS.Models.EstimateModels;

namespace POS.Infrastructure.Calculators
{
    public class CalendarWorkCalculator : ICalendarWorkCalculator
    {
        private readonly IConstructionMonthsCalculator _constructionPeriodCalculator;

        public CalendarWorkCalculator(IConstructionMonthsCalculator constructionPeriodCalculator)
        {
            _constructionPeriodCalculator = constructionPeriodCalculator;
        }

        public Task<OperationResult<CalendarWork>> Calculate(EstimateWork estimateWork, DateTime constructionStartDate)
        {
            var operation = OperationResult.CreateResult<CalendarWork>();

            var totalCostIncludingCAIW = estimateWork.TotalCost - estimateWork.EquipmentCost - estimateWork.OtherProductsCost;

            if (totalCostIncludingCAIW < 0)
            {
                operation.AddError($"Total cost including CAIW was less than zero on estimate work {estimateWork}");
                return Task.FromResult(operation);
            }

            return Calculate(estimateWork.WorkName, estimateWork.TotalCost, totalCostIncludingCAIW,
                constructionStartDate, estimateWork.Percentages, estimateWork.Chapter);
        }

        public async Task<OperationResult<CalendarWork>> Calculate(string workName, decimal totalCost, decimal totalCostIncludingCAIW, DateTime constructionStartDate, IReadOnlyList<decimal> percentages, int estimateChapter)
        {
            var operation = OperationResult.CreateResult<CalendarWork>();

            if (string.IsNullOrEmpty(workName))
            {
                operation.AddError("Work name can't be empty");
                return operation;
            }

            if (totalCost < 0)
            {
                operation.AddError("Total cost can't be less than zero");
                return operation;
            }

            if (totalCostIncludingCAIW < 0)
            {
                operation.AddError("Total cost including CAIW can't be less than zero");
                return operation;
            }

            if (constructionStartDate == default)
            {
                operation.AddError("Construction start date can't have default value");
                return operation;
            }

            if (estimateChapter < 0)
            {
                operation.AddError("Estimate chapter can't be less than zero");
                return operation;
            }

            if (estimateChapter == 10 || estimateChapter == 11 || estimateChapter == 12)
            {
                operation.Result = new CalendarWork
                {
                    WorkName = workName,
                    TotalCost = totalCost,
                    TotalCostIncludingCAIW = totalCostIncludingCAIW,
                    EstimateChapter = estimateChapter,
                };

                return operation;
            }

            if (percentages.Count == 0)
            {
                operation.AddError("Percentages were not set");
                return operation;
            }

            var calculateOperation = await _constructionPeriodCalculator.Calculate(totalCost, totalCostIncludingCAIW, constructionStartDate, percentages);

            if (!calculateOperation.Ok)
            {
                operation.AddError(calculateOperation.GetMetadataMessages());
                return operation;
            }

            operation.Result = new CalendarWork
            {
                WorkName = workName,
                TotalCost = totalCost,
                TotalCostIncludingCAIW = totalCostIncludingCAIW,
                ConstructionMonths = calculateOperation.Result,
                EstimateChapter = estimateChapter,
            };

            return operation;
        }
    }
}