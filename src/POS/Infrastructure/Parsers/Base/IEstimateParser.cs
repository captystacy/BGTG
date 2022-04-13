using Calabonga.OperationResults;
using POS.Infrastructure.Services.DocumentServices.ExcelService.Base;
using POS.Models.EstimateModels;

namespace POS.Infrastructure.Parsers.Base
{
    public interface IEstimateParser
    {
        Task<OperationResult<int>> GetLaborCosts(IMyExcelRange cells, int nrr103Row);
        Task<OperationResult<EstimateWork>> GetTotalEstimateWork(IMyExcelRange cells, int totalWorkPatternRow, TotalWorkChapter totalWorkChapter);
        Task<OperationResult<EstimateWork>> GetEstimateWork(IMyExcelRange cells, int workRow, int chapter = 0);
    }
}