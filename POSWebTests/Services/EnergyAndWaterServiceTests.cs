using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EnergyAndWaterLogic;
using POSCore.EnergyAndWaterLogic.Interfaces;
using POSCore.EstimateLogic;
using POSWeb.Services;
using POSWeb.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace POSWebTests.Services
{
    public class EnergyAndWaterServiceTests
    {
        private Mock<ICalendarPlanService> _calendarPlanServiceMock;
        private Mock<IEnergyAndWaterCreator> _energyAndWaterCreatorMock;
        private Mock<IEnergyAndWaterWriter> _energyAndWaterWriterMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private Mock<ICalendarWorkCreator> _calendarWorkCreatorMock;

        private EnergyAndWaterService CreateDefaultEnergyAndWaterService()
        {
            _calendarPlanServiceMock = new Mock<ICalendarPlanService>();
            _energyAndWaterCreatorMock = new Mock<IEnergyAndWaterCreator>();
            _energyAndWaterWriterMock = new Mock<IEnergyAndWaterWriter>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _calendarWorkCreatorMock = new Mock<ICalendarWorkCreator>();
            return new EnergyAndWaterService(_calendarPlanServiceMock.Object, _energyAndWaterCreatorMock.Object, _energyAndWaterWriterMock.Object,
                _webHostEnvironmentMock.Object, _calendarWorkCreatorMock.Object);
        }

        [Test]
        public void WriteEnergyAndWater()
        {
            var energyAndWaterService = CreateDefaultEnergyAndWaterService();
            var estimateFiles = new List<IFormFile>();
            var constractionStartDate = new DateTime(1999, 9, 21);
            var totalEstimateWork = new EstimateWork("", 0, 0, 0, 10);
            var estimate = new Estimate(new List<EstimateWork> { totalEstimateWork }, constractionStartDate, 0, "");
            _calendarPlanServiceMock.Setup(x => x.GetEstimate(estimateFiles)).Returns(estimate);
            var fileName = "EnergyAndWaterBGTGkss.docx";
            var totalCalendarWork = new CalendarWork("tarstr", (decimal)1.213, (decimal)2.234, null, 10);
            _calendarWorkCreatorMock.Setup(x => x.Create(constractionStartDate, totalEstimateWork)).Returns(totalCalendarWork);
            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("www");
            var energyAndWater = new EnergyAndWater(0, 0, 0, 0, 0, 0);
            _energyAndWaterCreatorMock.Setup(x => x.Create(totalCalendarWork.TotalCostIncludingContructionAndInstallationWorks, constractionStartDate.Year)).Returns(energyAndWater);

            energyAndWaterService.WriteEnergyAndWater(estimateFiles, constractionStartDate, fileName);

            _calendarPlanServiceMock.Verify(x => x.GetEstimate(estimateFiles), Times.Once);
            _calendarWorkCreatorMock.Verify(x => x.Create(constractionStartDate, totalEstimateWork), Times.Once);
            _energyAndWaterCreatorMock.Verify(x => x.Create(totalCalendarWork.TotalCostIncludingContructionAndInstallationWorks, constractionStartDate.Year), Times.Once);
            _energyAndWaterWriterMock.Verify(x => x.Write(energyAndWater, "www\\Templates\\EnergyAndWaterTemplate.docx", "www\\UsersFiles\\EnergyAndWaters", fileName), Times.Once);
        }

        public void GetEnergyAndWatersPath()
        {
            var energyAndWaterService = CreateDefaultEnergyAndWaterService();
            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("www");

            var energyAndWatersPath = energyAndWaterService.GetEnergyAndWatersPath();

            Assert.AreEqual("www\\EnergyAndWaters", energyAndWatersPath);
        }

        public void GetDownloadEnergyAndWaterName()
        {
            var energyAndWaterService = CreateDefaultEnergyAndWaterService();
            var objectCipher = "5.5-20.548";

            var energyAndWatersPath = energyAndWaterService.GetDownloadEnergyAndWaterName(objectCipher);

            Assert.AreEqual($"{objectCipher}ЭИВ.docx", energyAndWatersPath);
        }

        public void GetEstimate()
        {
            var energyAndWaterService = CreateDefaultEnergyAndWaterService();
            var estimateFiles = new List<IFormFile>();
            var expectedEstimate = new Estimate(null, default(DateTime), 0, string.Empty);
            _calendarPlanServiceMock.Setup(x => x.GetEstimate(estimateFiles)).Returns(expectedEstimate);

            var actualEstimate = energyAndWaterService.GetEstimate(estimateFiles);

            _calendarPlanServiceMock.Verify(x=>x.GetEstimate(estimateFiles), Times.Once());
            Assert.AreEqual(expectedEstimate, actualEstimate);
        }
    }
}
