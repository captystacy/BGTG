using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarPlanCreator : ICalendarPlanCreator
    {
        private readonly Estimate _estimate;
        private readonly ICalendarWorkCreator _calendarWorkCreator;

        public CalendarPlanCreator(Estimate estimateVatFree, ICalendarWorkCreator calendarWorkCreator)
        {
            _estimate = estimateVatFree;
            _calendarWorkCreator = calendarWorkCreator;
        }

        public CalendarPlanCreator(Estimate[] estimates, IEstimateConnector estimateConnector, ICalendarWorkCreator calendarWorkCreator)
        {
            _estimate = estimateConnector.Connect(estimates);
            _calendarWorkCreator = calendarWorkCreator;
        }

        public CalendarPlan CreateCalendarPlan(DateTime initialDate, List<List<decimal>> percentagesGroups)
        {
            if (percentagesGroups.Count != _estimate.EstimateWorks.Count || percentagesGroups.Count == 0)
            {
                return null;
            }

            var calendarWorks = new List<CalendarWork>();
            for (int i = 0; i < _estimate.EstimateWorks.Count; i++)
            {
                var calendarWork = _calendarWorkCreator.CreateCalendarWork(initialDate, _estimate.EstimateWorks[i], percentagesGroups[i]);
                calendarWorks.Add(calendarWork);
            }

            return new CalendarPlan(calendarWorks);
        }
    }
}
