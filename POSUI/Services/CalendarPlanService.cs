using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using POSUI.Services.Interfaces;
using System.Linq;

namespace POSUI.Services
{
    public class CalendarPlanService : ICalendarPlanService
    {
        private readonly IEstimateReader _estimateReader;
        private readonly ICalendarWorkCreator _calendarWorkCreator;
        private readonly IEstimateConnector _estimateConnector;
        private readonly ICalendarPlanCreator _calendarPlanCreator;

        public CalendarPlanService(IEstimateReader estimateReader, ICalendarWorkCreator calendarWorkCreator, IEstimateConnector estimateConnector, ICalendarPlanCreator calendarPlanCreator)
        {
            _estimateReader = estimateReader;
            _calendarWorkCreator = calendarWorkCreator;
            _estimateConnector = estimateConnector;
            _calendarPlanCreator = calendarPlanCreator;
        }

        public Estimate GetEstimate(string[] estimatesPaths)
        {
            var estimates = estimatesPaths.Select(x => _estimateReader.Read(x)).ToList();
            return _estimateConnector.Connect(estimates);
        }
    }
}
