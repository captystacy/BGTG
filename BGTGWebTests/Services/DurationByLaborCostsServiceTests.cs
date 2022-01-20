using System;
using System.Collections.Generic;
using BGTGWeb.Models;
using BGTGWeb.Services;
using BGTGWeb.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using POS.DurationLogic.DurationByLaborCosts;
using POS.DurationLogic.DurationByLaborCosts.Interfaces;
using POS.EstimateLogic;

namespace BGTGWebTests.Services
{
    public class DurationByLaborCostsServiceTests
    {
        private DurationByLaborCostsService _durationByLaborCostsService;
        private Mock<IEstimateService> _estimateServiceMock;
        private Mock<IDurationByLaborCostsCreator> _durationByLaborCostsCreatorMock;
        private Mock<IDurationByLaborCostsWriter> _durationByLaborCostsWriterMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;

        [SetUp]
        public void SetUp()
        {
            _estimateServiceMock = new Mock<IEstimateService>();
            _durationByLaborCostsCreatorMock = new Mock<IDurationByLaborCostsCreator>();
            _durationByLaborCostsWriterMock = new Mock<IDurationByLaborCostsWriter>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _durationByLaborCostsService = new DurationByLaborCostsService(_estimateServiceMock.Object, _durationByLaborCostsCreatorMock.Object,
                _durationByLaborCostsWriterMock.Object, _webHostEnvironmentMock.Object);

        }

        [Test]
        public void Write()
        {
            var estimateFiles = new List<IFormFile>();
            var durationByLaborCostsVM = new DurationByLaborCostsVM()
            {
                AcceptanceTimeIncluded = true,
                NumberOfEmployees = 4,
                NumberOfWorkingDays = 21.5M,
                Shift = 1.5M,
                WorkingDayDuration = 8,
                TechnologicalLaborCosts = 110,
            };
            var userFullName = "BGTG\\kss";
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, 0, "", 100);
            _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);
            var durationByLaborCosts = new DurationByLaborCosts(0, 0, 0, durationByLaborCostsVM.TechnologicalLaborCosts,
                0, 0, 0, 0, 0, 0, 0, 0, true, true);


            _durationByLaborCostsCreatorMock.Setup(x => x.Create(estimate.LaborCosts,
                    durationByLaborCostsVM.TechnologicalLaborCosts,
                    durationByLaborCostsVM.WorkingDayDuration, durationByLaborCostsVM.Shift,
                    durationByLaborCostsVM.NumberOfWorkingDays,
                    durationByLaborCostsVM.NumberOfEmployees, durationByLaborCostsVM.AcceptanceTimeIncluded))
                .Returns(durationByLaborCosts);

            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("wwwroot");

            _durationByLaborCostsService.Write(estimateFiles, durationByLaborCostsVM, userFullName);

            _estimateServiceMock.Verify(x => x.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter), Times.Once);
            _durationByLaborCostsCreatorMock.Verify(x => x.Create(estimate.LaborCosts,
                durationByLaborCostsVM.TechnologicalLaborCosts,
                durationByLaborCostsVM.WorkingDayDuration, durationByLaborCostsVM.Shift,
                durationByLaborCostsVM.NumberOfWorkingDays,
                durationByLaborCostsVM.NumberOfEmployees, durationByLaborCostsVM.AcceptanceTimeIncluded), Times.Once);
            _durationByLaborCostsWriterMock.Verify(x => x.Write(durationByLaborCosts,
                @"wwwroot\Templates\DurationByLaborCostsTemplates\Rounding+Acceptance+Template.docx",
                @"wwwroot\UsersFiles\DurationByLaborCosts\DurationByLaborCostsBGTGkss.docx"), Times.Once);
            _webHostEnvironmentMock.VerifyGet(x => x.WebRootPath, Times.Exactly(2));
            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
        }


        [Test]
        public void GetSavePath()
        {
            _webHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("wwwroot");
            var userFullName = "BGTG\\kss";

            var savePath = _durationByLaborCostsService.GetSavePath(userFullName);

            _webHostEnvironmentMock.VerifyGet(x => x.WebRootPath, Times.Once);
            Assert.AreEqual(@"wwwroot\UsersFiles\DurationByLaborCosts\DurationByLaborCostsBGTGkss.docx", savePath);
        }

        [Test]
        public void GetFileName()
        {
            var objectCipher = "5.5-20.548";
            var estimate = new Estimate(new List<EstimateWork>(), new List<EstimateWork>(), DateTime.Today, 0, 0, objectCipher, 0);
            _estimateServiceMock.Setup(x => x.Estimate).Returns(estimate);

            var fileName = _durationByLaborCostsService.GetFileName();

            _estimateServiceMock.VerifyGet(x => x.Estimate, Times.Once);
            Assert.AreEqual($"{objectCipher}ПРОД.docx", fileName);
        }
    }
}
