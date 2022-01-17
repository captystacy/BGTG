using System;
using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface IConstructionMonthsCreator
    {
       IEnumerable<ConstructionMonth> Create(DateTime constructionStartDate, decimal totalCost, decimal totalCostIncludingCAIW, List<decimal> percentages);
    }
}
