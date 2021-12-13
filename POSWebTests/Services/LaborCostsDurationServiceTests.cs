using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using POSCore.DurationLogic.LaborCosts;
using POSCore.DurationLogic.LaborCosts.Interfaces;
using POSCore.EstimateLogic;
using POSWeb.Models;
using POSWeb.Services;
using POSWeb.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace POSWebTests.Services
{
    public class LaborCostsDurationServiceTests
    {
        private Mock<IEstimateService> _estimateServiceMock;
        private Mock<ILaborCostsDurationCreator> _laborCostsDurationCreatorMock;
        private Mock<ILaborCostsDurationWriter> _laborCostsDurationWriterMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;

        private LaborCostsDurationService CreateDefaultLaborCostsDurationService()
        {
            _estimateServiceMock = new Mock<IEstimateService>();
            _laborCostsDurationCreatorMock = new Mock<ILaborCostsDurationCreator>();
            _laborCostsDurationWriterMock = new Mock<ILaborCostsDurationWriter>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            return new LaborCostsDurationService(_estimateServiceMock.Object, _laborCostsDurationCreatorMock.Object,
                _laborCostsDurationWriterMock.Object, _webHostEnvironmentMock.Object);
        }

        [Test]
        public void WriteLaborCostsDuration()
        {
            var laborCostsDurationService = CreateDefaultLaborCostsDurationService();
            var estimateFiles = new List<IFormFile>();
            var laborCostsDurationVM = new LaborCostsDurationVM()
            {
                AcceptanceTimeIncluded = true,
                NumberOfEmployees = 4,
                NumberOfWorkingDays = 21.5M,
                Shift = 1.5M,
                WorkingDayDuration = 8,
                TechnologicalLaborCosts = 110,
            };
            var userFullName = "BGTG\\kss";
            var estimate = new Estimate(null, null, default(DateTime), 0, null, 100);
            _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);
            var laborCostsDuration = new LaborCostsDuration(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true, true);
            _laborCostsDurationCreatorMock.Setup(x => x.Create(estimate.LaborCosts + laborCostsDurationVM.TechnologicalLaborCosts,
                laborCostsDurationVM.WorkingDayDuration, laborCostsDurationVM.Shift, laborCostsDurationVM.NumberOfWorkingDays,
                laborCostsDurationVM.NumberOfEmployees, laborCostsDurationVM.AcceptanceTimeIncluded)).Returns(laborCostsDuration);
            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("wwwroot");

            laborCostsDurationService.WriteLaborCostsDuration(estimateFiles, laborCostsDurationVM, userFullName);

            _estimateServiceMock.Verify(x => x.ReadEstimateFiles(estimateFiles), Times.Once);
            _laborCostsDurationCreatorMock.Verify(x => x.Create(estimate.LaborCosts + laborCostsDurationVM.TechnologicalLaborCosts,
                laborCostsDurationVM.WorkingDayDuration, laborCostsDurationVM.Shift, laborCostsDurationVM.NumberOfWorkingDays,
                laborCostsDurationVM.NumberOfEmployees, laborCostsDurationVM.AcceptanceTimeIncluded), Times.Once);
            _laborCostsDurationWriterMock.Verify(x => x.Write(laborCostsDuration,
                @"wwwroot\Templates\LaborCostsDurationTemplates\Rounding+Acceptance+Techcosts+Template.docx",
                @"wwwroot\UsersFiles\LaborCostsDurations", "LaborCostsDurationBGTGkss.docx"), Times.Once);
            _webHostEnvironmentMock.VerifyGet(x => x.WebRootPath, Times.Exactly(2));
            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
        }


        [Test]
        public void GetLaborCostsDurationsPath()
        {
            var laborCostsDurationService = CreateDefaultLaborCostsDurationService();
            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("wwwroot");
            var expectedLaborCostsDurationPath = @"wwwroot\UsersFiles\LaborCostsDurations";

            var actualLaborCostsDurationsPath = laborCostsDurationService.GetLaborCostsDurationsPath();

            _webHostEnvironmentMock.VerifyGet(x => x.WebRootPath, Times.Once);
            Assert.AreEqual(expectedLaborCostsDurationPath, actualLaborCostsDurationsPath);
        }

        [Test]
        public void GetLaborCostsDurationFileName()
        {
            var laborCostsDurationService = CreateDefaultLaborCostsDurationService();
            var userFullName = "BGTG\\kss";

            var laborCostsDurationFilName = laborCostsDurationService.GetLaborCostsDurationFileName(userFullName);

            Assert.AreEqual("LaborCostsDurationBGTGkss.docx", laborCostsDurationFilName);
        }

        [Test]
        public void GetDownloadLaborCostsDurationFileName()
        {
            var laborCostsDurationService = CreateDefaultLaborCostsDurationService();
            var objectCipher = "cipher";
            var estimate = new Estimate(null, null, default(DateTime), 0, objectCipher, 0);
            _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);

            var downloadLaborCostsDurationFilName = laborCostsDurationService.GetDownloadLaborCostsDurationFileName();

            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
            Assert.AreEqual($"{objectCipher}ПРОД.docx", downloadLaborCostsDurationFilName);
        }
    }
}
