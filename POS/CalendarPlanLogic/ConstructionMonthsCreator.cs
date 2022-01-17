using System;
using System.Collections.Generic;
using POS.CalendarPlanLogic.Interfaces;

namespace POS.CalendarPlanLogic
{
    public class ConstructionMonthsCreator : IConstructionMonthsCreator
    {
        public IEnumerable<ConstructionMonth> Create(DateTime constructionStartDate, decimal totalCost, decimal totalCostIncludingCAIW, List<decimal> percentages)
        {
            var constructionMonths = new List<ConstructionMonth>();

            for (int i = 0; i < percentages.Count; i++)
            {
                if (percentages[i] <= 0 || percentages[i] > 1)
                {
                    continue;
                }

                var date = constructionStartDate.AddMonths(i);
                var investmentVolume = totalCost * percentages[i];
                var caiwVolume = totalCostIncludingCAIW * percentages[i];
                var constructionMonth = new ConstructionMonth(date, investmentVolume, caiwVolume, percentages[i], i);
                constructionMonths.Add(constructionMonth);
            }

            return constructionMonths;
        }
    }
}
