using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using POS.Infrastructure.Tools.CalendarPlanTool;
using POS.Infrastructure.Tools.CalendarPlanTool.Base;
using POS.Infrastructure.Tools.CalendarPlanTool.Models;
using POS.Infrastructure.Tools.EstimateTool.Models;
using POS.Tests.Infrastructure.Tools.EstimateTool;

namespace POS.Tests.Infrastructure.Tools.CalendarPlanTool;

public class CalendarWorksProviderTests
{
    private CalendarWorksProvider _calendarWorksProvider = null!;
    private Mock<ICalendarWorkCreator> _calendarWorkCreatorMock = null!;

    [SetUp]
    public void SetUp()
    {
        _calendarWorkCreatorMock = new Mock<ICalendarWorkCreator>();
        _calendarWorksProvider = new CalendarWorksProvider(_calendarWorkCreatorMock.Object);
    }

    [Test]
    public void CreatePreparatoryCalendarWorks_Estimate548VATPreparatoryWorks_CorrectPreparatoryCalendarWorks()
    {
        var preparatoryPercentages = AppData.PreparatoryPercentages.ToList();
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
            .Setup(x => x.CreateAnyPreparatoryWork(AppData.PreparatoryWorkName, calendarWorksChapter1, AppData.PreparatoryWorkChapter, constructionStartDate, preparatoryPercentages))
            .Returns(calendarWorksChapter1[0]);

        var temporaryBuildingsWork = expectedCalendarWorks.First(x => x.WorkName == AppData.PreparatoryTemporaryBuildingsWorkName);
        var calendarWorksChapter8 = middleCalendarWorks.FindAll(x => x.EstimateChapter == 8);
        _calendarWorkCreatorMock
            .Setup(x => x.CreateAnyPreparatoryWork(AppData.PreparatoryTemporaryBuildingsWorkName, calendarWorksChapter8, AppData.PreparatoryTemporaryBuildingsWorkChapter, constructionStartDate, preparatoryPercentages))
            .Returns(temporaryBuildingsWork);

        var totalWork = expectedCalendarWorks.First(x => x.WorkName == AppData.TotalWorkName);
        _calendarWorkCreatorMock
            .Setup(x => x.CreateAnyPreparatoryWork(AppData.TotalWorkName, new List<CalendarWork> { temporaryBuildingsWork }, AppData.PreparatoryTotalWorkChapter, constructionStartDate, preparatoryPercentages))
            .Returns(totalWork);
        var actualCalendarWorks = _calendarWorksProvider.CreatePreparatoryCalendarWorks(preparatoryEstimateWorks, constructionStartDate);

        Assert.That(actualCalendarWorks, Is.EquivalentTo(expectedCalendarWorks));
        for (int i = 0; i < preparatoryEstimateWorks.Count; i++)
        {
            _calendarWorkCreatorMock.Verify(x => x.Create(preparatoryEstimateWorks[i], constructionStartDate), Times.Once);
        }
        _calendarWorkCreatorMock.Verify(x => x.CreateAnyPreparatoryWork(AppData.PreparatoryWorkName, calendarWorksChapter1, AppData.PreparatoryWorkChapter, constructionStartDate, preparatoryPercentages), Times.Once);
        _calendarWorkCreatorMock.Verify(x => x.CreateAnyPreparatoryWork(AppData.PreparatoryTemporaryBuildingsWorkName, calendarWorksChapter8, AppData.PreparatoryTemporaryBuildingsWorkChapter, constructionStartDate, preparatoryPercentages), Times.Once);
        _calendarWorkCreatorMock.Verify(x => x.CreateAnyPreparatoryWork(AppData.TotalWorkName, expectedCalendarWorks, AppData.PreparatoryTotalWorkChapter, constructionStartDate, preparatoryPercentages), Times.Once);
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
        var preparatoryTotalWork = CalendarPlanSource.CalendarPlan548.PreparatoryCalendarWorks.First(x => x.WorkName == AppData.TotalWorkName);
        var expectedCalendarWorks = CalendarPlanSource.CalendarPlan548.MainCalendarWorks.ToList();

        for (int i = 0; i < estimateWorks.Count; i++)
        {
            _calendarWorkCreatorMock.Setup(x => x.Create(estimateWorks[i], constructionStartDate)).Returns(middleCalendarWorks[i]);
        }

        var overallPreparatoryWork = new CalendarWork(AppData.MainOverallPreparatoryWorkName, preparatoryTotalWork.TotalCost, preparatoryTotalWork.TotalCostIncludingCAIW, preparatoryTotalWork.ConstructionMonths, AppData.MainOverallPreparatoryTotalWorkChapter);
        var electroChemichalWork = middleCalendarWorks.First(x => x.WorkName == "Электрохимическая защита");
        var landscapingWork = middleCalendarWorks.First(x => x.WorkName == "Благоустройство территории");

        var otherExpensesWork = expectedCalendarWorks.First(x => x.WorkName == AppData.MainOtherExpensesWorkName);
        _calendarWorkCreatorMock
            .Setup(x => x.CreateOtherExpensesWork(new List<CalendarWork> { overallPreparatoryWork, electroChemichalWork, landscapingWork }, initialTotalWork, constructionStartDate, otherExpensesPercentages))
            .Returns(otherExpensesWork);

        var mainTotalWork = expectedCalendarWorks.First(x => x.WorkName == AppData.TotalWorkName);
        _calendarWorkCreatorMock
            .Setup(x => x.CreateMainTotalWork(new List<CalendarWork> { overallPreparatoryWork, electroChemichalWork, landscapingWork, otherExpensesWork }, initialTotalWork, constructionStartDate, constructionDurationCeiling))
            .Returns(mainTotalWork);

        var actualCalendarWorks = _calendarWorksProvider.CreateMainCalendarWorks(estimateWorks, preparatoryTotalWork, constructionStartDate, constructionDurationCeiling, otherExpensesPercentages, TotalWorkChapter.TotalWork1To12Chapter);

        Assert.That(actualCalendarWorks, Is.EquivalentTo(expectedCalendarWorks));
        for (int i = 0; i < estimateWorks.Count; i++)
        {
            _calendarWorkCreatorMock.Verify(x => x.Create(estimateWorks[i], constructionStartDate), Times.Once);
        }
        _calendarWorkCreatorMock.Verify(x => x.CreateOtherExpensesWork(expectedCalendarWorks, initialTotalWork, constructionStartDate, otherExpensesPercentages), Times.Once);
        _calendarWorkCreatorMock.Verify(x => x.CreateMainTotalWork(expectedCalendarWorks, initialTotalWork, constructionStartDate, constructionDurationCeiling), Times.Once);
    }
}