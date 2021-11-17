using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic
{
    public class ConstructionPeriod
    {
        public IEnumerable<ConstructionMonth> ConstructionMonths { get; }

        public ConstructionPeriod(IEnumerable<ConstructionMonth> constructionMonths)
        {
            ConstructionMonths = constructionMonths;
        }
    }
}
