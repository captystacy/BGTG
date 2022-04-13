using Calabonga.OperationResults;
using Moq;
using POS.Infrastructure.Calculators.Base;
using POS.Models;

namespace POS.Tests.Helpers.Calculators
{
    public static class EnergyAndWaterCalculatorHelper
    {
        public static Mock<IEnergyAndWaterCalculator> GetMock()
        {
            return new Mock<IEnergyAndWaterCalculator>();
        }

        public static Mock<IEnergyAndWaterCalculator> GetMock(decimal totalCostIncludingCAIW, int year, EnergyAndWater energyAndWater)
        {
            var energyAndWaterCalculator = GetMock();

            energyAndWaterCalculator
                .Setup(x => x.Calculate(totalCostIncludingCAIW, year))
                .ReturnsAsync(new OperationResult<EnergyAndWater> { Result = energyAndWater });

            return energyAndWaterCalculator;
        }
    }
}
