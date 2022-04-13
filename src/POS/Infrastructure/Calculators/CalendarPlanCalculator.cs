using Calabonga.OperationResults;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Calculators.Base;
using POS.Models.CalendarPlanModels;
using POS.Models.EstimateModels;

namespace POS.Infrastructure.Calculators
{
    public class CalendarPlanCalculator : ICalendarPlanCalculator
    {
        private readonly ICalendarWorkCalculator _calendarWorkCalculator;

        public CalendarPlanCalculator(ICalendarWorkCalculator calendarWorkCalculator)
        {
            _calendarWorkCalculator = calendarWorkCalculator;
        }

        public async Task<OperationResult<CalendarPlan>> CalculatePreparatory(Estimate estimate, IReadOnlyList<decimal> preparatoryPercentages, IReadOnlyList<decimal> temporaryBuildingsPercentages)
        {
            var operation = OperationResult.CreateResult<CalendarPlan>();

            var calculateOperationsTasks = estimate.PreparatoryEstimateWorks
                .Select(estimateWork => _calendarWorkCalculator.Calculate(estimateWork, estimate.ConstructionStartDate))
                .ToList();

            var calculateOperations = (await Task.WhenAll(calculateOperationsTasks)).ToList();

            if (calculateOperations.Exists(x => !x.Ok))
            {
                var errors = calculateOperations.Select(x => x.GetMetadataMessages()).ToList();
                operation.AddError(string.Join('\n', errors));
                return operation;
            }

            var calendarWorks = calculateOperations.Select(x => x.Result).ToList();

            // add preparatory work

            var preparatoryCalendarWorks = calendarWorks.FindAll(x => x.EstimateChapter == Constants.PreparatoryWorkChapter);
            calendarWorks.RemoveAll(x => x.EstimateChapter == Constants.PreparatoryWorkChapter);

            if (preparatoryCalendarWorks.Count != 0)
            {
                var preparatoryTotalCost = preparatoryCalendarWorks.Sum(x => x.TotalCost);
                var preparatoryTotalCostIncludingCAIW = preparatoryCalendarWorks.Sum(x => x.TotalCostIncludingCAIW);
                var preparatoryWorkOperation = await _calendarWorkCalculator.Calculate(Constants.PreparatoryWorkName,
                    preparatoryTotalCost, preparatoryTotalCostIncludingCAIW, estimate.ConstructionStartDate,
                    preparatoryPercentages, Constants.PreparatoryWorkChapter);

                if (!preparatoryWorkOperation.Ok)
                {
                    operation.AddError(preparatoryWorkOperation.GetMetadataMessages());
                    return operation;
                }

                var preparatoryWork = preparatoryWorkOperation.Result;
                if (preparatoryWork.TotalCostIncludingCAIW > 0)
                {
                    calendarWorks.Add(preparatoryWork);
                }
            }

            // add temporary buildings work

            var temporaryBuildingsWorks = calendarWorks.FindAll(x => x.EstimateChapter == Constants.PreparatoryTemporaryBuildingsWorkChapter);
            calendarWorks.RemoveAll(x => x.EstimateChapter == Constants.PreparatoryTemporaryBuildingsWorkChapter);

            var temporaryBuildingsTotalCost = temporaryBuildingsWorks.Sum(x => x.TotalCost);
            var temporaryBuildingsTotalCostIncludingCAIW = temporaryBuildingsWorks.Sum(x => x.TotalCostIncludingCAIW);
            var temporaryBuildingsWorkOperation = await _calendarWorkCalculator.Calculate(
                Constants.PreparatoryTemporaryBuildingsWorkName, temporaryBuildingsTotalCost,
                temporaryBuildingsTotalCostIncludingCAIW, estimate.ConstructionStartDate,
                temporaryBuildingsPercentages, Constants.PreparatoryTemporaryBuildingsWorkChapter);

            if (!temporaryBuildingsWorkOperation.Ok)
            {
                operation.AddError(temporaryBuildingsWorkOperation.GetMetadataMessages());
                return operation;
            }

            calendarWorks.Add(temporaryBuildingsWorkOperation.Result);

            // add total work

            var totalWork = new CalendarWork
            {
                WorkName = Constants.TotalWorkName,
                TotalCost = calendarWorks.Sum(x => x.TotalCost),
                TotalCostIncludingCAIW = calendarWorks.Sum(x => x.TotalCostIncludingCAIW),
                ConstructionMonths = SumConstructionMonths(estimate.ConstructionDurationCeiling, calendarWorks),
                EstimateChapter = Constants.PreparatoryTotalWorkChapter
            };

            calendarWorks.Add(totalWork);

            operation.Result = new CalendarPlan
            {
                ConstructionStartDate = estimate.ConstructionStartDate,
                CalendarWorks = calendarWorks,
                ConstructionDuration = estimate.ConstructionDuration,
                ConstructionDurationCeiling = estimate.ConstructionDurationCeiling
            };

            return operation;
        }


