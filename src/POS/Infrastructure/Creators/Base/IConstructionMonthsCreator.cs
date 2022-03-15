using POS.DomainModels.CalendarPlanDomainModels;

namespace POS.Infrastructure.Creators.Base;

public interface IConstructionMonthsCreator
{
    IEnumerable<ConstructionMonth> Create(DateTime constructionStartDate, decimal totalCost, decimal totalCostIncludingCAIW, List<decimal> percentages);
}