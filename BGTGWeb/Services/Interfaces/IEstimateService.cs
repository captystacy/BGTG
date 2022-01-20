using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using POS.EstimateLogic;

namespace BGTGWeb.Services.Interfaces
{
    public interface IEstimateService
    {
        Estimate Estimate { get; }
        void Read(IEnumerable<IFormFile> estimateFiles, TotalWorkChapter totalWorkChapter);
    }
}
