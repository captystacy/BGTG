using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using System;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarWorkCreator : ICalendarWorkCreator
    {
        private IConstructionPeriodCreator _constructionPeriodCreator;

        public CalendarWorkCreator(IConstructionPeriodCreator constructionPeriodCreator)
        {
            _constructionPeriodCreator = constructionPeriodCreator;
        }

        public CalendarWork CreateCalendarWork(DateTime initialDate, EstimateWork estimateWork, decimal[] percentages)
        {
            var totalCostIncludingContructionAndInstallationWorks = estimateWork.TotalCost - estimateWork.EquipmentCost - estimateWork.OtherProductsCost;

            var constructionPeriod = _constructionPeriodCreator.CreateConstructionPeriod(initialDate, estimateWork.TotalCost, totalCostIncludingContructionAndInstallationWorks, percentages);

            return new CalendarWork(estimateWork.WorkName, estimateWork.TotalCost, totalCostIncludingContructionAndInstallationWorks, constructionPeriod, estimateWork.Chapter);
        }
    }
}
