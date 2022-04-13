using System.Threading.Tasks;
using AutoFixture;
using Moq;
using POS.Infrastructure.Replacers;
using POS.Models;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using Xunit;

namespace POS.Tests.Infrastructure.Replacers
{
    public class TechnicalAndEconomicalIndicatorsReplacerTests
    {
        [Fact]
        public async Task ItShould_replace_technical_and_economical_indicators_patterns()
        {
            // arrange

            var document = MyWordDocumentHelper.GetMock();
            var durationByLC = new Fixture().Create<DurationByLC>();

            var sut = new TechnicalAndEconomicalIndicatorsReplacer();

            // act

            await sut.Replace(document.Object, durationByLC);

            // assert

            document.Verify(x => x.Replace("%TD%", durationByLC.TotalDuration.ToString()), Times.Once);
            document.Verify(x => x.Replace("%PP%", durationByLC.PreparatoryPeriod.ToString()), Times.Once);
            document.Verify(x => x.Replace("%AT%", durationByLC.AcceptanceTime.ToString()), Times.Once);
            document.Verify(x => x.Replace("%TLC%", durationByLC.TotalLaborCosts.ToString()), Times.Once);
        }
    }
}
