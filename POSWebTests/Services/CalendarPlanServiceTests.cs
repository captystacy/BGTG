using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using POSWeb.Services;
using POSWeb.Services.Interfaces;
using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using POSCore.CalendarPlanLogic;
using POSWeb.Models;

namespace POSWebTests.Services
{
    //TODO: bad class, need to split logic, it is untestable
    public class CalendarPlanServiceTests
    {
        private CalendarPlanService _calendarPlanService;
        private Mock<IMapper> _mapperMock;
        private Mock<IEstimateService> _estimateServiceMock;
        private Mock<ICalendarPlanCreator> _calendarPlanCreatorMock;
        private Mock<ICalendarPlanWriter> _calendarPlanWriterMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;

        [SetUp]
        public void SetUp()
        {
            _mapperMock = new Mock<IMapper>();
            _estimateServiceMock = new Mock<IEstimateService>();
            _calendarPlanCreatorMock = new Mock<ICalendarPlanCreator>();
            _calendarPlanWriterMock = new Mock<ICalendarPlanWriter>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _calendarPlanService = new CalendarPlanService(_estimateServiceMock.Object, _calendarPlanCreatorMock.Object, _calendarPlanWriterMock.Object,
                _webHostEnvironmentMock.Object, _mapperMock.Object);

        }

        [Test]
        public void GetCalendarPlanVM()
        {
            var estimateFiles = new List<IFormFile>();
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, "", 0);
            var calendarPlanVM = new CalendarPlanVM()
            {
                UserWorks = new List<UserWorkVM>()
                {
                    new UserWorkVM()
                    {
                        WorkName = CalendarPlanInfo.TotalWorkName,
                        Chapter = CalendarPlanInfo.MainTotalWork1To12Chapter
                    },
                }
            };
            _estimateServiceMock.SetupGet(x => x.Estimate).Returns(estimate);
            _mapperMock.Setup(x => x.Map<CalendarPlanVM>(estimate)).Returns(calendarPlanVM);

            var expectedCalendarPlanVM = new CalendarPlanVM()
            {
                UserWorks = new List<UserWorkVM>()
                {
                    new UserWorkVM()
                    {
                        WorkName = CalendarPlanInfo.MainOtherExpensesWorkName,
                        Chapter = CalendarPlanInfo.MainOtherExpensesWorkChapter,
                        Percentages = new List<decimal>()
                    }
                }
            };

            var actualCalendarPlanVM = _calendarPlanService.GetCalendarPlanVM(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter);

            _estimateServiceMock.Verify(x => x.ReadEstimateFiles(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
            _mapperMock.Verify(x => x.Map<CalendarPlanVM>(estimate), Times.Once);
            Assert.AreEqual(expectedCalendarPlanVM, actualCalendarPlanVM);
        }

        [Test]
        public void GetCalendarPlansPath()
        {
            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("www");

            var energyAndWatersPath = _calendarPlanService.GetCalendarPlansPath();

            Assert.AreEqual("www\\UsersFiles\\CalendarPlans", energyAndWatersPath);
        }

        [Test]
        public void GetCalendarPlanFileName()
        {
            var userFullName = "BGTG\\kss";

            var calendarPlanFileName = _calendarPlanService.GetCalendarPlanFileName(userFullName);

            Assert.AreEqual($"CalendarPlanBGTGkss.docx", calendarPlanFileName);
        }

        [Test]
        public void GetDownloadCalendarPlanFileName()
        {
            var objectCipher = "5.5-20.548";
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, objectCipher, 0);
            _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);

            var downloadCalendarPlanName = _calendarPlanService.GetDownloadCalendarPlanFileName(objectCipher);

            Assert.AreEqual($"{objectCipher}КП.docx", downloadCalendarPlanName);
        }
    }
}
