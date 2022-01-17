using System;
using System.Collections.Generic;
using POS.CalendarPlanLogic;

namespace POSTests.CalendarPlanLogic
{
    public static class CalendarPlanSource
    {
        public readonly static CalendarPlan CalendarPlan548 = new CalendarPlan(new List<CalendarWork>()
        {
            new CalendarWork("Временные здания и сооружения", 0.017M, 0.017M, new List<ConstructionMonth>
            {
                new ConstructionMonth(new DateTime(2022, 8, 1), 0.017M, 0.017M, 1, 0)
            }, 8),
            new CalendarWork("Итого:", 0.017M, 0.017M, new List<ConstructionMonth>
            {
                new ConstructionMonth(new DateTime(2022, 8, 1), 0.017M, 0.017M, 1, 0)
            }, 1),
        }, new List<CalendarWork>
        {
            new CalendarWork("Работы, выполняемые в подготовительный период", 0.017M, 0.017M, new List<ConstructionMonth>
            {
                new ConstructionMonth(new DateTime(2022, 8, 1), 0.017M, 0.017M, 1, 0)
            }, 1),
            new CalendarWork("Электрохимическая защита", 0.632M, 0.592M, new List<ConstructionMonth>
            {
                new ConstructionMonth(new DateTime(2022, 8, 1), 0.632M, 0.592M, 1, 0)
            }, 2),
            new CalendarWork("Благоустройство территории", 0.02M, 0.02M, new List<ConstructionMonth>
            {
                new ConstructionMonth(new DateTime(2022, 8, 1), 0.02M, 0.02M, 1, 0)
            }, 7),
            new CalendarWork("Прочие работы и затраты", 2.557M, 0.02M, new List<ConstructionMonth>
            {
                new ConstructionMonth(new DateTime(2022, 8, 1), 2.557M, 0.02M, 1, 0)
            }, 10),
            new CalendarWork("Итого:", 3.226M, 0.649M, new List<ConstructionMonth>
            {
                new ConstructionMonth(new DateTime(2022, 8, 1), 3.226M, 0.649M, 1, 0)
            }, 12),
        }, new DateTime(2022, 8, 1), 1);
    }
}
