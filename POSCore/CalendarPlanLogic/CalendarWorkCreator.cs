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

        public CalendarWork Create(DateTime initialDate, EstimateWork estimateWork)
        {
            var totalCostIncludingContructionAndInstallationWorks = estimateWork.TotalCost - estimateWork.EquipmentCost - estimateWork.OtherProductsCost;

            ConstructionPeriod constructionPeriod = null;
            if (estimateWork.Percentages != null)
            {
                constructionPeriod = _constructionPeriodCreator.Create(initialDate, estimateWork.TotalCost, totalCostIncludingContructionAndInstallationWorks, estimateWork.Percentages);

            }

            return new CalendarWork(estimateWork.WorkName, estimateWork.TotalCost, totalCostIncludingContructionAndInstallationWorks, constructionPeriod, estimateWork.Chapter);
        }
    }
}
