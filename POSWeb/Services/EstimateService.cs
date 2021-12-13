using Microsoft.AspNetCore.Http;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using POSWeb.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace POSWeb.Services
{
    public class EstimateService : IEstimateService
    {
        private readonly IEstimateManager _estimateManager;

        public Estimate Estimate { get; private set; }

        public EstimateService(IEstimateManager estimateManager)
        {
            _estimateManager = estimateManager;
        }

        public void ReadEstimateFiles(IEnumerable<IFormFile> estimateFiles)
        {
            var estimateStreams = estimateFiles.Select(x => x.OpenReadStream());
            Estimate = _estimateManager.GetEstimate(estimateStreams);
        }
    }
}
