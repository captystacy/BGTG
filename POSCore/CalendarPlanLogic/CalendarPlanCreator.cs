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

        public CalendarPlanCreator(Estimate estimateVatFree, Estimate estimateVat, IEstimateConnector estimateConnector, ICalendarWorkCreator calendarWorkCreator)
        {
            _estimate = estimateConnector.Connect(estimateVatFree, estimateVat);
            _calendarWorkCreator = calendarWorkCreator;
        }

        public CalendarPlan CreateCalendarPlan(DateTime initialDate, IEnumerable<decimal[]> percentagesGroups)
        {
            var estimateWorks = _estimate.EstimateWorks.ToArray();
            var persentagesGroupsArray = percentagesGroups.ToArray();

            if (persentagesGroupsArray.Length != estimateWorks.Length || persentagesGroupsArray.Length == 0)
            {
                return null;
            }

            var calendarWorks = new List<CalendarWork>();
            for (int i = 0; i < estimateWorks.Length; i++)
            {
                var calendarWork = _calendarWorkCreator.CreateCalendarWork(initialDate, estimateWorks[i], persentagesGroupsArray[i]);
                calendarWorks.Add(calendarWork);
            }

            return new CalendarPlan(calendarWorks);
        }
    }
}
