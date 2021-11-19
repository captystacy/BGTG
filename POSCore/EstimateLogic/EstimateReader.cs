using OfficeOpenXml;
using POSCore.EstimateLogic.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace POSCore.EstimateLogic
{
    public class EstimateReader : IEstimateReader
    {
        public EstimateReader()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public IEnumerable<EstimateWork> Read(string filePath)
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

            if (estimateWorks.Any(x => x.TotalCost == 0))
            {
                return null;
            }

            return estimateWorks;
        }

        private EstimateWork ParseEstimateCellsToEstimateWork(ExcelWorksheet workSheet, int row)
        {
            var workNameCellStr = workSheet.Cells[row, 2].Value.ToString();
            var equipmentCostCellStr = workSheet.Cells[row, 7].Value.ToString();
            var otherProductsCostCellStr = workSheet.Cells[row, 8].Value.ToString();
            var totalCostCellStr = workSheet.Cells[row, 9].Value.ToString();

            var equipmentCost = ParseCost(equipmentCostCellStr);
            var otherProductsCost = ParseCost(otherProductsCostCellStr);
            var totalCost = ParseCost(totalCostCellStr);

            return new EstimateWork(workNameCellStr, equipmentCost, otherProductsCost, totalCost);
        }

        private double ParseCost(string costCellStr)
        {
            var costStr = Regex.Match(costCellStr, @"[0-9,]+").Value;

            var cost = default(double);
            if (!double.TryParse(costStr, NumberStyles.Any, CultureInfo.GetCultureInfo("ru-RU"), out cost))
            {
                return 0;
            }

            return cost;
        }
    }
}
