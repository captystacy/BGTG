using System;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface IConstructionPeriodCreator
    {
       ConstructionPeriod CreateConstructionPeriod(DateTime initialDate, decimal totalCost, decimal totalCostIncludingContructionAndInstallationWorks, int[] percentages);
    }
}
