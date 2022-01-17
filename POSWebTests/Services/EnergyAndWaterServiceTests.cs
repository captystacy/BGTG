using System;
using System.Collections.Generic;
using BGTGWeb.Services;
using BGTGWeb.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using POS.CalendarPlanLogic;
using POS.CalendarPlanLogic.Interfaces;
using POS.EnergyAndWaterLogic;
using POS.EnergyAndWaterLogic.Interfaces;
using POS.EstimateLogic;

namespace BGTGWebTests.Services
{
    public class EnergyAndWaterServiceTests
    {
        private EnergyAndWaterService _energyAndWaterService;
        private Mock<IEstimateService> _estimateServiceMock;
        private Mock<IEnergyAndWaterCreator> _energyAndWaterCreatorMock;
        private Mock<IEnergyAndWaterWriter> _energyAndWaterWriterMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private Mock<ICalendarWorkCreator> _calendarWorkCreatorMock;

        [SetUp]
        public void SetUp()
        {
            _estimateServiceMock = new Mock<IEstimateService>();
            _energyAndWaterCreatorMock = new Mock<IEnergyAndWaterCreator>();
            _energyAndWaterWriterMock = new Mock<IEnergyAndWaterWriter>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _calendarWorkCreatorMock = new Mock<ICalendarWorkCreator>();
            _energyAndWaterService = new EnergyAndWaterService(_estimateServiceMock.Object, _energyAndWaterCreatorMock.Object, _energyAndWaterWriterMock.Object,
                _webHostEnvironmentMock.Object, _calendarWorkCreatorMock.Object);

        }

        [Test]
        public void WriteEnergyAndWater()
        {
            var estimateFiles = new List<IFormFile>();
            var constructionStartDate = new DateTime(1999, 9, 21);
            var totalEstimateWork = new EstimateWork("", 0, 0, 0, 12);
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>() { totalEstimateWork }, constructionStartDate, 0, "", 0);
            var userFullName = "BGTG\\kss";
            var totalCalendarWork = new CalendarWork("tarstr", 1.213M, 2.234M, null, 10);
            _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);
            _calendarWorkCreatorMock.Setup(x => x.Create(totalEstimateWork, constructionStartDate)).Returns(totalCalendarWork);
            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("www");
            var energyAndWater = new EnergyAndWater(0, 0, 0, 0, 0, 0);
            _energyAndWaterCreatorMock.Setup(x => x.Create(totalCalendarWork.TotalCostIncludingCAIW, constructionStartDate.Year)).Returns(energyAndWater);

            _energyAndWaterService.WriteEnergyAndWater(estimateFiles, userFullName);

            _estimateServiceMock.Verify(x => x.ReadEstimateFiles(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Exactly(3));
            _calendarWorkCreatorMock.Verify(x => x.Create(totalEstimateWork, constructionStartDate), Times.Once);
            _energyAndWaterCreatorMock.Verify(x => x.Create(totalCalendarWork.TotalCostIncludingCAIW, constructionStartDate.Year), Times.Once);
            _energyAndWaterWriterMock.Verify(x => x.Write(energyAndWater, @"www\Templates\EnergyAndWaterTemplates\EnergyAndWaterTemplate.docx", @"www\UsersFiles\EnergyAndWaters\EnergyAndWaterBGTGkss.docx"), Times.Once);
        }

        public void GetEnergyAndWatersPath()
        {
            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("www");

            var energyAndWatersPath = _energyAndWaterService.GetEnergyAndWatersPath();

            Assert.AreEqual("www\\EnergyAndWaters", energyAndWatersPath);
        }

        public void GetEnergyAndWaterFileName()
        {
            var userFullName = "BGTG\\kss";

            var energyAndWaterFileName = _energyAndWaterService.GetEnergyAndWaterFileName(userFullName);

            Assert.AreEqual($"EnergyAndWaterBGTGkss.docx", energyAndWaterFileName);
        }

        public void GetDownloadEnergyAndWaterFileName()
        {
            var objectCipher = "5.5-20.548";
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, objectCipher, 0);
            _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);

            var downloadEnergyAndWaterFileName = _energyAndWaterService.GetDownloadEnergyAndWaterFileName();

            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
            Assert.AreEqual($"{objectCipher}ЭИВ.docx", downloadEnergyAndWaterFileName);
        }
    }
}
