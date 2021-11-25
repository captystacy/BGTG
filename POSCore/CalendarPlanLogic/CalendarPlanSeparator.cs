using POSCore.CalendarPlanLogic.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarPlanSeparator : ICalendarPlanSeparator
    {
        public CalendarPlan PreparatoryCalendarPlan { get; private set; }
        public CalendarPlan MainCalendarPlan { get; private set; }
        private IConstructionPeriodCreator _constructionPeriodCreator;

        private const string _preparatoryWork = "Подготовка территории строительства";
        private const string _preparatoryTemporaryBuildingsWork = "Временные здания и сооружения";

        private const string _mainOverallPreparatoryWork = "Работы, выполняемые в подготовительный период";
        private const string _mainOtherExpensesWork = "Прочие работы и затраты";

        private const string _totalWork = "Итого:";

        public CalendarPlanSeparator(IConstructionPeriodCreator constructionPeriodCreator)
        {
            _constructionPeriodCreator = constructionPeriodCreator;
        }

        public void Separate(CalendarPlan calendarPlan)
        {
            var preparatoryCalendarWorks = CreatePreparatoryCalendarWorks(calendarPlan);

            var preparatoryTotalWork = CreatePreparatoryTotalWork(preparatoryCalendarWorks);
            preparatoryCalendarWorks.Add(preparatoryTotalWork);

            var overallPreparatoryWork = CreateOverallPreparatoryTotalWork(preparatoryTotalWork);

            var mainCalendarWorks = CreateMainCalendarWorks(calendarPlan, overallPreparatoryWork);

            PreparatoryCalendarPlan = new CalendarPlan(preparatoryCalendarWorks);
            MainCalendarPlan = new CalendarPlan(mainCalendarWorks);
        }

        private List<CalendarWork> CreateMainCalendarWorks(CalendarPlan calendarPlan, CalendarWork overallPreparatoryWork)
        {
            var mainCalendarWorks = new List<CalendarWork>();

            mainCalendarWorks.Add(overallPreparatoryWork);

            var estimateChapterAllExcept1and8and10CalendarWorks = calendarPlan.CalendarWorks.FindAll(x => x.EstimateChapter != 1 && x.EstimateChapter != 8 && x.EstimateChapter != 10);
            foreach (var calendarWork in estimateChapterAllExcept1and8and10CalendarWorks)
            {
                mainCalendarWorks.Add(calendarWork);
            }

            var estimateChapter10CalendarWork = calendarPlan.CalendarWorks.Find(x => x.EstimateChapter == 10);

            var otherExpensesWork = CreateOtherExpensesWork(mainCalendarWorks, estimateChapter10CalendarWork, estimateChapter10CalendarWork.ConstructionPeriod.ConstructionMonths.Select(x => x.PercentePart).ToList());
            mainCalendarWorks.Add(otherExpensesWork);

            var mainTotalWork = CreateMainTotalWork(mainCalendarWorks, estimateChapter10CalendarWork);
            mainCalendarWorks.Add(mainTotalWork);

            return mainCalendarWorks;
        }

        private List<CalendarWork> CreatePreparatoryCalendarWorks(CalendarPlan calendarPlan)
        {
            var preparatoryCalendarWorks = new List<CalendarWork>();

            var estimateChapter1CalendarWorks = calendarPlan.CalendarWorks.FindAll(x => x.EstimateChapter == 1);
            if (estimateChapter1CalendarWorks.Count != 0)
            {
                var preparatoryWork = CreatePreparatoryWork(estimateChapter1CalendarWorks);
                preparatoryCalendarWorks.Add(preparatoryWork);
            }

            var estimateChapter8CalendarWork = calendarPlan.CalendarWorks.Find(x => x.EstimateChapter == 8);
            var temporaryBuildingsWork = CreateTemporaryBuildingsWork(estimateChapter8CalendarWork);
            preparatoryCalendarWorks.Add(temporaryBuildingsWork);

            return preparatoryCalendarWorks;
        }

        private CalendarWork CreateMainTotalWork(List<CalendarWork> mainCalendarWorks, CalendarWork estimateChapter10CalendarWork)
        {
            return new CalendarWork(
                _totalWork,
                estimateChapter10CalendarWork.TotalCost,
                estimateChapter10CalendarWork.TotalCostIncludingContructionAndInstallationWorks,
                new ConstructionPeriod(estimateChapter10CalendarWork.ConstructionPeriod.ConstructionMonths.Select(month =>
                    new ConstructionMonth(
                        month.Date,
                        decimal.Round(mainCalendarWorks.Sum(x => x.ConstructionPeriod.ConstructionMonths.Find(x => x.Date == month.Date)?.InvestmentVolume).Value, 3),
                        decimal.Round(mainCalendarWorks.Sum(x => x.ConstructionPeriod.ConstructionMonths.Find(x => x.Date == month.Date)?.ContructionAndInstallationWorksVolume).Value, 3),
                        decimal.Round((mainCalendarWorks.Sum(x => x.ConstructionPeriod.ConstructionMonths.Find(x => x.Date == month.Date)?.InvestmentVolume) / estimateChapter10CalendarWork.TotalCost).Value, 2),
                        month.Index
                )).ToList()),
                10);
        }

        private CalendarWork CreateOtherExpensesWork(List<CalendarWork> mainCalendarWorks, CalendarWork estimateChapter10CalendarWork, List<decimal> percentages)
        {
            var totalCost = decimal.Round(estimateChapter10CalendarWork.TotalCost - mainCalendarWorks.Sum(x => x.TotalCost), 3);
            var totalCostIncludingContructionAndInstallationWorks = decimal.Round(estimateChapter10CalendarWork.TotalCostIncludingContructionAndInstallationWorks - mainCalendarWorks.Sum(x => x.TotalCostIncludingContructionAndInstallationWorks), 3);
            return new CalendarWork(
                _mainOtherExpensesWork,
                totalCost,
                totalCostIncludingContructionAndInstallationWorks,
                _constructionPeriodCreator.CreateConstructionPeriod(
                    estimateChapter10CalendarWork.ConstructionPeriod.ConstructionMonths[0].Date,
                    totalCost,
                    totalCostIncludingContructionAndInstallationWorks,
                    percentages
                    ),
                9);
        }

        private CalendarWork CreateOverallPreparatoryTotalWork(CalendarWork preparatoryTotalWork)
        {
            return new CalendarWork(
                _mainOverallPreparatoryWork,
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
                _totalWork,
                preparatoryCalendarWorks[0].TotalCost + preparatoryCalendarWorks[1].TotalCost,
                preparatoryCalendarWorks[0].TotalCostIncludingContructionAndInstallationWorks + preparatoryCalendarWorks[1].TotalCostIncludingContructionAndInstallationWorks,
                new ConstructionPeriod(new List<ConstructionMonth> 
                { 
                    new ConstructionMonth(
                        preparatoryWorkConstructionMonth.Date,
                        preparatoryWorkConstructionMonth.InvestmentVolume + temporaryBuildingsWorkConstructionMonth.InvestmentVolume,
                        preparatoryWorkConstructionMonth.ContructionAndInstallationWorksVolume + temporaryBuildingsWorkConstructionMonth.ContructionAndInstallationWorksVolume,
                        1,
                        preparatoryWorkConstructionMonth.Index)
                }),
                1);
        }

        private CalendarWork CreatePreparatoryTotalWork(CalendarWork temporaryBuildingsWork)
        {
            return new CalendarWork(
                _totalWork,
                temporaryBuildingsWork.TotalCost,
                temporaryBuildingsWork.TotalCostIncludingContructionAndInstallationWorks,
                temporaryBuildingsWork.ConstructionPeriod,
                1);
        }

        private CalendarWork CreateTemporaryBuildingsWork(CalendarWork estimateChapter8CalendarWork)
        {
            return new CalendarWork(
                _preparatoryTemporaryBuildingsWork,
                estimateChapter8CalendarWork.TotalCost,
                estimateChapter8CalendarWork.TotalCostIncludingContructionAndInstallationWorks,
                estimateChapter8CalendarWork.ConstructionPeriod,
                8);
        }

        private CalendarWork CreatePreparatoryWork(List<CalendarWork> estimateChapter1CalendarWorks)
        {
            return new CalendarWork(
                _preparatoryWork,
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
