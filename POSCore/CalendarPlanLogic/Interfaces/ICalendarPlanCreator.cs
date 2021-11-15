using POSCore.EstimateLogic.Interfaces;
using System.Collections.Generic;

namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarPlanCreator
    {
        IEnumerable<IEstimate> Estimates { get; }

        ICalendarPlan CreatePreparatoryCalendarPlan();
        ICalendarPlan CreateCalendarPlan();
    }
}
