using POS.Infrastructure.Tools.CalendarPlanTool.Base;
using POS.Infrastructure.Tools.CalendarPlanTool.Models;
using POS.Infrastructure.Tools.EstimateTool.Models;

namespace POS.Infrastructure.Tools.CalendarPlanTool;

public class CalendarWorksProvider : ICalendarWorksProvider
{
    private readonly ICalendarWorkCreator _calendarWorkCreator;

    public CalendarWorksProvider(ICalendarWorkCreator calendarWorkCreator)
    {
        _calendarWorkCreator = calendarWorkCreator;
    }

    public List<CalendarWork> CreatePreparatoryCalendarWorks(IEnumerable<EstimateWork> preparatoryEstimateWorks, DateTime constructionStartDate)
    {
        var preparatoryCalendarWorks = new List<CalendarWork>();
        var preparatoryPercentages = AppData.PreparatoryPercentages.ToList();
        var calendarWorks = preparatoryEstimateWorks
            .Select(x => _calendarWorkCreator.Create(x, constructionStartDate)).ToList();

        var calendarWorksChapter1 =
            calendarWorks.FindAll(x => x.EstimateChapter == AppData.PreparatoryWorkChapter);
        if (calendarWorksChapter1.Count != 0)
        {
            var preparatoryCalendarWork = _calendarWorkCreator.CreateAnyPreparatoryWork(
                AppData.PreparatoryWorkName, calendarWorksChapter1,
                AppData.PreparatoryWorkChapter, constructionStartDate, preparatoryPercentages);

            if (preparatoryCalendarWork.TotalCostIncludingCAIW > 0)
            {
                preparatoryCalendarWorks.Add(preparatoryCalendarWork);
            }
        }

        var calendarWorksChapter8 = calendarWorks.FindAll(x =>
            x.EstimateChapter == AppData.PreparatoryTemporaryBuildingsWorkChapter);

        var temporaryBuildingsWork = _calendarWorkCreator.CreateAnyPreparatoryWork(
            AppData.PreparatoryTemporaryBuildingsWorkName, calendarWorksChapter8,
            AppData.PreparatoryTemporaryBuildingsWorkChapter, constructionStartDate,
            preparatoryPercentages);
        preparatoryCalendarWorks.Add(temporaryBuildingsWork);

        var totalCalendarWork = _calendarWorkCreator.CreateAnyPreparatoryWork(AppData.TotalWorkName,
            preparatoryCalendarWorks, AppData.PreparatoryTotalWorkChapter, constructionStartDate,
            preparatoryPercentages);
        preparatoryCalendarWorks.Add(totalCalendarWork);

        return preparatoryCalendarWorks;
    }


    public List<CalendarWork> CreateMainCalendarWorks(IEnumerable<EstimateWork> estimateWorks, CalendarWork preparatoryTotalWork, DateTime constructionStartDate, int constructionDurationCeiling, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter)
    {
        var mainCalendarWorks = estimateWorks.Select(x => _calendarWorkCreator.Create(x, constructionStartDate))
            .ToList();

        var initialTotalMainWork = mainCalendarWorks.Find(x => x.EstimateChapter == (int)totalWorkChapter)!;
        mainCalendarWorks.Remove(initialTotalMainWork);

        var mainOverallPreparatoryWork = new CalendarWork(AppData.MainOverallPreparatoryWorkName,
            preparatoryTotalWork.TotalCost, preparatoryTotalWork.TotalCostIncludingCAIW,
            preparatoryTotalWork.ConstructionMonths, AppData.MainOverallPreparatoryTotalWorkChapter);
        mainCalendarWorks.Insert(0, mainOverallPreparatoryWork);

        var otherExpensesCalendarWork = _calendarWorkCreator.CreateOtherExpensesWork(mainCalendarWorks,
            initialTotalMainWork, constructionStartDate, otherExpensesPercentages);
        mainCalendarWorks.Add(otherExpensesCalendarWork);

        var mainTotalWork = _calendarWorkCreator.CreateMainTotalWork(mainCalendarWorks, initialTotalMainWork,
            constructionStartDate, constructionDurationCeiling);
        mainCalendarWorks.Add(mainTotalWork);

        return mainCalendarWorks;
    }
}