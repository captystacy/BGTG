using POSCore.CalendarPlanLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarPlanSeparator : ICalendarPlanSeparator
    {
        public CalendarPlan PreparatoryCalendarPlan { get; private set; }
        public CalendarPlan MainCalendarPlan { get; private set; }
        private IConstructionPeriodCreator _constructionPeriodCreator;

        public const string PreparatoryWork = "Подготовка территории строительства";
        public const string PreparatoryTemporaryBuildingsWork = "Временные здания и сооружения";
        public const string MainOverallPreparatoryWork = "Работы, выполняемые в подготовительный период";
        public const string MainOtherExpensesWork = "Прочие работы и затраты";
        public const string TotalWork = "Итого:";

        public const string TemporaryBuildingsSearchPattern = "ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ";
        public const string TotalWorkSearchPattern = "ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ";

        public CalendarPlanSeparator(IConstructionPeriodCreator constructionPeriodCreator)
        {
            _constructionPeriodCreator = constructionPeriodCreator;
        }

        public void Separate(CalendarPlan calendarPlan, List<decimal> otherExpensesPercentages)
        {
            var preparatoryCalendarWorks = CreatePreparatoryCalendarWorks(calendarPlan);

            var preparatoryTotalWork = CreatePreparatoryTotalWork(preparatoryCalendarWorks);
            preparatoryCalendarWorks.Add(preparatoryTotalWork);

            var overallPreparatoryWork = CreateOverallPreparatoryTotalWork(preparatoryTotalWork);

            var mainCalendarWorks = CreateMainCalendarWorks(calendarPlan, overallPreparatoryWork, otherExpensesPercentages);

            PreparatoryCalendarPlan = new CalendarPlan(preparatoryCalendarWorks, calendarPlan.ConstructionStartDate, calendarPlan.ConstructionDuration);
            MainCalendarPlan = new CalendarPlan(mainCalendarWorks, calendarPlan.ConstructionStartDate, calendarPlan.ConstructionDuration);
        }

        private List<CalendarWork> CreateMainCalendarWorks(CalendarPlan calendarPlan, CalendarWork overallPreparatoryWork, List<decimal> otherExpensesPercentages)
        {
            var mainCalendarWorks = new List<CalendarWork>();

            mainCalendarWorks.Add(overallPreparatoryWork);

            var estimateChapterAllExcept1and8and10CalendarWorks = calendarPlan.CalendarWorks.FindAll(x => x.EstimateChapter != 1 && !x.WorkName.StartsWith(TemporaryBuildingsSearchPattern) && x.EstimateChapter != 10);
            foreach (var calendarWork in estimateChapterAllExcept1and8and10CalendarWorks)
            {
                mainCalendarWorks.Add(calendarWork);
            }

            var estimateChapter10CalendarWork = calendarPlan.CalendarWorks.Find(x => x.EstimateChapter == 10);

            var otherExpensesWork = CreateOtherExpensesWork(mainCalendarWorks, estimateChapter10CalendarWork, otherExpensesPercentages, calendarPlan.ConstructionStartDate);
            mainCalendarWorks.Add(otherExpensesWork);

            var mainTotalWork = CreateMainTotalWork(mainCalendarWorks, estimateChapter10CalendarWork, calendarPlan.ConstructionStartDate, calendarPlan.ConstructionDuration);
            RepairMainTotalWorkConstructionPeriodPercents(mainTotalWork.ConstructionPeriod);
            mainCalendarWorks.Add(mainTotalWork);

            return mainCalendarWorks;
        }

        private void RepairMainTotalWorkConstructionPeriodPercents(ConstructionPeriod constructionPeriod)
        {
            var percentSum = constructionPeriod.ConstructionMonths.Sum(x => x.PercentPart);
            if (percentSum > 1)
            {
                var biggestPercentMonth = constructionPeriod.ConstructionMonths.OrderByDescending(x => x.PercentPart).First();
                var percentSumExcludedBiggestPercent = percentSum - biggestPercentMonth.PercentPart;
                var correctPercent = 1 - percentSumExcludedBiggestPercent;
                biggestPercentMonth.PercentPart = correctPercent;
            }
        }

        private List<CalendarWork> CreatePreparatoryCalendarWorks(CalendarPlan calendarPlan)
        {
            var preparatoryCalendarWorks = new List<CalendarWork>();

            var estimateChapter1CalendarWorks = calendarPlan.CalendarWorks.FindAll(x => x.EstimateChapter == 1 && x.TotalCostIncludingContructionAndInstallationWorks != 0);
            if (estimateChapter1CalendarWorks.Count != 0)
            {
                var preparatoryWork = CreatePreparatoryWork(estimateChapter1CalendarWorks);
                preparatoryCalendarWorks.Add(preparatoryWork);
            }

            var estimateTemporaryBuildingsCalendarWork = calendarPlan.CalendarWorks.Find(x => x.WorkName.StartsWith(TemporaryBuildingsSearchPattern));
            var temporaryBuildingsWork = CreateTemporaryBuildingsWork(estimateTemporaryBuildingsCalendarWork);
            preparatoryCalendarWorks.Add(temporaryBuildingsWork);

            return preparatoryCalendarWorks;
        }

        private CalendarWork CreateMainTotalWork(List<CalendarWork> mainCalendarWorks, CalendarWork estimateChapter10CalendarWork, DateTime constructionStartDate, decimal constructionDuration)
        {
            var constructiondDurationCeiling = (int)decimal.Ceiling(constructionDuration);
            return new CalendarWork(
                TotalWork,
                estimateChapter10CalendarWork.TotalCost,
                estimateChapter10CalendarWork.TotalCostIncludingContructionAndInstallationWorks,
                new ConstructionPeriod(Enumerable.Range(0, constructiondDurationCeiling).Select(i =>
                    new ConstructionMonth(
                        constructionStartDate.AddMonths(i),
                        decimal.Round(mainCalendarWorks.Sum(x => x.ConstructionPeriod.ConstructionMonths.Find(x => x.Date == constructionStartDate.AddMonths(i))?.InvestmentVolume).Value, 3),
                        decimal.Round(mainCalendarWorks.Sum(x => x.ConstructionPeriod.ConstructionMonths.Find(x => x.Date == constructionStartDate.AddMonths(i))?.ContructionAndInstallationWorksVolume).Value, 3),
                        decimal.Round((mainCalendarWorks.Sum(x => x.ConstructionPeriod.ConstructionMonths.Find(x => x.Date == constructionStartDate.AddMonths(i))?.InvestmentVolume) / estimateChapter10CalendarWork.TotalCost).Value, 2),
                        i
                )).ToList()),
                10);
        }

        private CalendarWork CreateOtherExpensesWork(List<CalendarWork> mainCalendarWorks, CalendarWork estimateChapter10CalendarWork,
            List<decimal> percentages, DateTime constructionStartDate)
        {
            var totalCost = decimal.Round(estimateChapter10CalendarWork.TotalCost - mainCalendarWorks.Sum(x => x.TotalCost), 3);
            var totalCostIncludingContructionAndInstallationWorks = decimal.Round(estimateChapter10CalendarWork.TotalCostIncludingContructionAndInstallationWorks - mainCalendarWorks.Sum(x => x.TotalCostIncludingContructionAndInstallationWorks), 3);
            return new CalendarWork(
                MainOtherExpensesWork,
                totalCost,
                totalCostIncludingContructionAndInstallationWorks,
                _constructionPeriodCreator.Create(
                    constructionStartDate,
                    totalCost,
                    totalCostIncludingContructionAndInstallationWorks,
                    percentages
                    ),
                9);
        }

        private CalendarWork CreateOverallPreparatoryTotalWork(CalendarWork preparatoryTotalWork)
        {
            return new CalendarWork(
                MainOverallPreparatoryWork,
                preparatoryTotalWork.TotalCost,
                preparatoryTotalWork.TotalCostIncludingContructionAndInstallationWorks,
                preparatoryTotalWork.ConstructionPeriod,
                2);
        }

        private CalendarWork CreatePreparatoryTotalWork(List<CalendarWork> preparatoryCalendarWorks)
        {
            if (preparatoryCalendarWorks.Count == 1)
            {
                return CreatePreparatoryTotalWork(preparatoryCalendarWorks[0]);
            }

            var preparatoryWorkConstructionMonth = preparatoryCalendarWorks[0].ConstructionPeriod.ConstructionMonths[0];
            var temporaryBuildingsWorkConstructionMonth = preparatoryCalendarWorks[1].ConstructionPeriod.ConstructionMonths[0];

            return new CalendarWork(
                TotalWork,
                preparatoryCalendarWorks[0].TotalCost + preparatoryCalendarWorks[1].TotalCost,
                preparatoryCalendarWorks[0].TotalCostIncludingContructionAndInstallationWorks + preparatoryCalendarWorks[1].TotalCostIncludingContructionAndInstallationWorks,
                new ConstructionPeriod(new List<ConstructionMonth>
                {
                    new ConstructionMonth(
                        preparatoryWorkConstructionMonth.Date,
                        preparatoryWorkConstructionMonth.InvestmentVolume + temporaryBuildingsWorkConstructionMonth.InvestmentVolume,
                        preparatoryWorkConstructionMonth.ContructionAndInstallationWorksVolume + temporaryBuildingsWorkConstructionMonth.ContructionAndInstallationWorksVolume,
                        1,
                        preparatoryWorkConstructionMonth.CreationIndex)
                }),
                1);
        }

        private CalendarWork CreatePreparatoryTotalWork(CalendarWork temporaryBuildingsWork)
        {
            return new CalendarWork(
                TotalWork,
                temporaryBuildingsWork.TotalCost,
                temporaryBuildingsWork.TotalCostIncludingContructionAndInstallationWorks,
                temporaryBuildingsWork.ConstructionPeriod,
                1);
        }

        private CalendarWork CreateTemporaryBuildingsWork(CalendarWork estimateChapter8CalendarWork)
        {
            return new CalendarWork(
                PreparatoryTemporaryBuildingsWork,
                estimateChapter8CalendarWork.TotalCost,
                estimateChapter8CalendarWork.TotalCostIncludingContructionAndInstallationWorks,
                estimateChapter8CalendarWork.ConstructionPeriod,
                8);
        }

        private CalendarWork CreatePreparatoryWork(List<CalendarWork> estimateChapter1CalendarWorks)
        {
            return new CalendarWork(
                PreparatoryWork,
                estimateChapter1CalendarWorks.Sum(x => x.TotalCost),
                estimateChapter1CalendarWorks.Sum(x => x.TotalCostIncludingContructionAndInstallationWorks),
                new ConstructionPeriod(new List<ConstructionMonth>
                {
                    new ConstructionMonth(
                        estimateChapter1CalendarWorks[0].ConstructionPeriod.ConstructionMonths[0].Date,
                        estimateChapter1CalendarWorks.Sum(x => x.ConstructionPeriod.ConstructionMonths[0].InvestmentVolume),
                        estimateChapter1CalendarWorks.Sum(x => x.ConstructionPeriod.ConstructionMonths[0].ContructionAndInstallationWorksVolume),
                        1,
                        0)
                }),
                1);
        }
    }
}
