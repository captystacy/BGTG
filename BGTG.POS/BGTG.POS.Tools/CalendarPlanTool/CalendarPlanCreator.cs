using System.Collections.Generic;
using System.Linq;
using BGTG.POS.Tools.CalendarPlanTool.Interfaces;
using BGTG.POS.Tools.EstimateTool;

namespace BGTG.POS.Tools.CalendarPlanTool
{
    public class CalendarPlanCreator : ICalendarPlanCreator
    {
        private readonly ICalendarWorksProvider _calendarWorksProvider;

        public CalendarPlanCreator(ICalendarWorksProvider calendarWorksProvider)
        {
            _calendarWorksProvider = calendarWorksProvider;
        }

        public CalendarPlan Create(Estimate estimate, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter)
        {
            var preparatoryCalendarWorks = _calendarWorksProvider.CreatePreparatoryCalendarWorks(
                estimate.PreparatoryEstimateWorks,
                estimate.ConstructionStartDate);

            var mainCalendarWorks = _calendarWorksProvider.CreateMainCalendarWorks(
                estimate.MainEstimateWorks, 
                preparatoryCalendarWorks.Single(x => x.WorkName == CalendarPlanInfo.TotalWorkName),
                estimate.ConstructionStartDate, 
                estimate.ConstructionDurationCeiling, 
                otherExpensesPercentages,
                totalWorkChapter);

            return new CalendarPlan(preparatoryCalendarWorks, mainCalendarWorks, estimate.ConstructionStartDate, estimate.ConstructionDurationCeiling);
        }
    }
}
