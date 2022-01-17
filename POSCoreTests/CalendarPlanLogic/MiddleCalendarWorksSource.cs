using POSCore.CalendarPlanLogic;
using System;
using System.Collections.Generic;

namespace POSCoreTests.CalendarPlanLogic
{
    public static class MiddleCalendarWorksSource
    {
        public static readonly IEnumerable<CalendarWork> PreparatoryCalendarWorks548 = new CalendarWork[]
        {
            new CalendarWork("Вынос трассы в натуру", 0.013M, 0, new List<ConstructionMonth>
            {
                new ConstructionMonth(new DateTime(2022, 8, 1), 0.013M, 0, 1, 0)
            }, 1),
            new CalendarWork("Одд на период производства работ", 0.005M, 0.005M, new List<ConstructionMonth>
            {
                new ConstructionMonth(new DateTime(2022, 8, 1), 0.005M, 0.005M, 1, 0)
            }, 8),
            new CalendarWork("Временные здания и сооружения 8,56х0,93 - 7,961%", 0.012M, 0.012M, new List<ConstructionMonth>
            {
                new ConstructionMonth(new DateTime(2022, 8, 1), 0.012M, 0.012M, 1, 0)
            }, 8),
        };

        public static readonly IEnumerable<CalendarWork> MainCalendarWorks548 = new CalendarWork[]
        {
            new CalendarWork("Электрохимическая защита", 0.632M, 0.592M, new List<ConstructionMonth>
            {
                new ConstructionMonth(new DateTime(2022, 8, 1), 0.632M, 0.592M, 1, 0)
            }, 2),
            new CalendarWork("Благоустройство территории", 0.02M, 0.02M, new List<ConstructionMonth>
            {
                new ConstructionMonth(new DateTime(2022, 8, 1), 0.02M, 0.02M, 1, 0)
            }, 7),
            new CalendarWork("Всего по сводному сметному расчету", 3.226M, 0.649M, new List<ConstructionMonth>
            {
                new ConstructionMonth(new DateTime(2022, 8, 1), 3.226M, 0.649M, 1, 0)
            }, 12),
        };
    }
}
