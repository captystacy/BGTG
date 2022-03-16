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

public class CalendarWorksCreatorTests
{
    private CalendarWorksCreator _calendarWorksCreator = null!;
    private Mock<ICalendarWorkCreator> _calendarWorkCreatorMock = null!;

    [SetUp]
    public void SetUp()
    {
        _calendarWorkCreatorMock = new Mock<ICalendarWorkCreator>();
        _calendarWorksCreator = new CalendarWorksCreator(_calendarWorkCreatorMock.Object);
    }

    [Test]
    public void CreatePreparatoryCalendarWorks_Estimate548VATPreparatoryWorks_CorrectPreparatoryCalendarWorks()
    {
        var preparatoryPercentages = AppConstants.PreparatoryPercentages.ToList();
        var constructionStartDate = EstimateSource.Estimate548VAT.ConstructionStartDate;
        var preparatoryEstimateWorks = EstimateSource.Estimate548VAT.PreparatoryEstimateWorks.ToList();
        var middleCalendarWorks = MiddleCalendarWorksSource.PreparatoryCalendarWorks548.ToList();
        var expectedCalendarWorks = CalendarPlanSource.CalendarPlan548.PreparatoryCalendarWorks.ToList();

        for (int i = 0; i < preparatoryEstimateWorks.Count; i++)
        {
            _calendarWorkCreatorMock.Setup(x => x.Create(preparatoryEstimateWorks[i], constructionStartDate)).Returns(middleCalendarWorks[i]);
        }

        var calendarWorksChapter1 = middleCalendarWorks.FindAll(x => x.EstimateChapter == 1);
        _calendarWorkCreatorMock
            .Setup(x => x.CreateAnyPreparatoryWork(AppConstants.PreparatoryWorkName, calendarWorksChapter1, AppConstants.PreparatoryWorkChapter, constructionStartDate, preparatoryPercentages))
            .Returns(calendarWorksChapter1[0]);

        var temporaryBuildingsWork = expectedCalendarWorks.First(x => x.WorkName == AppConstants.PreparatoryTemporaryBuildingsWorkName);
        var calendarWorksChapter8 = middleCalendarWorks.FindAll(x => x.EstimateChapter == 8);
        _calendarWorkCreatorMock
            .Setup(x => x.CreateAnyPreparatoryWork(AppConstants.PreparatoryTemporaryBuildingsWorkName, calendarWorksChapter8, AppConstants.PreparatoryTemporaryBuildingsWorkChapter, constructionStartDate, preparatoryPercentages))
            .Returns(temporaryBuildingsWork);

        var totalWork = expectedCalendarWorks.First(x => x.WorkName == AppConstants.TotalWorkName);
        _calendarWorkCreatorMock
            .Setup(x => x.CreateAnyPreparatoryWork(AppConstants.TotalWorkName, new List<CalendarWork> { temporaryBuildingsWork }, AppConstants.PreparatoryTotalWorkChapter, constructionStartDate, preparatoryPercentages))
            .Returns(totalWork);
        var actualCalendarWorks = _calendarWorksCreator.CreatePreparatoryCalendarWorks(preparatoryEstimateWorks, constructionStartDate);

        Assert.That(actualCalendarWorks, Is.EquivalentTo(expectedCalendarWorks));
        for (int i = 0; i < preparatoryEstimateWorks.Count; i++)
        {
            _calendarWorkCreatorMock.Verify(x => x.Create(preparatoryEstimateWorks[i], constructionStartDate), Times.Once);
        }
        _calendarWorkCreatorMock.Verify(x => x.CreateAnyPreparatoryWork(AppConstants.PreparatoryWorkName, calendarWorksChapter1, AppConstants.PreparatoryWorkChapter, constructionStartDate, preparatoryPercentages), Times.Once);
        _calendarWorkCreatorMock.Verify(x => x.CreateAnyPreparatoryWork(AppConstants.PreparatoryTemporaryBuildingsWorkName, calendarWorksChapter8, AppConstants.PreparatoryTemporaryBuildingsWorkChapter, constructionStartDate, preparatoryPercentages), Times.Once);
        _calendarWorkCreatorMock.Verify(x => x.CreateAnyPreparatoryWork(AppConstants.TotalWorkName, expectedCalendarWorks, AppConstants.PreparatoryTotalWorkChapter, constructionStartDate, preparatoryPercentages), Times.Once);
    }

