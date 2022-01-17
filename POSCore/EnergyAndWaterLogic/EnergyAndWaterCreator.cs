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

        private const int _caiwVolumeCoef = 100;
        private const int _energyCoef = 205;
        private const decimal _waterCoef = 0.3M;
        private const decimal _compressedAirCoef = 3.9M;
        private const int _oxygenCoef = 4400;

        private const int _multiplier = 10000;

        public EnergyAndWater Create(decimal totalCostIncludingCAIW, int constructionYear)
        {
            var temp = totalCostIncludingCAIW * _multiplier / _coef1 / _coef2 / _coef3 / _coef4 / _coef5;
            var caiwVolume = decimal.Round(temp * _caiwVolumeCoef, 3);
            var energy = decimal.Round(temp * _energyCoef, 3);
            var water = decimal.Round(temp * _waterCoef, 3);
            var compressedAir = decimal.Round(temp * _compressedAirCoef, 3);
            var oxygen = decimal.Round(temp * _oxygenCoef, 3);
            return new EnergyAndWater(constructionYear, caiwVolume, energy, water, compressedAir, oxygen);
        }
    }
}
