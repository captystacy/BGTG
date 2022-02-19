using System.Linq;
using BGTG.POS.EstimateTool;
using BGTG.POS.EstimateTool.Base;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using Microsoft.AspNetCore.Http;

namespace BGTG.Web.Infrastructure.Services.POSServices
{
    public class EstimateService : IEstimateService
    {
        private readonly IEstimateManager _estimateManager;

        public Estimate Estimate { get; private set; } = null!;

        public EstimateService(IEstimateManager estimateManager)
        {
            _estimateManager = estimateManager;
        }

        public void Read(IFormFileCollection estimateFiles, TotalWorkChapter totalWorkChapter)
        {
            var estimateStreams = estimateFiles.Select(x => x.OpenReadStream());
            Estimate = _estimateManager.GetEstimate(estimateStreams, totalWorkChapter);
        }
    }
}
