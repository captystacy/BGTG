using NUnit.Framework;
using POSCore.EstimateLogic;
using System;
using System.Collections.Generic;

namespace POSCoreTests.EstimateLogic
{
    public class EstimateConnetorTests
    {
        private EstimateConnector CreateDefaultEstimateConnector()
        {
            return new EstimateConnector();
        }

        [Test]
        public void Connect_OneEstimate_SameEstimate()
        {
            var constructionStartDate = new DateTime(2021, 9, 1);
            var constructionDuration = 1;

            var estimateVatFree = new Estimate(new List<EstimateWork>
            {
                new EstimateWork("РЕКУЛЬТИВАЦИЯ", 0, 0, (decimal)0.005, 1),
                new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, (decimal)50.02, 2),
                new EstimateWork("БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ", 0, (decimal)0.004, (decimal)0.092, 7),
                new EstimateWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%", 0, 0, (decimal)0.592, 8),
                new EstimateWork("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ", 0, (decimal)11.519, (decimal)63.689, 10),
            }, constructionStartDate, constructionDuration);

            var estimateConnector = CreateDefaultEstimateConnector();
            var estimate = estimateConnector.Connect(new List<Estimate> { estimateVatFree });

            for (int i = 0; i < estimateVatFree.EstimateWorks.Count; i++)
            {
                Assert.AreEqual(estimateVatFree.EstimateWorks[i].WorkName, estimate.EstimateWorks[i].WorkName);
                Assert.AreEqual(estimateVatFree.EstimateWorks[i].TotalCost, estimate.EstimateWorks[i].TotalCost);
                Assert.AreEqual(estimateVatFree.EstimateWorks[i].EquipmentCost, estimate.EstimateWorks[i].EquipmentCost);
                Assert.AreEqual(estimateVatFree.EstimateWorks[i].OtherProductsCost, estimate.EstimateWorks[i].OtherProductsCost);
                Assert.AreEqual(estimateVatFree.EstimateWorks[i].Chapter, estimate.EstimateWorks[i].Chapter);
            }

            Assert.AreEqual(constructionStartDate, estimate.ConstructionStartDate);
            Assert.AreEqual(constructionDuration, estimate.ConstructionDuration);
        }

        [Test]
        public void Connect_TwoEstimates_CorrectOneEstimate()
        {
            var constructionStartDate = new DateTime(2021, 9, 1);
            var constructionDuration = 1;

            var estimateVat = new Estimate(new List<EstimateWork>
            {
                new EstimateWork("РЕКУЛЬТИВАЦИЯ", 0, 0, (decimal)0.001, 1),
                new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, (decimal)19.308, 2),
                new EstimateWork("БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ", 0, (decimal)0.002, (decimal)0.038, 7),
                new EstimateWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%", 0, 0, (decimal)0.253, 8),
                new EstimateWork("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ", 0, (decimal)9.911, (decimal)30.085, 10),
            }, constructionStartDate, constructionDuration);

            var estimateVatFree = new Estimate(new List<EstimateWork>
            {
                new EstimateWork("РЕКУЛЬТИВАЦИЯ", 0, 0, (decimal)0.005, 1),
                new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, (decimal)50.02, 2),
                new EstimateWork("ШРП", 0, (decimal)0.014, (decimal)0.192, 3),
                new EstimateWork("БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ", 0, (decimal)0.004, (decimal)0.092, 7),
                new EstimateWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%", 0, 0, (decimal)0.592, 8),
                new EstimateWork("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ", 0, (decimal)11.519, (decimal)63.689, 10),
            }, constructionStartDate, constructionDuration);

            var expectedEstimate = new Estimate(new List<EstimateWork>
            {
                new EstimateWork("РЕКУЛЬТИВАЦИЯ", 0, 0, (decimal)0.006, 1),
                new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, (decimal)69.328, 2),
                new EstimateWork("ШРП", 0, (decimal)0.014, (decimal)0.192, 3),
                new EstimateWork("БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ", 0, (decimal)0.006, (decimal)0.13, 7),
                new EstimateWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%", 0, 0, (decimal)0.845, 8),
                new EstimateWork("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ", 0, (decimal)21.43, (decimal)93.774, 10),
            }, constructionStartDate, constructionDuration);

            var estimateConnector = CreateDefaultEstimateConnector();
            var estimate = estimateConnector.Connect(new List<Estimate> { estimateVat, estimateVatFree });

            for (int i = 0; i < expectedEstimate.EstimateWorks.Count; i++)
            {
                Assert.AreEqual(expectedEstimate.EstimateWorks[i].WorkName, estimate.EstimateWorks[i].WorkName);
                Assert.AreEqual(expectedEstimate.EstimateWorks[i].TotalCost, estimate.EstimateWorks[i].TotalCost);
                Assert.AreEqual(expectedEstimate.EstimateWorks[i].EquipmentCost, estimate.EstimateWorks[i].EquipmentCost);
                Assert.AreEqual(expectedEstimate.EstimateWorks[i].OtherProductsCost, estimate.EstimateWorks[i].OtherProductsCost);
                Assert.AreEqual(expectedEstimate.EstimateWorks[i].Chapter, estimate.EstimateWorks[i].Chapter);
            }

            Assert.AreEqual(constructionStartDate, estimate.ConstructionStartDate);
            Assert.AreEqual(constructionDuration, estimate.ConstructionDuration);
        }
    }
}
