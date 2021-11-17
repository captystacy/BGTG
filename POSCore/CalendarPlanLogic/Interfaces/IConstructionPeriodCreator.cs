using System;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface IConstructionPeriodCreator
    {
        ConstructionPeriod CreateConstructionPeriod(DateTime initialDate, double totalCost, double totalCostIncludingContructionAndInstallationWorks, int[] percentages);
    }
}
