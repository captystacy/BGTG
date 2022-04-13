using System.Threading.Tasks;
using Calabonga.OperationResults;
using Microsoft.AspNetCore.Http;
using Moq;
using POS.Infrastructure.Services;
using POS.Models;
using POS.Tests.Helpers;
using POS.Tests.Helpers.Appenders;
using POS.Tests.Helpers.Calculators;
using POS.Tests.Helpers.Factories;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using POS.ViewModels;
using Xunit;

namespace POS.Tests.Infrastructure.Services
{
    public class DurationByLCServiceTests
    {
        [Fact]
        public async Task ItShould_be_able_to_give_correct_duration_by_labor_costs_stream()
        {
            // arrange

            var viewModel = new DurationByLCViewModel
            {
                EstimateFiles = new FormFileCollection
                {
                    FormFileHelper.GetMock().Object,
                    FormFileHelper.GetMock().Object,
                }
            };

            var section = MySectionHelper.GetMock();
            var document = MyWordDocumentHelper.GetMock(section);
            var documentFactory = MyWordDocumentFactoryHelper.GetMock(document);

            var durationByLC = new DurationByLC();
            var durationByLCCalculator = DurationByLCCalculatorHelper.GetMock(viewModel, durationByLC);
            var durationByLCAppender = DurationByLCAppenderHelper.GetMock();
            var sut = new DurationByLCService(documentFactory.Object, durationByLCCalculator.Object, durationByLCAppender.Object);

            // act

            var getDurationByLCStreamOperation = await sut.GetDurationByLCStream(viewModel);

            // assert

            Assert.True(getDurationByLCStreamOperation.Ok);

            Assert.NotNull(getDurationByLCStreamOperation.Result);

            durationByLCAppender.Verify(x => x.AppendAsync(section.Object, durationByLC), Times.Once);
        }
    }
}
