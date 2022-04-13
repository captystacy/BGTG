using System;
using System.Collections.Generic;
using Calabonga.OperationResults;
using Moq;
using POS.Infrastructure.Calculators.Base;
using POS.Models.CalendarPlanModels;
using POS.Models.EstimateModels;

namespace POS.Tests.Helpers.Calculators
{
    public static class CalendarWorkCalculatorHelper
    {
        public static Mock<ICalendarWorkCalculator> GetMock()
        {
            return new Mock<ICalendarWorkCalculator>();
        }

        public static Mock<ICalendarWorkCalculator> GetMock(IEnumerable<OperationResult<CalendarWork>> operations)
        {
            var calendarWorkCalculator = GetMock();

            var setup = calendarWorkCalculator.SetupSequence(x => x.Calculate(It.IsAny<EstimateWork>(), It.IsAny<DateTime>()));

            foreach (var operation in operations)
            {
                setup.ReturnsAsync(operation);
            }

            return calendarWorkCalculator;
        }

        public static Mock<ICalendarWorkCalculator> GetMock(EstimateWork estimateWork, DateTime constructionStartDate, CalendarWork calendarWork)
        {
            var calendarWorkCalculator = GetMock();

            calendarWorkCalculator.Setup(x => x.Calculate(estimateWork, constructionStartDate))
                .ReturnsAsync(new OperationResult<CalendarWork> { Result = calendarWork });

            return calendarWorkCalculator;
        }
    }
}