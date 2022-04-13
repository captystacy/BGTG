using System.Threading.Tasks;
using POS.Infrastructure.Calculators;
using POS.Models;
using Xunit;

namespace POS.Tests.Infrastructure.Calculators
{
    public class EnergyAndWaterCalculatorTests
    {
        [Fact]
        public async Task ItShould_calculate_correct_energy_and_water()
        {
            // arrange

            var totalCostIncludingCAIW = 12.986M;
            var constructionYear = 2021;
            var volumeCAIW = 0.45M;
            var energy = 0.923M;
            var water = 0.001M;
            var compressedAir = 0.018M;
            var oxygen = 19.803M;

            var expectedEnergyAndWater = new EnergyAndWater
            {
                ConstructionYear = constructionYear,
                CompressedAir = compressedAir,
                Energy = energy,
                Oxygen = oxygen,
                VolumeCAIW = volumeCAIW,
                Water = water
            };

            var sut = new EnergyAndWaterCalculator();

            // act

            var calculateOperation = await sut.Calculate(totalCostIncludingCAIW, constructionYear);

            // assert

            Assert.True(calculateOperation.Ok);

            Assert.Equal(expectedEnergyAndWater, calculateOperation.Result);
        }
    }
}