using Moq;
using NUnit.Framework;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace POSCoreTests.EstimateConnetorTests
{
    public class ConnectShould
    {
        private IEstimateConnector _estimateConnector;
        private Mock<IEstimate> _estimateVatFree;
        private Mock<IEstimate> _estimateVat;

        private string[] _defaultEstimateWorksNames = new string[]
        {
            "ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА",
            "БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ",
            "ОРГАНИЗАЦИЯ ДОРОЖНОГО ДВИЖЕНИЯ НА ПЕРИОД СТРОИТЕЛЬСТВА",
            "ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%"
        };

        [SetUp]
        public void SetUp()
        {
            _estimateVatFree = new Mock<IEstimate>();
            _estimateVat = new Mock<IEstimate>();
            _estimateConnector = new EstimateConnector();
        }

        private IEnumerable<IEstimateWork> CreateDefaultEstimateWorks()
        {
            return _defaultEstimateWorksNames.Select(x => CreateEstimateWorkMock(x, 0, 0, 0).Object);
        }

        private Mock<IEstimateWork> CreateEstimateWorkMock(string workName, double estimateWorkTotalCost, double equipmentCost, double otherProductsCost)
        {
            var estimateWorkMock = new Mock<IEstimateWork>();

            estimateWorkMock.Setup(x => x.WorkName).Returns(workName);
            estimateWorkMock.Setup(x => x.TotalCost).Returns(estimateWorkTotalCost);
            estimateWorkMock.Setup(x => x.EquipmentCost).Returns(equipmentCost);
            estimateWorkMock.Setup(x => x.OtherProductsCost).Returns(otherProductsCost);

            return estimateWorkMock;
        }

        [Test]
        public void NotReturnNull()
        {
            var estimateWorks = CreateDefaultEstimateWorks();

            _estimateVatFree.Setup(x => x.EstimateWorks).Returns(estimateWorks);
            _estimateVat.Setup(x => x.EstimateWorks).Returns(estimateWorks);

            var estimate = _estimateConnector.Connect(_estimateVatFree.Object, _estimateVat.Object);

            Assert.NotNull(estimate);
        }

        [Test]
        public void ReturnEstimateWithNotNullEstimateWorks()
        {
            var estimateWorks = CreateDefaultEstimateWorks();

            _estimateVatFree.Setup(x => x.EstimateWorks).Returns(estimateWorks);
            _estimateVat.Setup(x => x.EstimateWorks).Returns(estimateWorks);

            var estimate = _estimateConnector.Connect(_estimateVatFree.Object, _estimateVat.Object);

            foreach (var estimateWork in estimate.EstimateWorks)
            {
                Assert.NotNull(estimateWork);
            }
        }

        [Test]
        public void NotContainRepetitiveWorks()
        {
            var estimateWorks = CreateDefaultEstimateWorks();

            _estimateVatFree.Setup(x => x.EstimateWorks).Returns(estimateWorks);
            _estimateVat.Setup(x => x.EstimateWorks).Returns(estimateWorks);

            var estimate = _estimateConnector.Connect(_estimateVatFree.Object, _estimateVat.Object);

            foreach (var estimateWork in estimate.EstimateWorks)
            {
                Assert.True(estimate.EstimateWorks.Count(x => x.WorkName == estimateWork.WorkName) == 1);
            }
        }

        [Test]
        public void SumEstimateWorkTotalCosts()
        {
            var workName = "ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА";

            var estimateWorkVatFreeTotalCost = 55.297;
            var estimateWorkVatFreeMock = CreateEstimateWorkMock(workName, estimateWorkVatFreeTotalCost, 0, 0);

            var estimateWorkVatTotalCost = 21.316;
            var estimateWorkVatMock = CreateEstimateWorkMock(workName, estimateWorkVatTotalCost, 0, 0);

            _estimateVatFree.Setup(x => x.EstimateWorks).Returns(new IEstimateWork[] { estimateWorkVatFreeMock.Object });
            _estimateVat.Setup(x => x.EstimateWorks).Returns(new IEstimateWork[] { estimateWorkVatMock.Object });

            var estimate = _estimateConnector.Connect(_estimateVatFree.Object, _estimateVat.Object);

            var estimateWorkTotalCost = estimate.EstimateWorks.First(x => x.WorkName == workName).TotalCost;

            Assert.Equals(estimateWorkTotalCost, estimateWorkVatFreeTotalCost + estimateWorkVatTotalCost);
        }

        [Test]
        public void SumEstimateWorkEquipmentCosts()
        {
            var workName = "ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА";

            var estimateWorkVatFreeEquipmentCost = 0.021;
            var estimateWorkVatFreeMock = CreateEstimateWorkMock(workName, 0, estimateWorkVatFreeEquipmentCost, 0);

            var estimateWorkVatEquipmentCost = 0.023;
            var estimateWorkVatMock = CreateEstimateWorkMock(workName, 0, estimateWorkVatEquipmentCost, 0);

            _estimateVatFree.Setup(x => x.EstimateWorks).Returns(new IEstimateWork[] { estimateWorkVatFreeMock.Object });
            _estimateVat.Setup(x => x.EstimateWorks).Returns(new IEstimateWork[] { estimateWorkVatMock.Object });

            var estimate = _estimateConnector.Connect(_estimateVatFree.Object, _estimateVat.Object);

            var estimateWorkEquipmentCost = estimate.EstimateWorks.First(x => x.WorkName == workName).EquipmentCost;

            Assert.Equals(estimateWorkEquipmentCost, estimateWorkVatFreeEquipmentCost + estimateWorkVatEquipmentCost);
        }

        [Test]
        public void SumEstimateWorkOtherProductsCosts()
        {
            var workName = "ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА";

            var estimateWorkVatFreeOtherProductsCost = 0.022;
            var estimateWorkVatFreeMock = CreateEstimateWorkMock(workName, 0, 0, estimateWorkVatFreeOtherProductsCost);

            var estimateWorkVatOtherProductsCost = 0.024;
            var estimateWorkVatMock = CreateEstimateWorkMock(workName, 0, 0, estimateWorkVatOtherProductsCost);

            _estimateVatFree.Setup(x => x.EstimateWorks).Returns(new IEstimateWork[] { estimateWorkVatFreeMock.Object });
            _estimateVat.Setup(x => x.EstimateWorks).Returns(new IEstimateWork[] { estimateWorkVatMock.Object });

            var estimate = _estimateConnector.Connect(_estimateVatFree.Object, _estimateVat.Object);

            var estimateWorkOtherPoductsCost = estimate.EstimateWorks.First(x => x.WorkName == workName).EquipmentCost;

            Assert.Equals(estimateWorkOtherPoductsCost, estimateWorkVatFreeOtherProductsCost + estimateWorkVatOtherProductsCost);
        }
    }
}
