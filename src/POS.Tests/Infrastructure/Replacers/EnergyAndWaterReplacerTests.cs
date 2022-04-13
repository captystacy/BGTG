using Moq;
using POS.Infrastructure.Replacers;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using Xunit;

namespace POS.Tests.Infrastructure.Replacers
{
    public class EnergyAndWaterReplacerTests
    {
        [Fact]
        public void ItShould_replace_energy_and_water_table()
        {
            // arrange

            var document = MyWordDocumentHelper.GetMock();
            var table = MyTableHelper.GetMock();

            var sut = new EnergyAndWaterReplacer();

            // act

            sut.Replace(document.Object, table.Object);

            // assert

            document.Verify(x=>x.ReplaceTextWithTable("%ENERGY_AND_WATER_TABLE%", table.Object), Times.Once);
        }
    }
}
