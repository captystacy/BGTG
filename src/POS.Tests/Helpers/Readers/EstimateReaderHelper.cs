using System.Collections.Generic;
using System.IO;
using Calabonga.OperationResults;
using Moq;
using POS.Infrastructure.Readers.Base;
using POS.Models.EstimateModels;

namespace POS.Tests.Helpers.Readers
{
    public static class EstimateReaderHelper
    {
        public static Mock<IEstimateReader> GetMock()
        {
            return new Mock<IEstimateReader>();
        }

        public static Mock<IEstimateReader> GetMock(Dictionary<Mock<Stream>, Estimate> streamAndEstimate, TotalWorkChapter totalWorkChapter)
        {
            var estimateReader = GetMock();

            foreach (var pair in streamAndEstimate)
            {
                estimateReader.Setup(x => x.GetEstimate(pair.Key.Object, totalWorkChapter)).ReturnsAsync(new OperationResult<Estimate>
                {
                    Result = pair.Value
                });
            }

            return estimateReader;
        }

        public static Mock<IEstimateReader> GetMock(Dictionary<Mock<Stream>, int> streamAndLaborCosts)
        {
            var estimateReader = GetMock();

            foreach (var pair in streamAndLaborCosts)
            {
                estimateReader.Setup(x => x.GetLaborCosts(pair.Key.Object)).ReturnsAsync(new OperationResult<int>
                {
                    Result = pair.Value
                });
            }

            return estimateReader;
        }
    }
}
