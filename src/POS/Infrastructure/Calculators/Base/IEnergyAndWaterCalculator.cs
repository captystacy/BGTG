using Calabonga.OperationResults;
using POS.Models;

namespace POS.Infrastructure.Calculators.Base
{
    public interface IEnergyAndWaterCalculator
    {
        Task<OperationResult<EnergyAndWater>> Calculate(decimal totalCostIncludingCAIW, int constructionYear);
    }
}