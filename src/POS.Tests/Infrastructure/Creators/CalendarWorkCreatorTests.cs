using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using POS.DomainModels.CalendarPlanDomainModels;
using POS.DomainModels.EstimateDomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Creators;
using POS.Infrastructure.Creators.Base;

namespace POS.Tests.Infrastructure.Creators;

public class CalendarWorkCreatorTests
{
    private CalendarWorkCreator _calendarWorkCreator = null!;
    private Mock<IConstructionMonthsCreator> _constructionMonthsCreatorMock = null!;

    [SetUp]
    public void SetUp()
    {
        _constructionMonthsCreatorMock = new Mock<IConstructionMonthsCreator>();
        _calendarWorkCreator = new CalendarWorkCreator(_constructionMonthsCreatorMock.Object);
    }

    [Test]
    public void Create_Estimate548VATElectrochemicalProtectionWork_CorrectCalendarWork()
    {
        var constructionStartDate = EstimateSource.Estimate548VAT.ConstructionStartDate;
        var electrochemicalCalendarWorkName = "Электрохимическая защита";
        var constructionMonths = Array.Empty<ConstructionMonth>();
        var estimateWork = EstimateSource.Estimate548VAT.MainEstimateWorks.First(x => x.WorkName == electrochemicalCalendarWorkName);

        var calendarWork = CalendarPlanSource.CalendarPlan548.MainCalendarWorks.First(x => x.WorkName == electrochemicalCalendarWorkName);
        var expectedCalendarWork = new CalendarWork(calendarWork.WorkName, calendarWork.TotalCost, calendarWork.TotalCostIncludingCAIW,
            constructionMonths, calendarWork.EstimateChapter);

        _constructionMonthsCreatorMock.Setup(x => x.Create(constructionStartDate, estimateWork.TotalCost, expectedCalendarWork.TotalCostIncludingCAIW, estimateWork.Percentages)).Returns(constructionMonths);

        var actualCalendarWork = _calendarWorkCreator.Create(estimateWork, constructionStartDate);

        Assert.AreEqual(expectedCalendarWork, actualCalendarWork);
        _constructionMonthsCreatorMock.Verify(x => x.Create(constructionStartDate, estimateWork.TotalCost, expectedCalendarWork.TotalCostIncludingCAIW, estimateWork.Percentages), Times.Once);
    }

    [Test]
    public void CreateAnyPreparatoryWork_Estimate548VATCalendarWorksChapter8_CorrectCalendarWork()
    {
        var constructionStartDate = EstimateSource.Estimate548VAT.ConstructionStartDate;
        var preparatoryPercentages = AppConstants.PreparatoryPercentages.ToList();
        var calendarWorksChapter8 = MiddleCalendarWorksSource.PreparatoryCalendarWorks548.Where(x => x.EstimateChapter == 8).ToList();

        var constructionMonths = Array.Empty<ConstructionMonth>();
        var totalCost = 0.017M;
        var totalCostIncludingCAIW = 0.017M;
        _constructionMonthsCreatorMock.Setup(x => x.Create(constructionStartDate, totalCost, totalCostIncludingCAIW, preparatoryPercentages)).Returns(constructionMonths);

        var calendarWork = CalendarPlanSource.CalendarPlan548.PreparatoryCalendarWorks.First(x => x.WorkName == AppConstants.PreparatoryTemporaryBuildingsWorkName);
        var expectedCalendarWork = new CalendarWork(calendarWork.WorkName, calendarWork.TotalCost, calendarWork.TotalCostIncludingCAIW,
            constructionMonths, calendarWork.EstimateChapter);

        var actualCalendarWork = _calendarWorkCreator.CreateAnyPreparatoryWork(AppConstants.PreparatoryTemporaryBuildingsWorkName, calendarWorksChapter8,
            AppConstants.PreparatoryTemporaryBuildingsWorkChapter, constructionStartDate, preparatoryPercentages);

        Assert.AreEqual(expectedCalendarWork, actualCalendarWork);
        _constructionMonthsCreatorMock.Verify(x => x.Create(constructionStartDate, totalCost, totalCostIncludingCAIW, preparatoryPercentages), Times.Once);
    }

    [Test]
    public void CreateMainTotalWork_Estimate548VATMainCalendarWorks_CorrectCalendarWork()
    {
        var constructionStartDate = EstimateSource.Estimate548VAT.ConstructionStartDate;
        var constructionDurationCeiling = EstimateSource.Estimate548VAT.ConstructionDurationCeiling;
        var mainCalendarWorks = CalendarPlanSource.CalendarPlan548.MainCalendarWorks.ToList();
        var initialMainTotalWork = mainCalendarWorks.Find(x => x.EstimateChapter == (int)TotalWorkChapter.TotalWork1To12Chapter)!;
        mainCalendarWorks.Remove(initialMainTotalWork);

        var expectedCalendarWork = CalendarPlanSource.CalendarPlan548.MainCalendarWorks.First(x => x.WorkName == AppConstants.TotalWorkName);

        var actualCalendarWork = _calendarWorkCreator.CreateMainTotalWork(mainCalendarWorks, initialMainTotalWork, constructionStartDate, constructionDurationCeiling);

        Assert.AreEqual(expectedCalendarWork, actualCalendarWork);
    }

    [Test]
    public void CreateOtherExpensesWork_Estimate548VATMainCalendarWorks_CorrectCalendarWork()
    {
        var constructionStartDate = EstimateSource.Estimate548VAT.ConstructionStartDate;
        var constructionMonths = Array.Empty<ConstructionMonth>();
        var otherExpensesWorkPercentages = new List<decimal> { 1 };
        var mainCalendarWorks = CalendarPlanSource.CalendarPlan548.MainCalendarWorks.ToList();
        var initialMainTotalWork = mainCalendarWorks.Find(x => x.WorkName == AppConstants.TotalWorkName)!;
        mainCalendarWorks.RemoveAll(x => x.WorkName == AppConstants.MainOtherExpensesWorkName
                                         || x.WorkName == AppConstants.TotalWorkName);

        var calendarWork = CalendarPlanSource.CalendarPlan548.MainCalendarWorks.First(x => x.WorkName == AppConstants.MainOtherExpensesWorkName);
        var expectedCalendarWork = new CalendarWork(calendarWork.WorkName, calendarWork.TotalCost, calendarWork.TotalCostIncludingCAIW, constructionMonths, calendarWork.EstimateChapter);

        _constructionMonthsCreatorMock.Setup(x => x.Create(constructionStartDate, expectedCalendarWork.TotalCost, expectedCalendarWork.TotalCostIncludingCAIW, otherExpensesWorkPercentages)).Returns(constructionMonths);

        var actualCalendarWork = _calendarWorkCreator.CreateOtherExpensesWork(mainCalendarWorks, initialMainTotalWork, constructionStartDate, otherExpensesWorkPercentages);

        Assert.AreEqual(expectedCalendarWork, actualCalendarWork);
        _constructionMonthsCreatorMock.Verify(x => x.Create(constructionStartDate, expectedCalendarWork.TotalCost, expectedCalendarWork.TotalCostIncludingCAIW, otherExpensesWorkPercentages), Times.Once);
    }
}