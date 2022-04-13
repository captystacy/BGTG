using System;
using System.IO;
using Calabonga.OperationResults;
using Moq;
using POS.Infrastructure.Readers.Base;

namespace POS.Tests.Helpers.Readers
{
    public static class CalendarPlanReaderHelper
    {
        public static Mock<ICalendarPlanReader> GetMock()
        {
            return new Mock<ICalendarPlanReader>();
        }

        public static Mock<ICalendarPlanReader> GetMock(Mock<Stream> calendarPlanStream, DateTime constructionStartDate)
        {
            var calendarPlanReader = GetMock();

            calendarPlanReader
                .Setup(x => x.GetConstructionStartDate(calendarPlanStream.Object))
                .ReturnsAsync(new OperationResult<DateTime> { Result = constructionStartDate });

            return calendarPlanReader;
        }
    }
}
