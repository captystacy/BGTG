using Microsoft.AspNetCore.Http;
using POSCore.EstimateLogic;
using System.Collections.Generic;

namespace POSWeb.Services.Interfaces
{
    public interface IEstimateService
    {
        Estimate Estimate { get; }
        void ReadEstimateFiles(IEnumerable<IFormFile> estimateFiles, TotalWorkChapter totalWorkChapter);
    }
}
