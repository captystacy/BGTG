using System;
using System.Collections.Generic;
using AutoMapper;
using BGTGWeb.Models;
using BGTGWeb.Services;
using BGTGWeb.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using POS.CalendarPlanLogic;
using POS.CalendarPlanLogic.Interfaces;
using POS.EstimateLogic;

namespace BGTGWebTests.Services
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
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, 0, "", 0);
            var calendarPlanVM = new CalendarPlanVM()
            {
                UserWorks = new List<UserWorkVM>()
                {
                    new UserWorkVM()
                    {
                        WorkName = CalendarPlanInfo.TotalWorkName,
                        Chapter = (int)TotalWorkChapter.TotalWork1To12Chapter
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

            _estimateServiceMock.Verify(x => x.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
            _mapperMock.Verify(x => x.Map<CalendarPlanVM>(estimate), Times.Once);
            Assert.AreEqual(expectedCalendarPlanVM, actualCalendarPlanVM);
        }

        [Test]
        public void GetSavePath()
        {
            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("www");
            var userFullName = "BGTG\\kss";

            var savePath = _calendarPlanService.GetSavePath(userFullName);

            _webHostEnvironmentMock.VerifyGet(x => x.WebRootPath, Times.Once);
            Assert.AreEqual(@"www\UsersFiles\CalendarPlans\CalendarPlanBGTGkss.docx", savePath);
        }

        [Test]
        public void GetFileName()
        {
            var objectCipher = "5.5-20.548";

            var fileName = _calendarPlanService.GetFileName(objectCipher);

            Assert.AreEqual($"{objectCipher}КП.docx", fileName);
        }
    }
}
