using System.Globalization;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Appenders.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Format;
using POS.Models;

namespace POS.Infrastructure.Appenders
{
    public class DurationByLCAppender : IDurationByLCAppender
    {
        public const string FirstParagraphText = 
            "Нормативная продолжительность строительства объекта определена по (п.4.22) ТКП 45-1.03-122-2015 " +
            "«Нормы продолжительности строительства предприятий, зданий и сооружений», а также по нормативной " +
            "трудоемкости глав 1-8 ССР и ориентировочному количеству работающих:";

        public const string FormulaFirstRowFirstCellText = "Т =";

        public const string DescriptionFirstRowFirstCellText = "где";

        public const string LaborCostsText = "нормативные трудозатраты, человеко/часов;";
        public const string TotalLaborCostsText = "нормативные трудозатраты (трудозатраты по сметам и трудозатраты по технологической карте), человеко/часов;";
        public const string WorkingDayDurationText = "продолжительность рабочего дня, часов;";
        public const string ShiftText = "сменность;";
        public const string NumberOfWorkingDaysText = "количество рабочих дней в месяце;";
        public const string NumberOfEmployeesText = "количество работающих в бригаде в соответствии с технологией производства ремонтно-строительных работ.";

        public const int FormulaTableColumnsNumber = 3;
        public const int FormulaTableRowsNumber = 2;

        public const int DescriptionTableColumnsNumber = 4;
        public const int DescriptionTableRowsNumber = 5;
        
        public Task AppendAsync(IMySection section, DurationByLC durationByLC)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            var tasks = new List<Task>();
            section.ParagraphFormat = Constants.DefaultParagraph;
            section.CharacterFormat = Constants.DefaultFontSize;

            section.AddParagraph(FirstParagraphText);

            section.AddParagraph();

            var formulaTable = section.AddTable(FormulaTableColumnsNumber, FormulaTableRowsNumber,
                Constants.TableHorizontalCentered, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered,
                Constants.DefaultFontSize, MyTableStyle.TableCleared);
            tasks.Add(SetFormulaValues(formulaTable, durationByLC));

            section.AddParagraph();

            var descriptionTable = section.AddTable(DescriptionTableColumnsNumber, DescriptionTableRowsNumber, null!,
                null!, null!, Constants.DefaultFontSize, MyTableStyle.TableCleared);
            tasks.Add(SetDescriptionValues(descriptionTable, durationByLC));

            AddPenultimateParagraph(section, durationByLC);

            AddLastParagraph(section, durationByLC);

            return Task.WhenAll(tasks);
        }

        private void AddLastParagraph(IMySection section, DurationByLC durationByLC)
        {
            if (durationByLC.AcceptanceTimeIncluded)
            {
                section.AddParagraph(
                    $"Принимаем продолжительность строительства равную {durationByLC.TotalDuration} мес, в том числе подготовительный период – {durationByLC.PreparatoryPeriod} мес, " +
                    $"приемка объекта в эксплуатацию – {durationByLC.AcceptanceTime} мес.");
            }
            else
            {
                section.AddParagraph(
                    $"Принимаем продолжительность строительства равную {durationByLC.RoundedDuration} мес, в том числе подготовительный период – {durationByLC.PreparatoryPeriod} мес.");
            }
        }

        private void AddPenultimateParagraph(IMySection section, DurationByLC durationByLC)
        {
            if (durationByLC.RoundingIncluded)
            {
                if (durationByLC.AcceptanceTimeIncluded)
                {
                    section.AddParagraph(
                        $"С учетом округления в соответствии с 4.36 ТКП 45-1.03-122-2015 нормативная продолжительность строительства составит {durationByLC.RoundedDuration} мес, " +
                        "с учетом времени на приемку объекта в эксплуатацию и утверждения акта приемки объекта в эксплуатацию согласно ТКП 45-1.03-122-2015 " +
                        $"п. 4.22 общая продолжительность строительства составит – Tобщ = {durationByLC.RoundedDuration} + {durationByLC.AcceptanceTime} = {durationByLC.TotalDuration} мес.");
                }
                else
                {
                    section.AddParagraph(
                        $"С учетом округления в соответствии с 4.36 ТКП 45-1.03-122-2015 нормативная продолжительность строительства составит {durationByLC.RoundedDuration} мес.");
                }
            }
            else
            {
                if (durationByLC.AcceptanceTimeIncluded)
                {
                    section.AddParagraph(
                        "Нормативная продолжительность строительства с учетом времени на приемку объекта в эксплуатацию и утверждения акта приемки объекта в эксплуатацию согласно ТКП 45-1.03-122-2015 " +
                        $"п. 4.22 общая продолжительность строительства составит – Tобщ = {durationByLC.RoundedDuration} + {durationByLC.AcceptanceTime} = {durationByLC.TotalDuration} мес.");
                }
            }
        }

        private Task SetDescriptionValues(IMyTable table, DurationByLC durationByLC)
        {
            table.Rows[0].Cells[0].AddParagraph(DescriptionFirstRowFirstCellText);

            for (int i = 0; i < DescriptionTableRowsNumber; i++)
            {
                table.Rows[i].Cells[2].AddParagraph(Constants.DashSymbolStr);
            }

            table.Rows[0].Cells[1].AddParagraph(durationByLC.TotalLaborCosts.ToString());
            table.Rows[0].Cells[3].AddParagraph(
                durationByLC.TechnologicalLaborCosts > 0
                    ? TotalLaborCostsText
                    : LaborCostsText);

            table.Rows[1].Cells[1].AddParagraph(durationByLC.WorkingDayDuration.ToString());
            table.Rows[1].Cells[3].AddParagraph(WorkingDayDurationText);

            table.Rows[2].Cells[1].AddParagraph(durationByLC.Shift.ToString());
            table.Rows[2].Cells[3].AddParagraph(ShiftText);

            table.Rows[3].Cells[1].AddParagraph(durationByLC.NumberOfWorkingDays.ToString());
            table.Rows[3].Cells[3].AddParagraph(NumberOfWorkingDaysText);

            table.Rows[4].Cells[1].AddParagraph(durationByLC.NumberOfEmployees.ToString());
            table.Rows[4].Cells[3].AddParagraph(NumberOfEmployeesText);

            table.AutoFit(MyAutoFitBehaviorType.AutoFitToContents);

            return Task.CompletedTask;
        }

        private Task SetFormulaValues(IMyTable table, DurationByLC durationByLC)
        {
            table.Rows[0].Cells[0].AddParagraph(FormulaFirstRowFirstCellText);
            table.Rows[0].Cells[1]
                .ApplyFormat(Constants.BottomBorderSingle)
                .AddParagraph(durationByLC.TotalLaborCosts.ToString());

            table.ApplyVerticalMerge(0, 0, 1);

            table.Rows[0].Cells[2].AddParagraph($"= {durationByLC.Duration} мес.");

            table.ApplyVerticalMerge(2, 0, 1);

            table.Rows[1].Cells[1]
                .AddParagraph($"{durationByLC.NumberOfWorkingDays}х{durationByLC.WorkingDayDuration}х{durationByLC.Shift}х{durationByLC.NumberOfEmployees}");

            table.AutoFit(MyAutoFitBehaviorType.AutoFitToContents);

            return Task.CompletedTask;
        }
    }
}