namespace BGTG.POS.EnergyAndWaterTool.Base
{
    public interface IEnergyAndWaterCreator
    {
        EnergyAndWater Create(decimal totalCostIncludingCAIW, int constructionYear);
    }
}
