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
        private Mock<IEstimateService> _estimateServiceMock;
        private Mock<IEnergyAndWaterCreator> _energyAndWaterCreatorMock;
        private Mock<IEnergyAndWaterWriter> _energyAndWaterWriterMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private Mock<ICalendarWorkCreator> _calendarWorkCreatorMock;

        private EnergyAndWaterService CreateDefaultEnergyAndWaterService()
        {
            _estimateServiceMock = new Mock<IEstimateService>();
            _energyAndWaterCreatorMock = new Mock<IEnergyAndWaterCreator>();
            _energyAndWaterWriterMock = new Mock<IEnergyAndWaterWriter>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _calendarWorkCreatorMock = new Mock<ICalendarWorkCreator>();
            return new EnergyAndWaterService(_estimateServiceMock.Object, _energyAndWaterCreatorMock.Object, _energyAndWaterWriterMock.Object,
                _webHostEnvironmentMock.Object, _calendarWorkCreatorMock.Object);
        }

        [Test]
        public void WriteEnergyAndWater()
        {
            var energyAndWaterService = CreateDefaultEnergyAndWaterService();
            var estimateFiles = new List<IFormFile>();
            var constractionStartDate = new DateTime(1999, 9, 21);
            var totalEstimateWork = new EstimateWork("", 0, 0, 0, 10);
            var estimate = new Estimate(null, new List<EstimateWork> { totalEstimateWork }, constractionStartDate, 0, "", 0);
            var userFullName = "BGTG\\kss";
            var totalCalendarWork = new CalendarWork("tarstr", 1.213M, 2.234M, null, 10);
            _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);
            _calendarWorkCreatorMock.Setup(x => x.Create(totalEstimateWork, constractionStartDate)).Returns(totalCalendarWork);
            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("www");
            var energyAndWater = new EnergyAndWater(0, 0, 0, 0, 0, 0);
            _energyAndWaterCreatorMock.Setup(x => x.Create(totalCalendarWork.TotalCostIncludingContructionAndInstallationWorks, constractionStartDate.Year)).Returns(energyAndWater);

            energyAndWaterService.WriteEnergyAndWater(estimateFiles, userFullName);

            _estimateServiceMock.Verify(x => x.ReadEstimateFiles(estimateFiles), Times.Once);
            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Exactly(3));
            _calendarWorkCreatorMock.Verify(x => x.Create(totalEstimateWork, constractionStartDate), Times.Once);
            _energyAndWaterCreatorMock.Verify(x => x.Create(totalCalendarWork.TotalCostIncludingContructionAndInstallationWorks, constractionStartDate.Year), Times.Once);
            _energyAndWaterWriterMock.Verify(x => x.Write(energyAndWater, "www\\Templates\\EnergyAndWaterTemplate.docx", "www\\UsersFiles\\EnergyAndWaters", "EnergyAndWaterBGTGkss.docx"), Times.Once);
        }

        public void GetEnergyAndWatersPath()
        {
            var energyAndWaterService = CreateDefaultEnergyAndWaterService();
            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("www");

            var energyAndWatersPath = energyAndWaterService.GetEnergyAndWatersPath();

            Assert.AreEqual("www\\EnergyAndWaters", energyAndWatersPath);
        }

        public void GetEnergyAndWaterFileName()
        {
            var energyAndWaterService = CreateDefaultEnergyAndWaterService();
            var userFullName = "BGTG\\kss";

            var energyAndWaterFileName = energyAndWaterService.GetEnergyAndWaterFileName(userFullName);

            Assert.AreEqual($"EnergyAndWaterBGTGkss.docx", energyAndWaterFileName);
        }

        public void GetDownloadEnergyAndWaterFileName()
        {
            var energyAndWaterService = CreateDefaultEnergyAndWaterService();
            var objectCipher = "5.5-20.548";
            var estimate = new Estimate(null, null, default(DateTime), 0, objectCipher, 0);
            _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);

            var downloadEnergyAndWaterFileName = energyAndWaterService.GetDownloadEnergyAndWaterFileName();

            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
            Assert.AreEqual($"{objectCipher}ЭИВ.docx", downloadEnergyAndWaterFileName);
        }
    }
}
