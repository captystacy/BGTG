using BGTG.POS.EstimateTool;
using Microsoft.AspNetCore.Http;

namespace BGTG.Web.Infrastructure.Services.POSServices.Base
{
    public interface IEstimateService
    {
        Estimate Estimate { get; }
        void Read(IFormFileCollection estimateFiles, TotalWorkChapter totalWorkChapter);
    }
}
