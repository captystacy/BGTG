using System;
using System.Collections.Generic;
using System.Linq;
using BGTG.POS.Tools.CalendarPlanTool;
using BGTG.POS.Tools.Tests.EstimateTool;
using NUnit.Framework;

namespace BGTG.POS.Tools.Tests.CalendarPlanTool
{
    public class ConstructionMonthsCreatorTests
    {
        private ConstructionMonthsCreator _constructionMonthsCreator;

        [SetUp]
        public void SetUp()
        {
            _constructionMonthsCreator = new ConstructionMonthsCreator();
        }

        [Test]
        public void Create_Estimate548VATElectrochemicalWork_CorrectConstructionMonths()
        {
            var constructionStartDate = DateTime.Today;
            var percentages = new List<decimal> { 0.3M, 0.5M, 0.2M };

            var estimateWork = EstimateSource.Estimate548VAT.MainEstimateWorks.Single(x => x.WorkName == "Электрохимическая защита");

            var expectedConstructionMonths = new List<ConstructionMonth>
            {
                new ConstructionMonth(constructionStartDate, 0.1896M, 0.1776M, 0.3M, 0),
                new ConstructionMonth(constructionStartDate.AddMonths(1), 0.316M, 0.296M, 0.5M, 1),
                new ConstructionMonth(constructionStartDate.AddMonths(2), 0.1264M, 0.1184M, 0.2M, 2),
            };

            var actualConstructionMonths = _constructionMonthsCreator.Create(constructionStartDate, estimateWork.TotalCost, estimateWork.TotalCost - estimateWork.EquipmentCost - estimateWork.OtherProductsCost, percentages);

            Assert.That(actualConstructionMonths, Is.EquivalentTo(expectedConstructionMonths));
        }
    }
}
