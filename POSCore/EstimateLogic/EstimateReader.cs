using OfficeOpenXml;
using POSCore.EstimateLogic.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace POSCore.EstimateLogic
{
    public class EstimateReader : IEstimateReader
    {
        public EstimateReader()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public Estimate Read(string filePath)
        {
            var estimateWorks = new List<EstimateWork>();

            var existingFile = new FileInfo(filePath);
            using (var package = new ExcelPackage(existingFile))
            {
                var workSheet = package.Workbook.Worksheets[0];
                for (int row = 27; row < 100; row++)
                {
                    var estimateCalculationCell = workSheet.Cells[row, 1].Value ?? "";
                    var estimateCalculationCellStr = estimateCalculationCell.ToString();

                    var previousCalculationCell = workSheet.Cells[row - 1, 1].Value ?? "";
                    var previousCalculationCellStr = previousCalculationCell.ToString();

                    if (estimateCalculationCellStr.StartsWith("ОБЪЕКТНАЯ СМЕТА")
                        || estimateCalculationCellStr == "НИИ БЕЛГИПРОТОПГАЗ"
                        || estimateCalculationCellStr == "НРР 8.01.102-2017"
                        || previousCalculationCellStr == "ПОДПУНКТ 34.1 ИНСТРУКЦИИ")
                    {
                        var estimateWork = ParseEstimateCellsToEstimateWork(workSheet, row);
                        estimateWorks.Add(estimateWork);
                    }
                }
            }

            if (estimateWorks.Exists(x => x.TotalCost == 0 || x.Chapter == 0))
            {
                return null;
            }

            return new Estimate(estimateWorks);
        }

        private int ParseChapter(ExcelWorksheet workSheet, int row)
        {
            var chapter = 0;

            for (int i = 1; i < row; i++)
            {
                var chapterCellStr = workSheet.Cells[row - i, 2].Value.ToString();
                if (chapterCellStr.StartsWith("ГЛАВА"))
                {
                    var chapterStr = Regex.Match(chapterCellStr, @"\d+").Value;
                    int.TryParse(chapterStr, out chapter);
                    break;
                }
            }

            return chapter;
        }

        private EstimateWork ParseEstimateCellsToEstimateWork(ExcelWorksheet workSheet, int row)
        {
            var workNameCellStr = workSheet.Cells[row, 2].Value.ToString();
            var equipmentCostCellStr = workSheet.Cells[row, 7].Value.ToString();
            var otherProductsCostCellStr = workSheet.Cells[row, 8].Value.ToString();
            var totalCostCellStr = workSheet.Cells[row, 9].Value.ToString();

            var chapter = ParseChapter(workSheet, row);
            var equipmentCost = ParseCost(equipmentCostCellStr);
            var otherProductsCost = ParseCost(otherProductsCostCellStr);
            var totalCost = ParseCost(totalCostCellStr);

            return new EstimateWork(workNameCellStr, equipmentCost, otherProductsCost, totalCost, chapter);
        }

        private decimal ParseCost(string costCellStr)
        {
            var costStr = Regex.Match(costCellStr, @"[0-9,]+").Value;

            decimal.TryParse(costStr, NumberStyles.Any, CultureInfo.GetCultureInfo("ru-RU"), out var cost);

            return cost % 1 != 0 
                ? cost 
                : 0;
        }
    }
}
