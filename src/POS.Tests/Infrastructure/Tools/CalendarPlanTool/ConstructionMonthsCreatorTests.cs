using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using POS.Infrastructure.Tools.CalendarPlanTool;
using POS.Infrastructure.Tools.CalendarPlanTool.Models;
using POS.Tests.Infrastructure.Tools.EstimateTool;

namespace POS.Tests.Infrastructure.Tools.CalendarPlanTool
{
    public class ConstructionMonthsCreatorTests
    {
        private ConstructionMonthsCreator _constructionMonthsCreator = null!;

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

            var estimateWork = EstimateSource.Estimate548VAT.MainEstimateWorks.First(x => x.WorkName == "Электрохимическая защита");

            var expectedConstructionMonths = new List<ConstructionMonth>
            {
                new(constructionStartDate, 0.1896M, 0.1776M, 0.3M, 0),
                new(constructionStartDate.AddMonths(1), 0.316M, 0.296M, 0.5M, 1),
                new(constructionStartDate.AddMonths(2), 0.1264M, 0.1184M, 0.2M, 2),
            };

            var actualConstructionMonths = _constructionMonthsCreator.Create(constructionStartDate, estimateWork.TotalCost, estimateWork.TotalCost - estimateWork.EquipmentCost - estimateWork.OtherProductsCost, percentages);

            Assert.That(actualConstructionMonths, Is.EquivalentTo(expectedConstructionMonths));
        }
    }
}
