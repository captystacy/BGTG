using Calabonga.OperationResults;
using POS.Models.EstimateModels;

namespace POS.Infrastructure.Readers.Base
{
    public interface IEstimateReader
    {
        Task<OperationResult<Estimate>> GetEstimate(Stream stream, TotalWorkChapter totalWorkChapter);
        Task<OperationResult<int>> GetLaborCosts(Stream stream);
        Task<OperationResult<EstimateWork>> GetTotalEstimateWork(Stream stream, TotalWorkChapter totalWorkChapter);
        Task<OperationResult<DateTime>> GetConstructionStartDate(Stream stream);
    }
}