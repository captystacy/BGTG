namespace BGTG.POS.EnergyAndWaterTool.Interfaces
{
    public interface IEnergyAndWaterCreator
    {
        EnergyAndWater Create(decimal totalCostIncludingCAIW, int constructionYear);
    }
}
