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

        private readonly List<decimal> _preparatoryCaledarWorkPercentages = new List<decimal>() { 1 };

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

            var estimateChapter1CalendarWorks = calendarWorks.FindAll(x => x.EstimateChapter == _preparatoryWorkChapter && x.TotalCostIncludingContructionAndInstallationWorks != 0);
            if (estimateChapter1CalendarWorks.Count != 0)
            {
                var preparatoryCalendarWork = SumPreparatoryCalendarWorks(estimateChapter1CalendarWorks, PreparatoryWorkName, _preparatoryWorkChapter, constructionStartDate);
                preparatoryCalendarWorks.Add(preparatoryCalendarWork);
            }

            var temporaryBuildingsWork = calendarWorks.Find(x => x.EstimateChapter == _temporaryBuildingsWorkChapter);
            temporaryBuildingsWork.WorkName = TemporaryBuildingsWorkName;
            preparatoryCalendarWorks.Add(temporaryBuildingsWork);

            var totalCalendarWork = SumPreparatoryCalendarWorks(preparatoryCalendarWorks, TotalWorkName, _preparatoryTotalWorkChapter, constructionStartDate);
            preparatoryCalendarWorks.Add(totalCalendarWork);

            return preparatoryCalendarWorks;
        }

        private CalendarWork SumPreparatoryCalendarWorks(List<CalendarWork> calendarWorks, string workName, int estimateChapter, DateTime constructionStartDate)
        {
            var totalCost = calendarWorks.Sum(x => x.TotalCost);
            var totalCostIncludingContructionAndInstallationWorks = calendarWorks.Sum(x => x.TotalCostIncludingContructionAndInstallationWorks);
            var cosntructionPeriod = _constructionPeriodCreator.Create(constructionStartDate, totalCost, totalCostIncludingContructionAndInstallationWorks, _preparatoryCaledarWorkPercentages);
            return new CalendarWork(workName, totalCost, totalCostIncludingContructionAndInstallationWorks, cosntructionPeriod, estimateChapter);
        }

        public List<CalendarWork> CreateMainCalendarWorks(List<EstimateWork> estimateWorks, CalendarWork preparatoryTotalWork, DateTime constructionStartDate, decimal constructionDuration, List<decimal> otherExpensesPercentages)
        {
            var mainCalendarWorks = estimateWorks.Select(x => Create(x, constructionStartDate)).ToList();
            var estimateChapter10CalendarWork = mainCalendarWorks.Find(x => x.EstimateChapter == 10);
            mainCalendarWorks.Remove(estimateChapter10CalendarWork);

            var mainOverallPreparatoryWork = new CalendarWork(MainOverallPreparatoryWorkName, preparatoryTotalWork.TotalCost,
                preparatoryTotalWork.TotalCostIncludingContructionAndInstallationWorks, preparatoryTotalWork.ConstructionPeriod,
                _overallPreparatoryTotalWorkChapter);
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
