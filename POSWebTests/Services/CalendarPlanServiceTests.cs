using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using POSWeb.Models;
using POSWeb.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace POSWebTests.Services
{
    //TODO: bad class, need to split logic, it is untestable
    public class CalendarPlanServiceTests
    {
        private Mock<IEstimateReader> _estimateReaderMock;
        private Mock<IEstimateConnector> _estimateConnectorMock;
        private Mock<ICalendarPlanCreator> _calendarPlanCreatorMock;
        private Mock<ICalendarPlanSeparator> _calendarPlanSeparatorMock;
        private Mock<ICalendarPlanWriter> _calendarPlanWriterMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;

        private CalendarPlanService CreateDefaultCalendarPlanService()
        {
            _estimateReaderMock = new Mock<IEstimateReader>();
            _estimateConnectorMock = new Mock<IEstimateConnector>();
            _calendarPlanCreatorMock = new Mock<ICalendarPlanCreator>();
            _calendarPlanSeparatorMock = new Mock<ICalendarPlanSeparator>();
            _calendarPlanWriterMock = new Mock<ICalendarPlanWriter>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            return new CalendarPlanService(_estimateReaderMock.Object, _estimateConnectorMock.Object, _calendarPlanCreatorMock.Object,
                _calendarPlanSeparatorMock.Object, _calendarPlanWriterMock.Object, _webHostEnvironmentMock.Object);
        }

        [Test]
        public void GetEstimate()
        {
            var calendarPlanService = CreateDefaultCalendarPlanService();
            var estimateFile1Mock = new Mock<IFormFile>();
            var estimateFile2Mock = new Mock<IFormFile>();
            var estimateFiles = new List<IFormFile> { estimateFile1Mock.Object, estimateFile2Mock.Object, };
            var stream1Mock = new Mock<Stream>();
            var stream2Mock = new Mock<Stream>();
            estimateFile1Mock.Setup(x => x.OpenReadStream()).Returns(stream1Mock.Object);
            estimateFile2Mock.Setup(x => x.OpenReadStream()).Returns(stream2Mock.Object);
            var estimate1 = new Estimate(new List<EstimateWork>(), DateTime.Today, 0, "");
            var estimate2 = new Estimate(new List<EstimateWork>(), DateTime.Today, 0, "");
            _estimateReaderMock.Setup(x => x.Read(stream1Mock.Object)).Returns(estimate1);
            _estimateReaderMock.Setup(x => x.Read(stream2Mock.Object)).Returns(estimate2);
            var estimates = new List<Estimate> { estimate1, estimate2 };
            var estimate = new Estimate(new List<EstimateWork>(), DateTime.Today, 0, "");
            _estimateConnectorMock.Setup(x => x.Connect(estimates)).Returns(estimate);

            var result = calendarPlanService.GetEstimate(estimateFiles);

            estimateFile1Mock.Verify(x => x.OpenReadStream(), Times.Once);
            estimateFile2Mock.Verify(x => x.OpenReadStream(), Times.Once);
            _estimateReaderMock.Verify(x => x.Read(stream1Mock.Object), Times.Once);
            _estimateReaderMock.Verify(x => x.Read(stream2Mock.Object), Times.Once);
            _estimateConnectorMock.Verify(x => x.Connect(estimates), Times.Once);
            Assert.AreSame(estimate, result);
        }

        //unfinished test
        [Test]
        public void GetMainTotalWork()
        {
            var calendarPlanService = CreateDefaultCalendarPlanService();
            var estimateFile1Mock = new Mock<IFormFile>();
            var estimateFile2Mock = new Mock<IFormFile>();
            var estimateFiles = new List<IFormFile> { estimateFile1Mock.Object, estimateFile2Mock.Object, };
            var userWorkMock = new Mock<UserWork>();
            userWorkMock.Setup(x => x.WorkName).Returns("");
            userWorkMock.Setup(x => x.Percentages).Returns(new List<decimal> { 30, 30, 40 });
            var estimate = new Estimate(new List<EstimateWork>
            {
                new EstimateWork("", 0, 0, 0, 0),
                new EstimateWork("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ", 0, 0, 0, 0),
                new EstimateWork("", 0, 0, 0, 1),
                    
            }, DateTime.Today, 0, "");
            _estimateConnectorMock.Setup(x => x.Connect(It.IsAny<List<Estimate>>())).Returns(estimate);
            var calendarPlanVM = new CalendarPlanVM()
            {
                UserWorks = new List<UserWork>
                {
                    userWorkMock.Object,
                    new UserWork() { WorkName = "Прочие работы и затраты" , Percentages = new List<decimal>() }
                }
            };
            _calendarPlanSeparatorMock.Setup(x => x.MainCalendarPlan).Returns(new CalendarPlan(new List<CalendarWork> { new CalendarWork(null, 0, 0, null, 10) }, DateTime.Today, 0));

            #region GetEstimateSetup
            var stream1Mock = new Mock<Stream>();
            var stream2Mock = new Mock<Stream>();
            estimateFile1Mock.Setup(x => x.OpenReadStream()).Returns(stream1Mock.Object);
            estimateFile2Mock.Setup(x => x.OpenReadStream()).Returns(stream2Mock.Object);
            var estimate1 = new Estimate(new List<EstimateWork>(), DateTime.Today, 0, "");
            var estimate2 = new Estimate(new List<EstimateWork>(), DateTime.Today, 0, "");
            _estimateReaderMock.Setup(x => x.Read(stream1Mock.Object)).Returns(estimate1);
            _estimateReaderMock.Setup(x => x.Read(stream2Mock.Object)).Returns(estimate2);
            var estimates = new List<Estimate> { estimate1, estimate2 };
            _estimateConnectorMock.Setup(x => x.Connect(estimates)).Returns(estimate);
            #endregion

            var result = calendarPlanService.GetMainTotalWork(estimateFiles, calendarPlanVM);

            userWorkMock.VerifySet(x => x.Percentages = new List<decimal> { (decimal)0.3, (decimal)0.3, (decimal)0.4 }, Times.Once);

            #region GetEstimateVerify
            estimateFile1Mock.Verify(x => x.OpenReadStream(), Times.Once);
            estimateFile2Mock.Verify(x => x.OpenReadStream(), Times.Once);
            _estimateReaderMock.Verify(x => x.Read(stream1Mock.Object), Times.Once);
            _estimateReaderMock.Verify(x => x.Read(stream2Mock.Object), Times.Once);
            _estimateConnectorMock.Verify(x => x.Connect(estimates), Times.Once);
            #endregion
        }

        [Test]
        public void GetCalendarPlansPath()
        {
            var calendarPlanService = CreateDefaultCalendarPlanService();
            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("www");

            var energyAndWatersPath = calendarPlanService.GetCalendarPlansPath();

            Assert.AreEqual("www\\UsersFiles\\CalendarPlans", energyAndWatersPath);
        }

        [Test]
        public void GetDownloadCalendarPlanName()
        {
            var calendarPlanService = CreateDefaultCalendarPlanService();
            var objectCipher = "5.5-20.548";

            var downloadCalendarPlanName = calendarPlanService.GetDownloadCalendarPlanName(objectCipher);

            Assert.AreEqual($"{objectCipher}КП.docx", downloadCalendarPlanName);
        }
    }
}
