using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using System;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarPlanCreator : ICalendarPlanCreator
    {
        private readonly IEstimateConnector _estimateConnector;
        private readonly Estimate _estimate;

        public CalendarPlanCreator(Estimate estimateVatFree)
        {
            _estimate = estimateVatFree;
        }

        public CalendarPlanCreator(Estimate estimateVatFree, Estimate estimateVat, IEstimateConnector estimateConnector)
        {
            _estimate = estimateConnector.Connect(estimateVatFree, estimateVat);
        }

        public CalendarPlan CreateCalendarPlan()
        {
            throw new NotImplementedException();
        }
    }
}
