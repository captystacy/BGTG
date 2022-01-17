using NUnit.Framework;
using POSCore.EnergyAndWaterLogic;

namespace POSCoreTests.EnergyAndWaterLogic
{
    public class EnergyAndWaterCreatorTests
    {
        private EnergyAndWaterCreator _energyAndWaterCreator;

        [SetUp]
        public void SetUp()
        {
            _energyAndWaterCreator = new EnergyAndWaterCreator();
        }

        [Test]
        public void Create_TotalCostIncludingCAIW_CorrectEnergyAndWater()
        {
            var totalCostIncludingCAIW = 12.986M;
            var constructionYear = 2021;
            var caiwVolume = 0.45M;
            var energy = 0.923M;
            var water = 0.001M;
            var compressedAir = 0.018M;
            var oxygen = 19.803M;

            var expectedEnergyAndWater = new EnergyAndWater(constructionYear, caiwVolume, energy, water, compressedAir, oxygen);

            var actualEnergyAndWater = _energyAndWaterCreator.Create(totalCostIncludingCAIW, constructionYear);

            Assert.AreEqual(expectedEnergyAndWater, actualEnergyAndWater);
        }
    }
}
