using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic
{
    public class ConstructionPeriod
    {
        public List<ConstructionMonth> ConstructionMonths { get; }

        public ConstructionPeriod(List<ConstructionMonth> constructionMonths)
        {
            ConstructionMonths = constructionMonths;
        }
    }
}
