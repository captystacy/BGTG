using POSCore.EnergyAndWaterLogic.Interfaces;

namespace POSCore.EnergyAndWaterLogic
{
    public class EnergyAndWaterCreator : IEnergyAndWaterCreator
    {
        private const decimal _coef1 = (decimal)1.0066;
        private const decimal _coef2 = (decimal)6.9877;
        private const decimal _coef3 = 1400;
        private const decimal _coef4 = (decimal)2.93;
        private const decimal _coef5 = 1000;

        private const decimal _smrVolumeCoef = 100;
        private const decimal _energyCoef = 205;
        private const decimal _waterCoef = (decimal)0.3;
        private const decimal _compressedAirCoef = (decimal)3.9;
        private const decimal _oxygenCoef = 4400;

        private const decimal _multiplier = 10000;

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
