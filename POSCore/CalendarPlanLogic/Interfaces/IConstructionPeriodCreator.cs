using System;
using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface IConstructionPeriodCreator
    {
       ConstructionPeriod Create(DateTime initialDate, decimal totalCost, decimal totalCostIncludingContructionAndInstallationWorks, List<decimal> percentages);
    }
}
