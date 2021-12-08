using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using POSCore.EstimateLogic;
using POSWeb.Models;
using POSWeb.Presentations;
using POSWeb.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace POSWebTests.Presentations
{
    public class EnergyAndWaterPresentationTests
    {
        private Mock<IEnergyAndWaterService> _energyAndWaterServiceMock;
        private Mock<IMapper> _mapperMock;

        private EnergyAndWaterPresentation CreateDefaultEnergyAndWaterPresentation()
        {
            _energyAndWaterServiceMock = new Mock<IEnergyAndWaterService>();
            _mapperMock = new Mock<IMapper>();
            return new EnergyAndWaterPresentation(_energyAndWaterServiceMock.Object, _mapperMock.Object);
        }

        [Test]
        public void WriteEnergyAndWater()
        {
            var energyAndWaterPresentation = CreateDefaultEnergyAndWaterPresentation();
            var estimateFiles = new List<IFormFile>();
            var userFullName = "BGTG\\kss";
            var constructionStartDate = new DateTime(199, 9, 21);

            energyAndWaterPresentation.WriteEnergyAndWater(estimateFiles, constructionStartDate, userFullName);

            _energyAndWaterServiceMock.Verify(x => x.WriteEnergyAndWater(estimateFiles, constructionStartDate, "EnergyAndWaterBGTGkss.docx"), Times.Once);
        }

        [Test]
        public void GetEnergyAndWaterFileName()
        {
            var energyAndWaterPresentation = CreateDefaultEnergyAndWaterPresentation();
            var userFullName = "BGTG\\kss";

            var energyAndWaterFileName = energyAndWaterPresentation.GetEnergyAndWaterFileName(userFullName);

            Assert.AreEqual("EnergyAndWaterBGTGkss.docx", energyAndWaterFileName);
        }

        [Test]
        public void GetDownloadEnergyAndWaterName()
        {
            var energyAndWaterPresentation = CreateDefaultEnergyAndWaterPresentation();
            var objectCipher = "5.5-20.548";

            var downloadEnergyAndWaterName = energyAndWaterPresentation.GetDownloadEnergyAndWaterName(objectCipher);

            _energyAndWaterServiceMock.Verify(x => x.GetDownloadEnergyAndWaterName(objectCipher), Times.Once);
        }

        [Test]
        public void GetEnergyAndWatersPath()
        {
            var energyAndWaterPresentation = CreateDefaultEnergyAndWaterPresentation();

            var downloadEnergyAndWaterName = energyAndWaterPresentation.GetEnergyAndWatersPath();

            _energyAndWaterServiceMock.Verify(x => x.GetEnergyAndWatersPath(), Times.Once);
        }

        [Test]
        public void GetEnergyAndWaterVM()
        {
            var energyAndWaterPresentation = CreateDefaultEnergyAndWaterPresentation();
            var estimateFiles = new List<IFormFile>();
            var estimate = new Estimate(null, default(DateTime), 0, "");
            _energyAndWaterServiceMock.Setup(x => x.GetEstimate(estimateFiles)).Returns(estimate);

            var downloadEnergyAndWaterName = energyAndWaterPresentation.GetEnergyAndWaterVM(estimateFiles);

            _energyAndWaterServiceMock.Verify(x => x.GetEstimate(estimateFiles), Times.Once);
            _mapperMock.Verify(x => x.Map<EnergyAndWaterVM>(estimate), Times.Once);
        }
    }
}
