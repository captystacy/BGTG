using POS.Infrastructure.Tools.EnergyAndWaterTool.Base;

namespace POS.Infrastructure.Tools.EnergyAndWaterTool;

public class EnergyAndWaterCreator : IEnergyAndWaterCreator
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

    public EnergyAndWater Create(decimal totalCostIncludingCAIW, int constructionYear)
    {
        var temp = totalCostIncludingCAIW * Multiplier / Coef1 / Coef2 / Coef3 / Coef4 / Coef5;
        var volumeCAIW = decimal.Round(temp * VolumeCaiwCoef, 3);
        var energy = decimal.Round(temp * EnergyCoef, 3);
        var water = decimal.Round(temp * WaterCoef, 3);
        var compressedAir = decimal.Round(temp * CompressedAirCoef, 3);
        var oxygen = decimal.Round(temp * OxygenCoef, 3);
        return new EnergyAndWater(constructionYear < 1900
                ? DateTime.Today.Year
                : constructionYear,
            volumeCAIW, energy, water, compressedAir, oxygen);
    }
}