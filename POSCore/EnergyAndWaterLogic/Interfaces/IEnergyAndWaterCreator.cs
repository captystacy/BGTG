namespace POSCore.EnergyAndWaterLogic.Interfaces
{
    public interface IEnergyAndWaterCreator
    {
        EnergyAndWater Create(decimal totalCostIncludingContructionAndInstallationWorks, int constructionYear);
    }
}
