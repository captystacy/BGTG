using NUnit.Framework;
using POSCore.EstimateLogic;

namespace POSCoreTests.EstimateLogic
{
    public class EstimateAsserter
    {
        public void AssertEstimate(Estimate expected, Estimate actual)
        {
            for (int i = 0; i < expected.PreparatoryEstimateWorks.Count; i++)
            {
                Assert.AreEqual(expected.PreparatoryEstimateWorks[i].WorkName, actual.PreparatoryEstimateWorks[i].WorkName);
                Assert.AreEqual(expected.PreparatoryEstimateWorks[i].Chapter, actual.PreparatoryEstimateWorks[i].Chapter);
                Assert.AreEqual(expected.PreparatoryEstimateWorks[i].TotalCost, actual.PreparatoryEstimateWorks[i].TotalCost);
                Assert.AreEqual(expected.PreparatoryEstimateWorks[i].OtherProductsCost, actual.PreparatoryEstimateWorks[i].OtherProductsCost);
                Assert.AreEqual(expected.PreparatoryEstimateWorks[i].EquipmentCost, actual.PreparatoryEstimateWorks[i].EquipmentCost);
                Assert.AreEqual(expected.PreparatoryEstimateWorks[i].Percentages, actual.PreparatoryEstimateWorks[i].Percentages);
            }

            for (int i = 0; i < expected.MainEstimateWorks.Count; i++)
            {
                Assert.AreEqual(expected.MainEstimateWorks[i].WorkName, actual.MainEstimateWorks[i].WorkName);
                Assert.AreEqual(expected.MainEstimateWorks[i].Chapter, actual.MainEstimateWorks[i].Chapter);
                Assert.AreEqual(expected.MainEstimateWorks[i].TotalCost, actual.MainEstimateWorks[i].TotalCost);
                Assert.AreEqual(expected.MainEstimateWorks[i].OtherProductsCost, actual.MainEstimateWorks[i].OtherProductsCost);
                Assert.AreEqual(expected.MainEstimateWorks[i].EquipmentCost, actual.MainEstimateWorks[i].EquipmentCost);
                Assert.AreEqual(expected.MainEstimateWorks[i].Percentages, actual.MainEstimateWorks[i].Percentages);
            }

            Assert.AreEqual(expected.ConstructionStartDate, actual.ConstructionStartDate);
            Assert.AreEqual(expected.ConstructionDuration, actual.ConstructionDuration);
            Assert.AreEqual(expected.ObjectCipher, actual.ObjectCipher);
            Assert.AreEqual(expected.LaborCosts, actual.LaborCosts);
        }
    }
}