    [Test]
    public void CreateMainCalendarWorks_Estimate548VATMainWorks_CorrectMainCalendarWorks()
    {
        var otherExpensesPercentages = new List<decimal>();
        var constructionStartDate = EstimateSource.Estimate548VAT.ConstructionStartDate;
        var constructionDurationCeiling = EstimateSource.Estimate548VAT.ConstructionDurationCeiling;
        var estimateWorks = EstimateSource.Estimate548VAT.MainEstimateWorks.ToList();
        var middleCalendarWorks = MiddleCalendarWorksSource.MainCalendarWorks548.ToList();
        var initialTotalWork = middleCalendarWorks.Find(x => x.EstimateChapter == (int)TotalWorkChapter.TotalWork1To12Chapter)!;
        var preparatoryTotalWork = CalendarPlanSource.CalendarPlan548.PreparatoryCalendarWorks.First(x => x.WorkName == AppConstants.TotalWorkName);
        var expectedCalendarWorks = CalendarPlanSource.CalendarPlan548.MainCalendarWorks.ToList();

        for (int i = 0; i < estimateWorks.Count; i++)
        {
            _calendarWorkCreatorMock.Setup(x => x.Create(estimateWorks[i], constructionStartDate)).Returns(middleCalendarWorks[i]);
        }

        var overallPreparatoryWork = new CalendarWork(AppConstants.MainOverallPreparatoryWorkName, preparatoryTotalWork.TotalCost, preparatoryTotalWork.TotalCostIncludingCAIW, preparatoryTotalWork.ConstructionMonths, AppConstants.MainOverallPreparatoryTotalWorkChapter);
        var electroChemichalWork = middleCalendarWorks.First(x => x.WorkName == "Электрохимическая защита");
        var landscapingWork = middleCalendarWorks.First(x => x.WorkName == "Благоустройство территории");

        var otherExpensesWork = expectedCalendarWorks.First(x => x.WorkName == AppConstants.MainOtherExpensesWorkName);
        _calendarWorkCreatorMock
            .Setup(x => x.CreateOtherExpensesWork(new List<CalendarWork> { overallPreparatoryWork, electroChemichalWork, landscapingWork }, initialTotalWork, constructionStartDate, otherExpensesPercentages))
            .Returns(otherExpensesWork);

        var mainTotalWork = expectedCalendarWorks.First(x => x.WorkName == AppConstants.TotalWorkName);
        _calendarWorkCreatorMock
            .Setup(x => x.CreateMainTotalWork(new List<CalendarWork> { overallPreparatoryWork, electroChemichalWork, landscapingWork, otherExpensesWork }, initialTotalWork, constructionStartDate, constructionDurationCeiling))
            .Returns(mainTotalWork);

        var actualCalendarWorks = _calendarWorksCreator.CreateMainCalendarWorks(estimateWorks, preparatoryTotalWork, constructionStartDate, constructionDurationCeiling, otherExpensesPercentages, TotalWorkChapter.TotalWork1To12Chapter);

        Assert.That(actualCalendarWorks, Is.EquivalentTo(expectedCalendarWorks));
        for (int i = 0; i < estimateWorks.Count; i++)
        {
            _calendarWorkCreatorMock.Verify(x => x.Create(estimateWorks[i], constructionStartDate), Times.Once);
        }
        _calendarWorkCreatorMock.Verify(x => x.CreateOtherExpensesWork(expectedCalendarWorks, initialTotalWork, constructionStartDate, otherExpensesPercentages), Times.Once);
        _calendarWorkCreatorMock.Verify(x => x.CreateMainTotalWork(expectedCalendarWorks, initialTotalWork, constructionStartDate, constructionDurationCeiling), Times.Once);
    }
}