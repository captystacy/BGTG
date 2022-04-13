using Calabonga.OperationResults;
using POS.Infrastructure.Calculators.Base;
using POS.Models;

namespace POS.Infrastructure.Calculators
{
    public class EnergyAndWaterCalculator : IEnergyAndWaterCalculator
    {
        private const decimal Coef1 = 1.0066M;
        private const decimal Coef2 = 6.9877M;
        private const int Coef3 = 1400;
        private const decimal Coef4 = 2.93M;
        private const int Coef5 = 1000;

        private const int VolumeCaiwCoef = 100;
        private const int EnergyCoef = 205;
        private const decimal WaterCoef = 0.3M;
        private const decimal CompressedAirCoef = 3.9M;
        private const int OxygenCoef = 4400;

        private const int Multiplier = 10000;

        public Task<OperationResult<EnergyAndWater>> Calculate(decimal totalCostIncludingCAIW, int constructionYear)
        {
            var operation = OperationResult.CreateResult<EnergyAndWater>();

            if (totalCostIncludingCAIW <= 0)
            {
                operation.AddError("Total const including construction and installation works could not be less or equal zero");
                return Task.FromResult(operation);
            }

            if (constructionYear <= 0)
            {
                operation.AddError("Construction year could not be less or equal zero");
                return Task.FromResult(operation);
            }

            var temp = totalCostIncludingCAIW * Multiplier / Coef1 / Coef2 / Coef3 / Coef4 / Coef5;
            var volumeCAIW = decimal.Round(temp * VolumeCaiwCoef, 3);
            var energy = decimal.Round(temp * EnergyCoef, 3);
            var water = decimal.Round(temp * WaterCoef, 3);
            var compressedAir = decimal.Round(temp * CompressedAirCoef, 3);
            var oxygen = decimal.Round(temp * OxygenCoef, 3);

            operation.Result = new EnergyAndWater
            {
                VolumeCAIW = volumeCAIW,
                CompressedAir = compressedAir,
                Energy = energy,
                Water = water,
                ConstructionYear = constructionYear < 1900
                    ? DateTime.Today.Year
                    : constructionYear,
                Oxygen = oxygen
            };

            return Task.FromResult(operation);
        }
    }
}