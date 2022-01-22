using System.Linq;
using BGTG.POS.Tools.EstimateTool;
using BGTG.POS.Tools.EstimateTool.Interfaces;
using BGTG.POS.Web.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BGTG.POS.Web.Infrastructure.Services
{
    public class EstimateService : IEstimateService
    {
        private readonly IEstimateManager _estimateManager;

        public Estimate Estimate { get; private set; }

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
