using POS.DomainModels;

namespace POS.Infrastructure.Creators.Base;

public interface IEnergyAndWaterCreator
{
    EnergyAndWater Create(decimal totalCostIncludingCAIW, int constructionYear);
}