using NUnit.Framework;
using POSCore.EnergyAndWaterLogic;

namespace POSCoreTests.EnergyAndWaterLogic
{
    public class EnergyAndWaterCreatorTests
    {
        private EnergyAndWaterCreator CreateDefaultEnergyAndWaterCreator()
        {
            return new EnergyAndWaterCreator();
        }

        [Test]
        public void Create_TotalCostIncludingContructionAndInstallationWorks_CorrectEnergyAndWater()
        {
            var energyAndWaterCreator = CreateDefaultEnergyAndWaterCreator();
            var constructionYear = 2021;
            var totalCostIncludingContructionAndInstallationWorks = 12.986M;

            var energyAndWater = energyAndWaterCreator.Create(totalCostIncludingContructionAndInstallationWorks, constructionYear);

            Assert.AreEqual(constructionYear, energyAndWater.ConstructionYear);
            Assert.AreEqual(0.45, decimal.Round(energyAndWater.SmrVolume, 3));
            Assert.AreEqual(0.923, decimal.Round(energyAndWater.Energy, 3));
            Assert.AreEqual(0.001, decimal.Round(energyAndWater.Water, 3));
            Assert.AreEqual(0.018, decimal.Round(energyAndWater.CompressedAir, 3));
            Assert.AreEqual(19.803, decimal.Round(energyAndWater.Oxygen, 3));
        }
    }
}
