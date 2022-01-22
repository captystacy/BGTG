using System;
using System.Collections.Generic;
using AutoMapper;
using BGTG.POS.Tools.CalendarPlanTool;
using BGTG.POS.Tools.CalendarPlanTool.Interfaces;
using BGTG.POS.Tools.EstimateTool;
using BGTG.POS.Web.Infrastructure.Services;
using BGTG.POS.Web.Infrastructure.Services.Interfaces;
using BGTG.POS.Web.ViewModels.CalendarPlanViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace BGTG.POS.Web.Tests.Infrastructure.Services
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
        public void GetCalendarPlanViewModel()
        {
            var estimateFiles = new FormFileCollection();
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, 0, "", 0);
            var calendarPlanViewModel = new CalendarPlanViewModel()
            {
                UserWorks = new List<UserWorkViewModel>()
                {
                    new UserWorkViewModel()
                    {
                        WorkName = CalendarPlanInfo.TotalWorkName,
                        Chapter = (int)TotalWorkChapter.TotalWork1To12Chapter
                    },
                }
            };
            _estimateServiceMock.SetupGet(x => x.Estimate).Returns(estimate);
            _mapperMock.Setup(x => x.Map<CalendarPlanViewModel>(estimate)).Returns(calendarPlanViewModel);

            var expectedCalendarPlanViewModel = new CalendarPlanViewModel()
            {
                UserWorks = new List<UserWorkViewModel>()
                {
                    new UserWorkViewModel()
                    {
                        WorkName = CalendarPlanInfo.MainOtherExpensesWorkName,
                        Chapter = CalendarPlanInfo.MainOtherExpensesWorkChapter,
                        Percentages = new List<decimal>()
                    }
                }
            };

            var actualCalendarPlanViewModel = _calendarPlanService.GetCalendarPlanViewModel(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter);

            _estimateServiceMock.Verify(x => x.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
            _mapperMock.Verify(x => x.Map<CalendarPlanViewModel>(estimate), Times.Once);
            Assert.AreEqual(expectedCalendarPlanViewModel, actualCalendarPlanViewModel);
        }

        [Test]
        public void GetSavePath()
        {
            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
            var userFullName = "BGTG\\kss";

            var savePath = _calendarPlanService.GetSavePath(userFullName);

            _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
            Assert.AreEqual(@"wwwroot\AppData\UserFiles\CalendarPlanFiles\CalendarPlanBGTGkss.docx", savePath);
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
