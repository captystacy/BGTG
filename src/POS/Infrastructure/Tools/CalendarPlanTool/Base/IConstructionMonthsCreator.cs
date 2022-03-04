using POS.Infrastructure.Tools.CalendarPlanTool.Models;

namespace POS.Infrastructure.Tools.CalendarPlanTool.Base;

public interface IConstructionMonthsCreator
{
    IEnumerable<ConstructionMonth> Create(DateTime constructionStartDate, decimal totalCost, decimal totalCostIncludingCAIW, List<decimal> percentages);
}