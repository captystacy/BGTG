using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarWorkConverter : ICalendarWorkConverter
    {
        public CalendarWork Convert(EstimateWork estimateWork, ConstructionPeriod constructionPeriod)
        {
            var totalCostIncludingContructionAndInstallationWorks = estimateWork.TotalCost - estimateWork.EquipmentCost - estimateWork.OtherProductsCost;
            return new CalendarWork(estimateWork.WorkName, estimateWork.TotalCost, totalCostIncludingContructionAndInstallationWorks, constructionPeriod);
        }
    }
}
