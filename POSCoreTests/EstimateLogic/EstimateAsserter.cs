using NUnit.Framework;
using POSCore.EstimateLogic;

namespace POSCoreTests.EstimateLogic
{
    public class EstimateAsserter
    {
        public void AssertEstimate(Estimate expected, Estimate actual)
        {
            for (int i = 0; i < expected.EstimateWorks.Count; i++)
            {
                Assert.AreEqual(expected.EstimateWorks[i].WorkName, actual.EstimateWorks[i].WorkName);
                Assert.AreEqual(expected.EstimateWorks[i].Chapter, actual.EstimateWorks[i].Chapter);
                Assert.AreEqual(expected.EstimateWorks[i].TotalCost, actual.EstimateWorks[i].TotalCost);
                Assert.AreEqual(expected.EstimateWorks[i].OtherProductsCost, actual.EstimateWorks[i].OtherProductsCost);
                Assert.AreEqual(expected.EstimateWorks[i].EquipmentCost, actual.EstimateWorks[i].EquipmentCost);
            }

            Assert.AreEqual(expected.ConstructionStartDate, actual.ConstructionStartDate);
            Assert.AreEqual(expected.ConstructionDuration, actual.ConstructionDuration);
            Assert.AreEqual(expected.ObjectCipher, actual.ObjectCipher);
        }
    }
}
