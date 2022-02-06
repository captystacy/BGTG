using System;
using System.Collections.Generic;
using System.Linq;
using BGTG.POS.CalendarPlanTool.Interfaces;
using BGTG.POS.EstimateTool;

namespace BGTG.POS.CalendarPlanTool
{
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
            var preparatoryPercentages = CalendarPlanInfo.PreparatoryPercentages.ToList();
            var calendarWorks = preparatoryEstimateWorks
                .Select(x => _calendarWorkCreator.Create(x, constructionStartDate)).ToList();

            var calendarWorksChapter1 =
                calendarWorks.FindAll(x => x.EstimateChapter == CalendarPlanInfo.PreparatoryWorkChapter);
            if (calendarWorksChapter1.Count != 0)
            {
                var preparatoryCalendarWork = _calendarWorkCreator.CreateAnyPreparatoryWork(
                    CalendarPlanInfo.PreparatoryWorkName, calendarWorksChapter1,
                    CalendarPlanInfo.PreparatoryWorkChapter, constructionStartDate, preparatoryPercentages);

                if (preparatoryCalendarWork.TotalCostIncludingCAIW > 0)
                {
                    preparatoryCalendarWorks.Add(preparatoryCalendarWork);
                }
            }

            var calendarWorksChapter8 = calendarWorks.FindAll(x =>
                x.EstimateChapter == CalendarPlanInfo.PreparatoryTemporaryBuildingsWorkChapter);

            var temporaryBuildingsWork = _calendarWorkCreator.CreateAnyPreparatoryWork(
                CalendarPlanInfo.PreparatoryTemporaryBuildingsWorkName, calendarWorksChapter8,
                CalendarPlanInfo.PreparatoryTemporaryBuildingsWorkChapter, constructionStartDate,
                preparatoryPercentages);
            preparatoryCalendarWorks.Add(temporaryBuildingsWork);

            var totalCalendarWork = _calendarWorkCreator.CreateAnyPreparatoryWork(CalendarPlanInfo.TotalWorkName,
                preparatoryCalendarWorks, CalendarPlanInfo.PreparatoryTotalWorkChapter, constructionStartDate,
                preparatoryPercentages);
            preparatoryCalendarWorks.Add(totalCalendarWork);

            return preparatoryCalendarWorks;
        }


        public List<CalendarWork> CreateMainCalendarWorks(IEnumerable<EstimateWork> estimateWorks, CalendarWork preparatoryTotalWork, DateTime constructionStartDate, int constructionDurationCeiling, List<decimal> otherExpensesPercentages, TotalWorkChapter totalWorkChapter)
        {
            var mainCalendarWorks = estimateWorks.Select(x => _calendarWorkCreator.Create(x, constructionStartDate))
                .ToList();

            var initialTotalMainWork =
                mainCalendarWorks.Find(x => x.EstimateChapter == (int)totalWorkChapter);
            mainCalendarWorks.Remove(initialTotalMainWork);

            var mainOverallPreparatoryWork = new CalendarWork(CalendarPlanInfo.MainOverallPreparatoryWorkName,
                preparatoryTotalWork.TotalCost, preparatoryTotalWork.TotalCostIncludingCAIW,
                preparatoryTotalWork.ConstructionMonths, CalendarPlanInfo.MainOverallPreparatoryTotalWorkChapter);
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
}
