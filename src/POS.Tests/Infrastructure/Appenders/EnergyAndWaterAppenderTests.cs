using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Appenders;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Format;
using POS.Models;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using Xunit;

namespace POS.Tests.Infrastructure.Appenders
{
    public class EnergyAndWaterAppenderTests
    {
        [Fact]
        public async Task ItShould_set_correct_header()
        {
            // arrange

            var fixture = new Fixture();

            var energyAndWater = fixture.Create<EnergyAndWater>();

            var firstRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var secondRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var table = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(firstRowCells),
                MyRowHelper.GetMock(secondRowCells),
                MyRowHelper.GetMock(),
            });

            var section = MySectionHelper.GetMock(table, null!, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, null!, MyTableStyle.TableGrid);

            var sut = new EnergyAndWaterAppender();

            // act

            await sut.AppendAsync(section.Object, energyAndWater);

            // assert

            firstRowCells[0].Verify(x => x.AddParagraph("Год строит."), Times.Once);
            firstRowCells[1].Verify(x => x.AddParagraph("Объем СМР, тыс.руб."), Times.Once);
            firstRowCells[2].Verify(x => x.AddParagraph("Потребность в энергоресурсах и воде"), Times.Once);

            secondRowCells[2].Verify(x => x.AddParagraph("электроэнергия, кВа"), Times.Once);
            secondRowCells[3].Verify(x => x.AddParagraph("вода, л/с"), Times.Once);
            secondRowCells[4].Verify(x => x.AddParagraph("сжатый воздух, компрессор, шт."), Times.Once);
            secondRowCells[5].Verify(x => x.AddParagraph("кислород, м3"), Times.Once);
        }

        [Fact]
        public async Task ItShould_set_correct_energy_and_water_values()
        {
            // arrange

            var fixture = new Fixture();

            var energyAndWater = fixture.Create<EnergyAndWater>();

            var thirdRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var table = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(thirdRowCells),
            });

            var section = MySectionHelper.GetMock(table, null!, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, null!, MyTableStyle.TableGrid);

            var sut = new EnergyAndWaterAppender();

            // act

            await sut.AppendAsync(section.Object, energyAndWater);

            // assert

            thirdRowCells[0].Verify(x => x.AddParagraph(energyAndWater.ConstructionYear.ToString()), Times.Once);
            thirdRowCells[1].Verify(x => x.AddParagraph(energyAndWater.VolumeCAIW.ToString()), Times.Once);
            thirdRowCells[2].Verify(x => x.AddParagraph(energyAndWater.Energy.ToString()), Times.Once);
            thirdRowCells[3].Verify(x => x.AddParagraph(energyAndWater.Water.ToString()), Times.Once);
            thirdRowCells[4].Verify(x => x.AddParagraph(energyAndWater.CompressedAir.ToString()), Times.Once);
            thirdRowCells[5].Verify(x => x.AddParagraph(energyAndWater.Oxygen.ToString()), Times.Once);
        }
    }
}