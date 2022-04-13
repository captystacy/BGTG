using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using POS.Infrastructure.Appenders;
using POS.Infrastructure.Services;
using POS.Models;
using POS.Models.DurationByTCPModels;
using POS.Tests.Helpers;
using POS.Tests.Helpers.Appenders;
using POS.Tests.Helpers.Calculators;
using POS.Tests.Helpers.Factories;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using POS.ViewModels;
using Xunit;

namespace POS.Tests.Infrastructure.Services
{
    public class DurationByTCPServiceTests
    {
        [Fact]
        public async Task ItShould_be_able_to_give_correct_duration_by_tcp()
        {
            // arrange

            var viewModel = new DurationByTCPViewModel();

            var section = MySectionHelper.GetMock();
            var document = MyWordDocumentHelper.GetMock(section);
            var documentFactory = MyWordDocumentFactoryHelper.GetMock(document);

            var durationByTCP = new InterpolationDurationByTCP();
            var durationByTCPCalculator = DurationByTCPCalculatorHelper.GetMock(viewModel, durationByTCP);
            var durationByTCPAppender = DurationByTCPAppenderHelper.GetMock();
            var sut = new DurationByTCPService(documentFactory.Object, durationByTCPCalculator.Object, durationByTCPAppender.Object);

            // act

            var getDurationByLCStreamOperation = await sut.GetDurationByTCPStream(viewModel);

            // assert

            Assert.True(getDurationByLCStreamOperation.Ok);

            Assert.NotNull(getDurationByLCStreamOperation.Result);

            durationByTCPAppender.Verify(x => x.AppendAsync(section.Object, durationByTCP), Times.Once);
        }
    }
}
