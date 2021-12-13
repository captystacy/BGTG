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
            var userWorks = new List<UserWork>();
            var userFullName = "BGTG\\kss";

            calendarPlanPresentation.WriteCalendarPlan(estimateFiles, userWorks, userFullName);

            _calendarPlanServiceMock.Verify(x => x.WriteCalendarPlan(estimateFiles, userWorks, userFullName), Times.Once);
        }

        [Test]
        public void GetCalendarPlanVM()
        {
            var calendarPlanPresentation = CreateDefaultCalendarPlanPresentation();
            var estimateFiles = new List<IFormFile>();
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, "", 0);
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

            Assert.NotNull(result.UserWorks.Find(x =>
                x.WorkName == CalendarWorkCreator.MainOtherExpensesWorkName
                && x.Chapter == 9));

            Assert.AreSame(calendarPlanVM, result);
        }

        [Test]
        public void GetTotalPercentages()
        {
            var calendarPlanPresentation = CreateDefaultCalendarPlanPresentation();
            var estimateFiles = new List<IFormFile>();
            var percentages = new List<decimal>();
            var userWorks = new List<UserWork>();
            _calendarPlanServiceMock.Setup(x => x.GetTotalPercentages(estimateFiles, userWorks)).Returns(percentages);

            var result = calendarPlanPresentation.GetTotalPercentages(estimateFiles, userWorks);

            _calendarPlanServiceMock.Verify(x => x.GetTotalPercentages(estimateFiles, userWorks), Times.Once);
            Assert.AreEqual(percentages, result);
        }


        [Test]
        public void GetCalendarPlanFileName()
        {
            var calendarPlanPresentation = CreateDefaultCalendarPlanPresentation();
            var userFullName = "BGTG\\kss";
            var fileName = "fileName";
            _calendarPlanServiceMock.Setup(x => x.GetCalendarPlanFileName(userFullName)).Returns(fileName);

            var calendarPlanFileName = calendarPlanPresentation.GetCalendarPlanFileName(userFullName);

            _calendarPlanServiceMock.Verify(x=>x.GetCalendarPlanFileName(userFullName), Times.Once);
            Assert.AreEqual(fileName, calendarPlanFileName);
        }

        [Test]
        public void GetDownloadCalendarPlanName()
        {
            var calendarPlanPresentation = CreateDefaultCalendarPlanPresentation();
            var fileName = "fileName";
            _calendarPlanServiceMock.Setup(x => x.GetDownloadCalendarPlanFileName()).Returns(fileName);

            var downloadCalendarPlanName = calendarPlanPresentation.GetDownloadCalendarPlanFileName();

            _calendarPlanServiceMock.Verify(x => x.GetDownloadCalendarPlanFileName(), Times.Once);
            Assert.AreEqual(fileName, downloadCalendarPlanName);
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
