namespace POS.EnergyAndWaterLogic.Interfaces
{
    public interface IEnergyAndWaterCreator
    {
        EnergyAndWater Create(decimal totalCostIncludingCAIW, int constructionYear);
    }
}
