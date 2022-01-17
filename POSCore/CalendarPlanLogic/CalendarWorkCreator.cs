using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarWorkCreator : ICalendarWorkCreator
    {
        private readonly IConstructionMonthsCreator _constructionMonthsCreator;

        public CalendarWorkCreator(IConstructionMonthsCreator constructionMonthsCreator)
        {
            _constructionMonthsCreator = constructionMonthsCreator;
        }

        private CalendarWork Create(string workName, decimal totalCost, decimal totalCostIncludingCaiw, int estimateChapter, IEnumerable<ConstructionMonth> constructionMonths)
        {
            return new CalendarWork(workName, totalCost, totalCostIncludingCaiw, constructionMonths, estimateChapter);
        }

        private CalendarWork Create(string workName, decimal totalCost, decimal totalCostIncludingCaiw, int estimateChapter,
            DateTime constructionStartDate, List<decimal> percentages)
        {
            var constructionMonths = _constructionMonthsCreator.Create(constructionStartDate, totalCost, totalCostIncludingCaiw, percentages);
            return Create(workName, totalCost, totalCostIncludingCaiw, estimateChapter, constructionMonths);
        }

        public CalendarWork Create(EstimateWork estimateWork, DateTime constructionStartDate)
        {
            var totalCostIncludingCaiw = estimateWork.TotalCost - estimateWork.EquipmentCost - estimateWork.OtherProductsCost;
            return Create(estimateWork.WorkName, estimateWork.TotalCost, totalCostIncludingCaiw, estimateWork.Chapter, constructionStartDate, estimateWork.Percentages);
        }

        public CalendarWork CreateAnyPreparatoryWork(string workName, List<CalendarWork> preparatoryCalendarWorks, int estimateChapter,
            DateTime constructionStartDate, List<decimal> percentages)
        {
            var totalCost = preparatoryCalendarWorks.Sum(x => x.TotalCost);
            var totalCostIncludingCaiw = preparatoryCalendarWorks.Sum(x => x.TotalCostIncludingCAIW);
            var constructionMonths = _constructionMonthsCreator.Create(constructionStartDate, totalCost, totalCostIncludingCaiw, percentages);
            return Create(workName, totalCost, totalCostIncludingCaiw, estimateChapter, constructionMonths);
        }

        public CalendarWork CreateMainTotalWork(List<CalendarWork> mainCalendarWorks, CalendarWork initialMainTotalWork, DateTime constructionStartDate, int constructionDurationCeiling)
        {
            var constructionMonths = Enumerable.Range(0, constructionDurationCeiling).Select(i =>
                    new ConstructionMonth(
                        constructionStartDate.AddMonths(i),
                        mainCalendarWorks.Sum(calendarWork => calendarWork.ConstructionMonths.SingleOrDefault(constructionMonth => constructionMonth.Date == constructionStartDate.AddMonths(i))?.InvestmentVolume).Value,
                        mainCalendarWorks.Sum(calendarWork => calendarWork.ConstructionMonths.SingleOrDefault(constructionMonth => constructionMonth.Date == constructionStartDate.AddMonths(i))?.CAIWVolume).Value,
                        (mainCalendarWorks.Sum(calendarWork => calendarWork.ConstructionMonths.SingleOrDefault(constructionMonth => constructionMonth.Date == constructionStartDate.AddMonths(i))?.InvestmentVolume) / initialMainTotalWork.TotalCost).Value,
                        i
                )).ToList();

            return Create(CalendarPlanInfo.TotalWorkName, initialMainTotalWork.TotalCost, initialMainTotalWork.TotalCostIncludingCAIW,
                CalendarPlanInfo.MainTotalWork1To12Chapter, constructionMonths);
        }

        public CalendarWork CreateOtherExpensesWork(List<CalendarWork> mainCalendarWorks, CalendarWork initialMainTotalWork,
            DateTime constructionStartDate, List<decimal> percentages)
        {
            var totalCost = initialMainTotalWork.TotalCost - mainCalendarWorks.Sum(x => x.TotalCost);
            var totalCostIncludingCaiw = initialMainTotalWork.TotalCostIncludingCAIW - mainCalendarWorks.Sum(x => x.TotalCostIncludingCAIW);
            return Create(CalendarPlanInfo.MainOtherExpensesWorkName, totalCost, totalCostIncludingCaiw, CalendarPlanInfo.MainOtherExpensesWorkChapter, constructionStartDate, percentages);
        }
    }
}
