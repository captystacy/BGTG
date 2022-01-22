namespace BGTG.POS.Tools.EnergyAndWaterTool.Interfaces
{
    public interface IEnergyAndWaterCreator
    {
        EnergyAndWater Create(decimal totalCostIncludingCAIW, int constructionYear);
    }
}
