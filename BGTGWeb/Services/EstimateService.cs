using System.Collections.Generic;
using System.Linq;
using BGTGWeb.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using POS.EstimateLogic;
using POS.EstimateLogic.Interfaces;

namespace BGTGWeb.Services
{
    public class EstimateService : IEstimateService
    {
        private readonly IEstimateManager _estimateManager;

        public Estimate Estimate { get; private set; }

        public EstimateService(IEstimateManager estimateManager)
        {
            _estimateManager = estimateManager;
        }

        public void Read(IEnumerable<IFormFile> estimateFiles, TotalWorkChapter totalWorkChapter)
        {
            var estimateStreams = estimateFiles.Select(x => x.OpenReadStream());
            Estimate = _estimateManager.GetEstimate(estimateStreams, totalWorkChapter);
        }
    }
}
