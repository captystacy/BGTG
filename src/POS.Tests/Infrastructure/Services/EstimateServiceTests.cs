using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using POS.Infrastructure.Services;
using POS.Models.EstimateModels;
using POS.Tests.Helpers;
using POS.Tests.Helpers.Connectors;
using POS.Tests.Helpers.Readers;
using Xunit;

namespace POS.Tests.Infrastructure.Services
{
    public class EstimateServiceTests
    {
        [Fact]
        public async Task ItShould_get_estimate()
        {
            // arrange

            var totalWorkChapter = TotalWorkChapter.TotalWork1To12Chapter;

            var stream1 = StreamHelper.GetMock();
            var stream2 = StreamHelper.GetMock();

            var estimate1 = new Estimate();
            var estimate2 = new Estimate();
            var expected = new Estimate();

            var streamAndEstimate = new Dictionary<Mock<Stream>, Estimate>()
            {
                {stream1, estimate1},
                {stream2, estimate2},
            };

            var estimateFiles = new FormFileCollection
            {
                FormFileHelper.GetMock(stream1).Object,
                FormFileHelper.GetMock(stream2).Object,
            };

            var estimateReader = EstimateReaderHelper.GetMock(streamAndEstimate, totalWorkChapter);
            var estimateConnector = EstimateConnectorHelper.GetMock(new List<Estimate> { estimate1, estimate2 }, expected);
            var sut = new EstimateService(estimateConnector.Object, estimateReader.Object);

            // act

            var getEstimateOperation = await sut.GetEstimate(estimateFiles, totalWorkChapter);

            // assert

            Assert.True(getEstimateOperation.Ok);

            Assert.Same(expected, getEstimateOperation.Result);
        }

        [Fact]
        public async Task ItShould_get_labor_cost()
        {
            // arrange

            var stream1 = StreamHelper.GetMock();
            var stream2 = StreamHelper.GetMock();

            var estimate1 = new Estimate();
            var estimate2 = new Estimate();
            var expected = new Estimate();

            var streamAndLaborCosts = new Dictionary<Mock<Stream>, int>()
            {
                {stream1, 11},
                {stream2, 10},
            };

            var estimateFiles = new FormFileCollection
            {
                FormFileHelper.GetMock(stream1).Object,
                FormFileHelper.GetMock(stream2).Object,
            };

            var estimateReader = EstimateReaderHelper.GetMock(streamAndLaborCosts);
            var estimateConnector = EstimateConnectorHelper.GetMock(new List<Estimate> { estimate1, estimate2 }, expected);
            var sut = new EstimateService(estimateConnector.Object, estimateReader.Object);

            // act

            var getLaborCostsOperation = await sut.GetLaborCosts(estimateFiles);

            // assert

            Assert.True(getLaborCostsOperation.Ok);

            Assert.Equal(21, getLaborCostsOperation.Result);
        }
    }
}