        public async Task<OperationResult<CalendarPlan>> CalculateMain(Estimate estimate, CalendarWork totalPreparatoryWork,
            IReadOnlyList<decimal> otherExpensesPercentages)
        {
            var operation = OperationResult.CreateResult<CalendarPlan>();

            var calculateOperationsTasks = estimate.MainEstimateWorks
                .Select(estimateWork => _calendarWorkCalculator.Calculate(estimateWork, estimate.ConstructionStartDate))
                .ToList();

            var calculateOperations = (await Task.WhenAll(calculateOperationsTasks)).ToList();

            if (calculateOperations.Exists(x => !x.Ok))
            {
                var errors = calculateOperations.Select(x => x.GetMetadataMessages()).ToList();
                operation.AddError(string.Join('\n', errors));
                return operation;
            }

            var calendarWorks = calculateOperations.Select(x => x.Result).ToList();

            // add overall preparatory work

            var overallPreparatoryWork = new CalendarWork
            {
                WorkName = Constants.MainOverallPreparatoryWorkName,
                EstimateChapter = Constants.MainOverallPreparatoryTotalWorkChapter,
                ConstructionMonths = totalPreparatoryWork.ConstructionMonths,
                TotalCost = totalPreparatoryWork.TotalCost,
                TotalCostIncludingCAIW = totalPreparatoryWork.TotalCostIncludingCAIW
            };

            calendarWorks.Insert(0, overallPreparatoryWork);

            // create main total work

            var mainTotalWork = calendarWorks.Find(x => x.EstimateChapter == (int)estimate.TotalWorkChapter);

            if (mainTotalWork is null)
            {
                operation.AddError($"The total work with chapter {estimate.TotalWorkChapter} was not found.");
                return operation;
            }

            calendarWorks.Remove(mainTotalWork);

            mainTotalWork.WorkName = Constants.TotalWorkName;

            // add other expenses work

            var otherExpensesTotalCost = mainTotalWork.TotalCost - calendarWorks.Sum(x => x.TotalCost);
            var otherExpensesTotalCostIncludingCAIW =
                mainTotalWork.TotalCostIncludingCAIW - calendarWorks.Sum(x => x.TotalCostIncludingCAIW);
            var otherExpensesWorkOperation = await _calendarWorkCalculator.Calculate(Constants.MainOtherExpensesWorkName, otherExpensesTotalCost,
                otherExpensesTotalCostIncludingCAIW, estimate.ConstructionStartDate, otherExpensesPercentages,
                Constants.MainOtherExpensesWorkChapter);

            if (!otherExpensesWorkOperation.Ok)
            {
                operation.AddError(otherExpensesWorkOperation.GetMetadataMessages());
                return operation;
            }

            calendarWorks.Add(otherExpensesWorkOperation.Result);

            // add main total work

            mainTotalWork.ConstructionMonths = SumConstructionMonths(estimate.ConstructionDurationCeiling, calendarWorks);

            foreach (var constructionMonth in mainTotalWork.ConstructionMonths)
            {
                constructionMonth.PercentPart = constructionMonth.InvestmentVolume / mainTotalWork.TotalCost;
            }

            calendarWorks.Add(mainTotalWork);

            operation.Result = new CalendarPlan
            {
                ConstructionStartDate = estimate.ConstructionStartDate,
                CalendarWorks = calendarWorks,
                ConstructionDuration = estimate.ConstructionDuration,
                ConstructionDurationCeiling = estimate.ConstructionDurationCeiling
            };

            return operation;
        }

        private IEnumerable<ConstructionMonth> SumConstructionMonths(int constructionDurationCeiling,
            IEnumerable<CalendarWork> calendarWorks)
        {
            var totalMonths = new List<ConstructionMonth>();

            foreach (var calendarWork in calendarWorks)
            {
                for (int i = 0; i < constructionDurationCeiling; i++)
                {
                    var month = calendarWork.ConstructionMonths.FirstOrDefault(x => x.CreationIndex == i);

                    if (month is null)
                    {
                        continue;
                    }

                    var alreadyAddedMonth = totalMonths.Find(x => x.CreationIndex == i);
                    if (alreadyAddedMonth is null)
                    {
                        var newMonth = new ConstructionMonth
                        {
                            VolumeCAIW = month.VolumeCAIW,
                            CreationIndex = month.CreationIndex,
                            Date = month.Date,
                            InvestmentVolume = month.InvestmentVolume,
                            PercentPart = month.PercentPart
                        };

                        totalMonths.Add(newMonth);
                        continue;
                    }

                    alreadyAddedMonth.InvestmentVolume += month.InvestmentVolume;
                    alreadyAddedMonth.VolumeCAIW += month.VolumeCAIW;
                }
            }

            return totalMonths;
        }
    }
}