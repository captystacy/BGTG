using POSCore.EstimateLogic.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POSCore.EstimateLogic
{
    public class EstimateManager : IEstimateManager
    {
        private readonly IEstimateReader _estimateReader;
        private readonly IEstimateConnector _estimateConnector;

        public EstimateManager(IEstimateReader estimateReader, IEstimateConnector estimateConnector)
        {
            _estimateReader = estimateReader;
            _estimateConnector = estimateConnector;
        }

        public Estimate GetEstimate(IEnumerable<Stream> estimateStreams)
        {
            var estimates = estimateStreams.Select(s => _estimateReader.Read(s));
            return _estimateConnector.Connect(estimates.ToList());
        }
    }
}
