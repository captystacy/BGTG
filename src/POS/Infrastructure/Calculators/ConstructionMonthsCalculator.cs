using Calabonga.OperationResults;
using POS.Infrastructure.Calculators.Base;
using POS.Models.CalendarPlanModels;

namespace POS.Infrastructure.Calculators
{
    public class ConstructionMonthsCalculator : IConstructionMonthsCalculator
    {
        public Task<OperationResult<IEnumerable<ConstructionMonth>>> Calculate(decimal totalCost, decimal totalCostIncludingCAIW, DateTime constructionStartDate, IReadOnlyList<decimal> percentages)
        {
            var operation = OperationResult.CreateResult<IEnumerable<ConstructionMonth>>();

            var constructionMonths = new List<ConstructionMonth>();

            for (int i = 0; i < percentages.Count; i++)
            {
                if (percentages[i] == 0)
                {
                    continue;
                }

                if (percentages[i] < 0 || percentages[i] > 1)
                {
                    operation.AddError(
                        $"Percentage has incorrect value of {percentages[i]}. {nameof(totalCost)}: {totalCost}, " +
                        $"{nameof(totalCostIncludingCAIW)}: {totalCostIncludingCAIW}, {nameof(constructionStartDate)}: {constructionStartDate}, " +
                        $"{nameof(percentages)}: {string.Join(',', percentages)}");
                    return Task.FromResult(operation);
                }

                var date = constructionStartDate.AddMonths(i);
                var investmentVolume = totalCost * percentages[i];
                var volumeCAIW = totalCostIncludingCAIW * percentages[i];
                var constructionMonth = new ConstructionMonth
                {
                    VolumeCAIW = volumeCAIW,
                    CreationIndex = i,
                    Date = date,
                    InvestmentVolume = investmentVolume,
                    PercentPart = percentages[i]
                };

                constructionMonths.Add(constructionMonth);
            }

            operation.Result = constructionMonths;

            return Task.FromResult(operation);
        }
    }
}