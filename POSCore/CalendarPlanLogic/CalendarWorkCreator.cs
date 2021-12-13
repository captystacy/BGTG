using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarWorkCreator : ICalendarWorkCreator
    {
        private IConstructionPeriodCreator _constructionPeriodCreator;

        public const string PreparatoryWorkName = "Подготовка территории строительства";
        public const string TemporaryBuildingsWorkName = "Временные здания и сооружения";
        public const string MainOverallPreparatoryWorkName = "Работы, выполняемые в подготовительный период";
        public const string MainOtherExpensesWorkName = "Прочие работы и затраты";
        public const string TotalWorkName = "Итого:";

        private const int _preparatoryWorkChapter = 1;
        private const int _preparatoryTotalWorkChapter = 1;
        private const int _overallPreparatoryTotalWorkChapter = 2;
        private const int _temporaryBuildingsWorkChapter = 8;
        private const int _otherExpensesWorkChapter = 9;
        private const int _mainTotalWorkChapter = 10;

        public CalendarWorkCreator(IConstructionPeriodCreator constructionPeriodCreator)
        {
            _constructionPeriodCreator = constructionPeriodCreator;
        }

        public CalendarWork Create(EstimateWork estimateWork, DateTime constructionStartDate)
        {
            var totalCostIncludingContructionAndInstallationWorks = estimateWork.TotalCost - estimateWork.EquipmentCost - estimateWork.OtherProductsCost;

            var constructionPeriod = _constructionPeriodCreator.Create(constructionStartDate, estimateWork.TotalCost, totalCostIncludingContructionAndInstallationWorks, estimateWork.Percentages);

            return new CalendarWork(estimateWork.WorkName, estimateWork.TotalCost, totalCostIncludingContructionAndInstallationWorks, constructionPeriod, estimateWork.Chapter);
        }

        public List<CalendarWork> CreatePreparatoryCalendarWorks(List<EstimateWork> preparatoryEstimateWorks, DateTime constructionStartDate)
        {
            var preparatoryCalendarWorks = new List<CalendarWork>();
            var calendarWorks = preparatoryEstimateWorks.Select(x => Create(x, constructionStartDate)).ToList();

            var estimateChapter1CalendarWorks = calendarWorks.FindAll(x => x.EstimateChapter == 1 && x.TotalCostIncludingContructionAndInstallationWorks != 0);
            if (estimateChapter1CalendarWorks.Count != 0)
            {
                var preparatoryCalendarWork = SumCalendarWorks(estimateChapter1CalendarWorks, PreparatoryWorkName, _preparatoryWorkChapter);
                preparatoryCalendarWorks.Add(preparatoryCalendarWork);
            }

            var temporaryBuildingsWork = calendarWorks.Find(x => x.EstimateChapter == 8);
            temporaryBuildingsWork.WorkName = TemporaryBuildingsWorkName;
            preparatoryCalendarWorks.Add(temporaryBuildingsWork);

            var totalCalendarWork = SumCalendarWorks(preparatoryCalendarWorks, TotalWorkName, _preparatoryTotalWorkChapter);
            preparatoryCalendarWorks.Add(totalCalendarWork);

            return preparatoryCalendarWorks;
        }

        private CalendarWork SumCalendarWorks(List<CalendarWork> calendarWorks, string workName, int estimateChapter)
        {
            var totalCostIncludingContructionAndInstallationWorks = calendarWorks.Sum(x => x.TotalCostIncludingContructionAndInstallationWorks);
            var totalCost = calendarWorks.Sum(x => x.TotalCost);
            return new CalendarWork(workName, totalCost, totalCostIncludingContructionAndInstallationWorks, calendarWorks[0].ConstructionPeriod, estimateChapter);
        }

        public List<CalendarWork> CreateMainCalendarWorks(List<EstimateWork> estimateWorks, CalendarWork preparatoryTotalWork, DateTime constructionStartDate, decimal constructionDuration, List<decimal> otherExpensesPercentages)
        {
            var mainCalendarWorks = estimateWorks.Select(x => Create(x, constructionStartDate)).ToList();
            var estimateChapter10CalendarWork = mainCalendarWorks[^1];
            mainCalendarWorks.RemoveAt(mainCalendarWorks.Count - 1);

            var mainOverallPreparatoryWork = new CalendarWork(MainOverallPreparatoryWorkName, preparatoryTotalWork.TotalCost,
                preparatoryTotalWork.TotalCostIncludingContructionAndInstallationWorks, preparatoryTotalWork.ConstructionPeriod,
                preparatoryTotalWork.EstimateChapter);
            mainCalendarWorks.Insert(0, mainOverallPreparatoryWork);

            var otherExpensesCalendarWork = CreateOtherExpensesWork(mainCalendarWorks, estimateChapter10CalendarWork, otherExpensesPercentages,
                constructionStartDate);
            mainCalendarWorks.Add(otherExpensesCalendarWork);

            var mainTotalWork = CreateMainTotalWork(mainCalendarWorks, estimateChapter10CalendarWork, constructionStartDate, constructionDuration);
            mainCalendarWorks.Add(mainTotalWork);

            return mainCalendarWorks;
        }

        private CalendarWork CreateMainTotalWork(List<CalendarWork> mainCalendarWorks, CalendarWork estimateChapter10CalendarWork, DateTime constructionStartDate, decimal constructionDuration)
        {
            var constructiondDurationCeiling = (int)decimal.Ceiling(constructionDuration);
            return new CalendarWork(
                TotalWorkName,
                estimateChapter10CalendarWork.TotalCost,
                estimateChapter10CalendarWork.TotalCostIncludingContructionAndInstallationWorks,
                new ConstructionPeriod(Enumerable.Range(0, constructiondDurationCeiling).Select(i =>
                    new ConstructionMonth(
                        constructionStartDate.AddMonths(i),
                        mainCalendarWorks.Sum(x => x.ConstructionPeriod.ConstructionMonths.Find(x => x.Date == constructionStartDate.AddMonths(i))?.InvestmentVolume).Value,
                        mainCalendarWorks.Sum(x => x.ConstructionPeriod.ConstructionMonths.Find(x => x.Date == constructionStartDate.AddMonths(i))?.ContructionAndInstallationWorksVolume).Value,
                        (mainCalendarWorks.Sum(x => x.ConstructionPeriod.ConstructionMonths.Find(x => x.Date == constructionStartDate.AddMonths(i))?.InvestmentVolume) / estimateChapter10CalendarWork.TotalCost).Value,
                        i
                )).ToList()),
                _mainTotalWorkChapter);
        }

        private CalendarWork CreateOtherExpensesWork(List<CalendarWork> mainCalendarWorks, CalendarWork estimateChapter10CalendarWork,
            List<decimal> percentages, DateTime constructionStartDate)
        {
            var totalCost = estimateChapter10CalendarWork.TotalCost - mainCalendarWorks.Sum(x => x.TotalCost);
            var totalCostIncludingContructionAndInstallationWorks = estimateChapter10CalendarWork.TotalCostIncludingContructionAndInstallationWorks - mainCalendarWorks.Sum(x => x.TotalCostIncludingContructionAndInstallationWorks);
            return new CalendarWork(
                MainOtherExpensesWorkName,
                totalCost,
                totalCostIncludingContructionAndInstallationWorks,
                _constructionPeriodCreator.Create(
                    constructionStartDate,
                    totalCost,
                    totalCostIncludingContructionAndInstallationWorks,
                    percentages
                    ),
                _otherExpensesWorkChapter);
        }
    }
}
