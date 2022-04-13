using System.Globalization;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Appenders.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Format;
using POS.Models;

namespace POS.Infrastructure.Appenders
{
    public class EnergyAndWaterAppender : IEnergyAndWaterAppender
    {
        private const string FirstRowFirstCellText = "Год строит.";
        private const string FirstRowSecondCellText = "Объем СМР, тыс.руб.";
        private const string FirstRowThirdCellText = "Потребность в энергоресурсах и воде";

        private const string SecondRowThirdCellText = "электроэнергия, кВа";
        private const string SecondRowFourthCellText = "вода, л/с";
        private const string SecondRowFifthCellText = "сжатый воздух, компрессор, шт.";
        private const string SecondRowSixthCellText = "кислород, м3";

        public async Task<IMyTable> AppendAsync(IMySection section, EnergyAndWater energyAndWater)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            var tasks = new List<Task>();
            var table = section.AddTable(6, 3, null!, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, null!, MyTableStyle.TableGrid);

            tasks.Add(SetHeader(table));

            tasks.Add(SetEnergyAndWaterValues(table, energyAndWater));

            await Task.WhenAll(tasks);

            return table;
        }

        private Task SetEnergyAndWaterValues(IMyTable table, EnergyAndWater energyAndWater)
        {
            table.Rows[2].Cells[0].AddParagraph(energyAndWater.ConstructionYear.ToString());
            table.Rows[2].Cells[1].AddParagraph(energyAndWater.VolumeCAIW.ToString());
            table.Rows[2].Cells[2].AddParagraph(energyAndWater.Energy.ToString());
            table.Rows[2].Cells[3].AddParagraph(energyAndWater.Water.ToString());
            table.Rows[2].Cells[4].AddParagraph(energyAndWater.CompressedAir.ToString());
            table.Rows[2].Cells[5].AddParagraph(energyAndWater.Oxygen.ToString());

            return Task.CompletedTask;
        }

        private Task SetHeader(IMyTable table)
        {
            table.Rows[0].Cells[0].AddParagraph(FirstRowFirstCellText);
            table.ApplyVerticalMerge(0, 0, 1);
            table.Rows[0].Cells[1].AddParagraph(FirstRowSecondCellText);
            table.ApplyVerticalMerge(1, 0, 1);
            table.Rows[0].Cells[2].AddParagraph(FirstRowThirdCellText);
            table.ApplyHorizontalMerge(0, 2, 5);

            table.Rows[1].Cells[2].AddParagraph(SecondRowThirdCellText);
            table.Rows[1].Cells[3].AddParagraph(SecondRowFourthCellText);
            table.Rows[1].Cells[4].AddParagraph(SecondRowFifthCellText);
            table.Rows[1].Cells[5].AddParagraph(SecondRowSixthCellText);

            return Task.CompletedTask;
        }
    }
}