using NUnit.Framework;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace POSCoreTests.EstimateLogic.EstimateConnetorTests
{
    public class ConnectShould
    {
        private IEstimateConnector _estimateConnector;

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
            _estimateConnector = new EstimateConnector();
        }

        private IEnumerable<EstimateWork> CreateDefaultEstimateWorks()
        {
            return _defaultEstimateWorksNames.Select(x => new EstimateWork(x, 0, 0, 0, 0));
        }

        [Test]
        public void BeInOrder()
        {
            var estimateWorksVatFree = new EstimateWork[]
            {
                new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, 0, 0),
                new EstimateWork("БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ", 0, 0, 0, 0),
                new EstimateWork("ОРГАНИЗАЦИЯ ДОРОЖНОГО ДВИЖЕНИЯ НА ПЕРИОД СТРОИТЕЛЬСТВА", 0, 0, 0, 0),
                new EstimateWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%", 0, 0, 0, 0),
            };
            var estimateVatFree = new Estimate(estimateWorksVatFree);

            var estimateWorksVat = new EstimateWork[]
            {
                new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, 0, 0),
                new EstimateWork("ШРП", 0, 0, 0, 0),
                new EstimateWork("БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ", 0, 0, 0, 0),
                new EstimateWork("ГРП", 0, 0, 0, 0),
                new EstimateWork("ОРГАНИЗАЦИЯ ДОРОЖНОГО ДВИЖЕНИЯ НА ПЕРИОД СТРОИТЕЛЬСТВА", 0, 0, 0, 0),
                new EstimateWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%", 0, 0, 0, 0),
            };
            var estimateVat = new Estimate(estimateWorksVat);

            var expectedOrder = new EstimateWork[]
            {
                new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, 0, 0),
                new EstimateWork("БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ", 0, 0, 0, 0),
                new EstimateWork("ШРП", 0, 0, 0, 0),
                new EstimateWork("ОРГАНИЗАЦИЯ ДОРОЖНОГО ДВИЖЕНИЯ НА ПЕРИОД СТРОИТЕЛЬСТВА", 0, 0, 0, 0),
                new EstimateWork("ГРП", 0, 0, 0, 0),
                new EstimateWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%", 0, 0, 0, 0),
            };

            var estimate = _estimateConnector.Connect(estimateVatFree, estimateVat);

            var estimateWorks = estimate.EstimateWorks.ToArray();
            for (int i = 0; i < expectedOrder.Length; i++)
            {
                Assert.AreEqual(expectedOrder[i].WorkName, estimateWorks[i].WorkName);
            }
        }

        [Test]
        public void NotReturnNull()
        {
            var estimateWorks = CreateDefaultEstimateWorks();

            var estimateVatFree = new Estimate(estimateWorks);
            var estimateVat = new Estimate(estimateWorks);

            var estimate = _estimateConnector.Connect(estimateVatFree, estimateVat);

            Assert.NotNull(estimate);
        }

        [Test]
        public void ReturnEstimateWithNotNullEstimateWorks()
        {
            var estimateWorks = CreateDefaultEstimateWorks();

            var estimateVatFree = new Estimate(estimateWorks);
            var estimateVat = new Estimate(estimateWorks);

            var estimate = _estimateConnector.Connect(estimateVatFree, estimateVat);

            foreach (var estimateWork in estimate.EstimateWorks)
            {
                Assert.NotNull(estimateWork);
            }
        }

        [Test]
        public void NotContainRepetitiveWorks()
        {
            var estimateWorks = CreateDefaultEstimateWorks();

            var estimateVatFree = new Estimate(estimateWorks);
            var estimateVat = new Estimate(estimateWorks);

            var estimate = _estimateConnector.Connect(estimateVatFree, estimateVat);

            foreach (var estimateWork in estimate.EstimateWorks)
            {
                Assert.True(estimate.EstimateWorks.Count(x => x.WorkName == estimateWork.WorkName) == 1);
            }
        }

        private Estimate CreateEstimateWithOneEstimateWork(string workName, double equipmentCost, double otherProductsCost, double totalCost, int chapter)
        {
            var estimateWork = new EstimateWork(workName, equipmentCost, otherProductsCost, totalCost, chapter);
            var estimateWorks = new EstimateWork[] { estimateWork };
            return new Estimate(estimateWorks);
        }

        [Test]
        public void SumEstimateWorkEquipmentCosts()
        {
            var workName = "ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА";

            var estimateWorkVatFreeEquipmentCost = 0.021;
            var estimateVatFree = CreateEstimateWithOneEstimateWork(workName, estimateWorkVatFreeEquipmentCost, 0, 0, 0);

            var estimateWorkVatEquipmentCost = 0.023;
            var estimateVat = CreateEstimateWithOneEstimateWork(workName, estimateWorkVatEquipmentCost, 0, 0, 0);

            var estimate = _estimateConnector.Connect(estimateVatFree, estimateVat);

            var estimateWorkEquipmentCost = estimate.EstimateWorks.First(x => x.WorkName == workName).EquipmentCost;

            Assert.AreEqual(estimateWorkVatFreeEquipmentCost + estimateWorkVatEquipmentCost, estimateWorkEquipmentCost);
        }

        [Test]
        public void SumEstimateWorkOtherProductsCosts()
        {
            var workName = "ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА";

            var estimateWorkVatFreeOtherProductsCost = 0.022;
            var estimateVatFree = CreateEstimateWithOneEstimateWork(workName, 0, estimateWorkVatFreeOtherProductsCost, 0, 0);

            var estimateWorkVatOtherProductsCost = 0.024;
            var estimateVat = CreateEstimateWithOneEstimateWork(workName, 0, estimateWorkVatOtherProductsCost, 0, 0);

            var estimate = _estimateConnector.Connect(estimateVatFree, estimateVat);

            var estimateWorkOtherPoductsCost = estimate.EstimateWorks.First(x => x.WorkName == workName).OtherProductsCost;

            Assert.AreEqual(estimateWorkVatFreeOtherProductsCost + estimateWorkVatOtherProductsCost, estimateWorkOtherPoductsCost);
        }

        [Test]
        public void SumEstimateWorkTotalCosts()
        {
            var workName = "ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА";

            var estimateWorkVatFreeTotalCost = 55.297;
            var estimateVatFree = CreateEstimateWithOneEstimateWork(workName, 0, 0, estimateWorkVatFreeTotalCost, 0);

            var estimateWorkVatTotalCost = 21.316;
            var estimateVat = CreateEstimateWithOneEstimateWork(workName, 0, 0, estimateWorkVatTotalCost, 0);

            var estimate = _estimateConnector.Connect(estimateVatFree, estimateVat);

            var estimateWorkTotalCost = estimate.EstimateWorks.First(x => x.WorkName == workName).TotalCost;

            Assert.AreEqual(estimateWorkVatFreeTotalCost + estimateWorkVatTotalCost, estimateWorkTotalCost);
        }

        [Test]
        public void SetEstimateWorkChapterFromFirstPassedEstimate()
        {
            var workName = "ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА";

            var chapter = 1;
            var estimateVatFree = CreateEstimateWithOneEstimateWork(workName, 0, 0, 0, chapter);

            var estimateVat = CreateEstimateWithOneEstimateWork(workName, 0, 0, 0, chapter);

            var estimate = _estimateConnector.Connect(estimateVatFree, estimateVat);

            var estimateWorkChapter = estimate.EstimateWorks.First(x => x.WorkName == workName).Chapter;

            Assert.AreEqual(chapter, estimateWorkChapter);
        }
    }
}
