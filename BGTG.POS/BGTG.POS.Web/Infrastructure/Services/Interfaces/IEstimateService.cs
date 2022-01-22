using BGTG.POS.Tools.EstimateTool;
using Microsoft.AspNetCore.Http;

namespace BGTG.POS.Web.Infrastructure.Services.Interfaces
{
    public interface IEstimateService
    {
        Estimate Estimate { get; }
        void Read(IFormFileCollection estimateFiles, TotalWorkChapter totalWorkChapter);
    }
}
