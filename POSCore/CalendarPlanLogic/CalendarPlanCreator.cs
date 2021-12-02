using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using System.Linq;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarPlanCreator : ICalendarPlanCreator
    {
        private readonly ICalendarWorkCreator _calendarWorkCreator;

        public CalendarPlanCreator(ICalendarWorkCreator calendarWorkCreator)
        {
            _calendarWorkCreator = calendarWorkCreator;
        }

        public CalendarPlan Create(Estimate estimate)
        {
            var calendarWorks = estimate.EstimateWorks.Select(
                estimateWork => _calendarWorkCreator.Create(estimate.ConstructionStartDate, estimateWork));

            return new CalendarPlan(calendarWorks.ToList(), estimate.ConstructionStartDate, estimate.ConstructionDuration);
        }
    }
}
