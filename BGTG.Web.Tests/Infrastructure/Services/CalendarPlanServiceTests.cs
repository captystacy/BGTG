using System;
using System.Collections.Generic;
using AutoMapper;
using BGTG.POS.CalendarPlanTool;
using BGTG.POS.CalendarPlanTool.Interfaces;
using BGTG.POS.EstimateTool;
using BGTG.Web.Infrastructure.Services;
using BGTG.Web.Infrastructure.Services.Interfaces;
using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;
using BGTG.Web.ViewModels.POSViewModels.CalendarWorkViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace BGTG.Web.Tests.Infrastructure.Services
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
        public void GetCalendarPlanCreateViewModel()
        {
            var estimateFiles = new FormFileCollection();

            var calendarPlanPreCreateViewModel = new CalendarPlanPreCreateViewModel()
            {
                EstimateFiles = estimateFiles,
                TotalWorkChapter = TotalWorkChapter.TotalWork1To12Chapter
            };
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, 0, 0);
            var calendarPlanCreateViewModel = new CalendarPlanCreateViewModel()
            {
                CalendarWorkViewModels = new List<CalendarWorkViewModel>()
                {
                    new CalendarWorkViewModel()
                    {
                        WorkName = CalendarPlanInfo.TotalWorkName,
                        Chapter = (int)TotalWorkChapter.TotalWork1To12Chapter
                    },
                }
            };
            _estimateServiceMock.SetupGet(x => x.Estimate).Returns(estimate);
            _mapperMock.Setup(x => x.Map<CalendarPlanCreateViewModel>(estimate)).Returns(calendarPlanCreateViewModel);

            var expectedCalendarPlanCreateViewModel = new CalendarPlanCreateViewModel()
            {
                CalendarWorkViewModels = new List<CalendarWorkViewModel>()
                {
                    new CalendarWorkViewModel()
                    {
                        WorkName = CalendarPlanInfo.MainOtherExpensesWorkName,
                        Chapter = CalendarPlanInfo.MainOtherExpensesWorkChapter,
                        Percentages = new List<decimal>()
                    }
                }
            };

            var actualCalendarPlanCreateViewModel = _calendarPlanService.GetCalendarPlanCreateViewModel(calendarPlanPreCreateViewModel);

            _estimateServiceMock.Verify(x => x.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
            _mapperMock.Verify(x => x.Map<CalendarPlanCreateViewModel>(estimate), Times.Once);
            Assert.AreEqual(expectedCalendarPlanCreateViewModel, actualCalendarPlanCreateViewModel);
        }

        [Test]
        public void GetSavePath()
        {
            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
            var windowsName = "BGTG\\kss";

            var savePath = _calendarPlanService.GetSavePath(windowsName);

            _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
            Assert.AreEqual(@"wwwroot\AppData\UserFiles\CalendarPlanFiles\CalendarPlanBGTGkss.docx", savePath);
        }
    }
}
