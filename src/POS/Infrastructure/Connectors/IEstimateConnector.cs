using Calabonga.OperationResults;
using POS.Models.EstimateModels;

namespace POS.Infrastructure.Connectors
{
    public interface IEstimateConnector
    {
        Task<OperationResult<Estimate>> Connect(IReadOnlyList<Estimate> estimates);
        Task<OperationResult<EstimateWork>> ConnectEstimateWork(IReadOnlyList<EstimateWork> estimateWorks);
    }
}