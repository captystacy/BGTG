using System.Collections.Generic;
using Calabonga.OperationResults;
using Moq;
using POS.Infrastructure.Connectors;
using POS.Models.EstimateModels;

namespace POS.Tests.Helpers.Connectors
{
    public static class EstimateConnectorHelper
    {
        public static Mock<IEstimateConnector> GetMock()
        {
            return new Mock<IEstimateConnector>();
        }

        public static Mock<IEstimateConnector> GetMock(IReadOnlyList<Estimate> inputEstimates, Estimate resultEstimate)
        {
            var estimateConnector = GetMock();

            estimateConnector.Setup(x => x.Connect(inputEstimates)).ReturnsAsync(new OperationResult<Estimate>
            {
                Result = resultEstimate
            });

            return estimateConnector;
        }
    }
}
