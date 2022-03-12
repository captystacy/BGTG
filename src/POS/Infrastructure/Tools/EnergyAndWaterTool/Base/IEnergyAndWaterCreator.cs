namespace POS.Infrastructure.Tools.EnergyAndWaterTool.Base;

public interface IEnergyAndWaterCreator
{
    EnergyAndWater Create(decimal totalCostIncludingCAIW, int constructionYear);
}