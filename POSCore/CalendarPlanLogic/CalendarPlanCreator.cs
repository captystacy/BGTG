using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarPlanCreator : ICalendarPlanCreator
    {
        private readonly ICalendarWorkCreator _calendarWorkCreator;

        public CalendarPlanCreator(ICalendarWorkCreator calendarWorkCreator)
        {
            _calendarWorkCreator = calendarWorkCreator;
        }

        public CalendarPlan Create(Estimate estimate, List<decimal> otherExpensesPercentages)
        {
            var preparatoryCalendarWorks = _calendarWorkCreator.CreatePreparatoryCalendarWorks(estimate.PreparatoryEstimateWorks, 
                estimate.ConstructionStartDate);
            var mainCalendarWorks = _calendarWorkCreator.CreateMainCalendarWorks(estimate.MainEstimateWorks, preparatoryCalendarWorks[^1], 
                estimate.ConstructionStartDate, estimate.ConstructionDuration, otherExpensesPercentages);
            return new CalendarPlan(preparatoryCalendarWorks, mainCalendarWorks, estimate.ConstructionStartDate, estimate.ConstructionDuration);
        }
    }
}
