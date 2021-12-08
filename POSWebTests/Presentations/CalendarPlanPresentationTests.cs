using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using POSCore.EstimateLogic;
using POSWeb.Models;
using POSWeb.Presentations;
using POSWeb.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace POSWebTests.Presentations
{
    public class CalendarPlanPresentationTests
    {
        private Mock<ICalendarPlanService> _calendarPlanServiceMock;
        private Mock<IMapper> _mapperMock;

        private CalendarPlanPresentation CreateDefaultCalendarPlanPresentation()
        {
            _calendarPlanServiceMock = new Mock<ICalendarPlanService>();
            _mapperMock = new Mock<IMapper>();
            return new CalendarPlanPresentation(_calendarPlanServiceMock.Object, _mapperMock.Object);
        }

        [Test]
        public void WriteCalendarPlan()
        {
            var calendarPlanPresentation = CreateDefaultCalendarPlanPresentation();
            var estimateFiles = new List<IFormFile>();
            var calendarPlanVM = new CalendarPlanVM();
            var userFullName = "BGTG\\kss";

            calendarPlanPresentation.WriteCalendarPlan(estimateFiles, calendarPlanVM, userFullName);

            _calendarPlanServiceMock.Verify(x => x.WriteCalendarPlan(estimateFiles, calendarPlanVM, "CalendarPlanBGTGkss.docx"), Times.Once);
        }

        [Test]
        public void GetCalendarPlanVM()
        {
            var calendarPlanPresentation = CreateDefaultCalendarPlanPresentation();
            var estimateFiles = new List<IFormFile>();
            var estimate = new Estimate(new List<EstimateWork>(), DateTime.Today, 0, "");
            _calendarPlanServiceMock.Setup(x => x.GetEstimate(estimateFiles)).Returns(estimate);
            var userWorks = new List<UserWork>()
            {
                new UserWork() { WorkName = "", Chapter = 1 },
                new UserWork() { WorkName = "", Chapter = 10 },
                new UserWork() { WorkName = "ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 111111" },
            };
            var calendarPlanVM = new CalendarPlanVM()
            {
                UserWorks = userWorks
            };
            _mapperMock.Setup(x => x.Map<CalendarPlanVM>(estimate)).Returns(calendarPlanVM);

            var result = calendarPlanPresentation.GetCalendarPlanVM(estimateFiles);

            _calendarPlanServiceMock.Verify(x => x.GetEstimate(estimateFiles), Times.Once);
            _mapperMock.Verify(x => x.Map<CalendarPlanVM>(estimate), Times.Once);

            Assert.IsEmpty(result.UserWorks.FindAll(x =>
                x.Chapter == 1
                || x.WorkName.StartsWith(CalendarPlanSeparator.TemporaryBuildingsSearchPattern)
                || x.Chapter == 10));

            Assert.NotNull(result.UserWorks.Find(x =>
                x.WorkName == CalendarPlanSeparator.MainOtherExpensesWork
                && x.Chapter == 9));

            Assert.AreSame(calendarPlanVM, result);
        }

        [Test]
        public void GetMainTotalWork()
        {
            var calendarPlanPresentation = CreateDefaultCalendarPlanPresentation();
            var estimateFiles = new List<IFormFile>();
            var calendarPlanVM = new CalendarPlanVM();
            var mainTotalWork = new CalendarWork("", 0, 0, null, 0);
            var userWork = new UserWork();
            _calendarPlanServiceMock.Setup(x => x.GetMainTotalWork(estimateFiles, calendarPlanVM)).Returns(mainTotalWork);
            _mapperMock.Setup(x => x.Map<UserWork>(mainTotalWork)).Returns(userWork);

            var result = calendarPlanPresentation.GetMainTotalWork(estimateFiles, calendarPlanVM);

            _calendarPlanServiceMock.Verify(x => x.GetMainTotalWork(estimateFiles, calendarPlanVM), Times.Once);
            _mapperMock.Verify(x => x.Map<UserWork>(mainTotalWork), Times.Once);
            Assert.AreSame(userWork, result);
        }


        [Test]
        public void GetCalendarPlanFileName()
        {
            var calendarPlanPresentation = CreateDefaultCalendarPlanPresentation();
            var userFullName = "BGTG\\kss";

            var calendarPlanFileName = calendarPlanPresentation.GetCalendarPlanFileName(userFullName);

            Assert.AreEqual("CalendarPlanBGTGkss.docx", calendarPlanFileName);
        }

        [Test]
        public void GetDownloadCalendarPlanName()
        {
            var calendarPlanPresentation = CreateDefaultCalendarPlanPresentation();
            var objectCipher = "5.5-20.548";

            calendarPlanPresentation.GetDownloadCalendarPlanName(objectCipher);

            _calendarPlanServiceMock.Verify(x => x.GetDownloadCalendarPlanName(objectCipher), Times.Once);
        }


        [Test]
        public void GetCalendarPlanPath()
        {
            var calendarPlanPresentation = CreateDefaultCalendarPlanPresentation();

            calendarPlanPresentation.GetCalendarPlansPath();

            _calendarPlanServiceMock.Verify(x=>x.GetCalendarPlansPath(), Times.Once);
        }
    }
}
