using System;
using System.Collections.Generic;
using BGTG.POS.Tools.CalendarPlanTool;
using BGTG.POS.Tools.CalendarPlanTool.Interfaces;
using BGTG.POS.Tools.EnergyAndWaterTool;
using BGTG.POS.Tools.EnergyAndWaterTool.Interfaces;
using BGTG.POS.Tools.EstimateTool;
using BGTG.POS.Web.Infrastructure.Services;
using BGTG.POS.Web.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace BGTG.POS.Web.Tests.Infrastructure.Services
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
        public void Write()
        {
            var estimateFiles = new FormFileCollection();
            var constructionStartDate = new DateTime(1999, 9, 21);
            var totalEstimateWork = new EstimateWork("", 0, 0, 0, 12);
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>() { totalEstimateWork }, constructionStartDate, 0, 0, "", 0);
            var userFullName = "BGTG\\kss";
            var totalCalendarWork = new CalendarWork("tarstr", 1.213M, 2.234M, null, 10);
            _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);
            _calendarWorkCreatorMock.Setup(x => x.Create(totalEstimateWork, constructionStartDate)).Returns(totalCalendarWork);
            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
            var energyAndWater = new EnergyAndWater(0, 0, 0, 0, 0, 0);
            _energyAndWaterCreatorMock.Setup(x => x.Create(totalCalendarWork.TotalCostIncludingCAIW, constructionStartDate.Year)).Returns(energyAndWater);

            _energyAndWaterService.Write(estimateFiles, userFullName);

            _webHostEnvironmentMock.Verify(x => x.ContentRootPath, Times.Exactly(2));
            _estimateServiceMock.Verify(x => x.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Exactly(3));
            _calendarWorkCreatorMock.Verify(x => x.Create(totalEstimateWork, constructionStartDate), Times.Once);
            _energyAndWaterCreatorMock.Verify(x => x.Create(totalCalendarWork.TotalCostIncludingCAIW, constructionStartDate.Year), Times.Once);
            _energyAndWaterWriterMock.Verify(
                x => x.Write(energyAndWater,
                    @"wwwroot\AppData\Templates\EnergyAndWaterTemplates\EnergyAndWaterTemplate.docx",
                    @"wwwroot\AppData\UserFiles\EnergyAndWaterFiles\EnergyAndWaterBGTGkss.docx"), Times.Once);

        }

        [Test]
        public void GetSavePath()
        {
            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
            var userFullName = "BGTG\\kss";

            var savePath = _energyAndWaterService.GetSavePath(userFullName);

            _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
            Assert.AreEqual(@"wwwroot\AppData\UserFiles\EnergyAndWaterFiles\EnergyAndWaterBGTGkss.docx", savePath);
        }

        [Test]
        public void GetFileName()
        {
            var objectCipher = "5.5-20.548";
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, 0, objectCipher, 0);
            _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);

            var fileName = _energyAndWaterService.GetFileName();

            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
            Assert.AreEqual($"{objectCipher}ЭИВ.docx", fileName);
        }
    }
}
