using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic.Interfaces;
using System;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarPlanCreator : ICalendarPlanCreator
    {
        private readonly IEstimateConnector _estimateConnector;
        private readonly IEstimate _estimate;

        public CalendarPlanCreator(IEstimate estimateVatFree)
        {
            _estimate = estimateVatFree;
        }

        public CalendarPlanCreator(IEstimate estimateVatFree, IEstimate estimateVat, IEstimateConnector estimateConnector)
        {
            _estimate = estimateConnector.Connect(estimateVatFree, estimateVat);
        }

        public ICalendarPlan CreateCalendarPlan()
        {
            throw new NotImplementedException();
        }
    }
}
