using Calabonga.OperationResults;
using POS.Models.EstimateModels;

namespace POS.Infrastructure.Services.Base
{
    public interface IEstimateService
    {
        Task<OperationResult<Estimate>> GetEstimate(IFormFileCollection estimateFiles, TotalWorkChapter totalWorkChapter);
        Task<OperationResult<int>> GetLaborCosts(IFormFileCollection estimateFiles);
        Task<OperationResult<EstimateWork>> GetTotalEstimateWork(IFormFileCollection estimateFiles, TotalWorkChapter totalWorkChapter);
        Task<OperationResult<DateTime>> GetConstructionStartDate(IFormFile estimateFile);
    }
}