using POSCore.EnergyAndWaterLogic.Interfaces;

namespace POSCore.EnergyAndWaterLogic
{
    public class EnergyAndWaterCreator : IEnergyAndWaterCreator
    {
        private const decimal _coef1 = 1.0066M;
        private const decimal _coef2 = 6.9877M;
        private const int _coef3 = 1400;
        private const decimal _coef4 = 2.93M;
        private const int _coef5 = 1000;

        private const int _smrVolumeCoef = 100;
        private const int _energyCoef = 205;
        private const decimal _waterCoef = 0.3M;
        private const decimal _compressedAirCoef = 3.9M;
        private const int _oxygenCoef = 4400;

        private const int _multiplier = 10000;

        public EnergyAndWater Create(decimal totalCostIncludingContructionAndInstallationWorks, int constructionYear)
        {
            var temp = totalCostIncludingContructionAndInstallationWorks * _multiplier / _coef1 / _coef2 / _coef3 / _coef4 / _coef5;
            var smrVolume = temp * _smrVolumeCoef;
            var energy = temp * _energyCoef;
            var water = temp * _waterCoef;
            var compressedAir = temp * _compressedAirCoef;
            var oxygen = temp * _oxygenCoef;
            return new EnergyAndWater(constructionYear, smrVolume, energy, water, compressedAir, oxygen);
        }
    }
}
