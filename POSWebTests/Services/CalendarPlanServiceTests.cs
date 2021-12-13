using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using POSWeb.Services;
using POSWeb.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace POSWebTests.Services
{
    //TODO: bad class, need to split logic, it is untestable
    public class CalendarPlanServiceTests
    {
        private Mock<IEstimateService> _estimateService;
        private Mock<ICalendarPlanCreator> _calendarPlanCreatorMock;
        private Mock<ICalendarPlanWriter> _calendarPlanWriterMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;

        private CalendarPlanService CreateDefaultCalendarPlanService()
        {
            _estimateService = new Mock<IEstimateService>();
            _calendarPlanCreatorMock = new Mock<ICalendarPlanCreator>();
            _calendarPlanWriterMock = new Mock<ICalendarPlanWriter>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            return new CalendarPlanService(_estimateService.Object, _calendarPlanCreatorMock.Object, _calendarPlanWriterMock.Object,
                _webHostEnvironmentMock.Object);
        }

        [Test]
        public void GetEstimate()
        {
            var calendarPlanService = CreateDefaultCalendarPlanService();
            var estimateFiles = new List<IFormFile>();
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, "", 0);
            _estimateService.Setup(x => x.Estimate).Returns(estimate);

            var result = calendarPlanService.GetEstimate(estimateFiles);

            _estimateService.Verify(x => x.ReadEstimateFiles(estimateFiles), Times.Once);
            _estimateService.VerifyGet(x => x.Estimate, Times.Once);
            Assert.AreSame(estimate, result);
        }

        [Test]
        public void GetCalendarPlansPath()
        {
            var calendarPlanService = CreateDefaultCalendarPlanService();
            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("www");

            var energyAndWatersPath = calendarPlanService.GetCalendarPlansPath();

            Assert.AreEqual("www\\UsersFiles\\CalendarPlans", energyAndWatersPath);
        }

        public void GetCalendarPlanFileName()
        {
            var calendarPlanService = CreateDefaultCalendarPlanService();
            var userFullName = "BGTG\\kss";

            var calendarPlanFileName = calendarPlanService.GetCalendarPlanFileName(userFullName);

            Assert.AreEqual($"CalendarPlanBGTGkss.docx", calendarPlanFileName);
        }

        [Test]
        public void GetDownloadCalendarPlanFileName()
        {
            var calendarPlanService = CreateDefaultCalendarPlanService();
            var objectCipher = "5.5-20.548";
            var estimate = new Estimate(null, null, default(DateTime), 0, objectCipher, 0);
            _estimateService.Setup(x => x.Estimate).Returns(estimate);

            var downloadCalendarPlanName = calendarPlanService.GetDownloadCalendarPlanFileName();

            _estimateService.VerifyGet(x => x.Estimate, Times.Once);
            Assert.AreEqual($"{objectCipher}КП.docx", downloadCalendarPlanName);
        }
    }
}
