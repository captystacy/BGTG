using System;
using System.Collections.Generic;
using BGTG.POS.CalendarPlanTool;

namespace BGTG.POS.Tests.CalendarPlanTool
{
    public static class CalendarPlanSource
    {
        public static readonly CalendarPlan CalendarPlan548 = new(new List<CalendarWork>()
        {
            new("Временные здания и сооружения", 0.017M, 0.017M, new List<ConstructionMonth>
            {
                new(new DateTime(2022, 8, 1), 0.017M, 0.017M, 1, 0)
            }, 8),
            new("Итого:", 0.017M, 0.017M, new List<ConstructionMonth>
            {
                new(new DateTime(2022, 8, 1), 0.017M, 0.017M, 1, 0)
            }, 1),
        }, new List<CalendarWork>
        {
            new("Работы, выполняемые в подготовительный период", 0.017M, 0.017M, new List<ConstructionMonth>
            {
                new (new DateTime(2022, 8, 1), 0.017M, 0.017M, 1, 0)
            }, 2),
            new("Электрохимическая защита", 0.632M, 0.592M, new List<ConstructionMonth>
            {
                new (new DateTime(2022, 8, 1), 0.632M, 0.592M, 1, 0)
            }, 2),
            new("Благоустройство территории", 0.02M, 0.02M, new List<ConstructionMonth>
            {
                new (new DateTime(2022, 8, 1), 0.02M, 0.02M, 1, 0)
            }, 7),
            new("Прочие работы и затраты", 2.557M, 0.02M, new List<ConstructionMonth>
            {
                new (new DateTime(2022, 8, 1), 2.557M, 0.02M, 1, 0)
            }, 10),
            new("Итого:", 3.226M, 0.649M, new List<ConstructionMonth>
            {
                new (new DateTime(2022, 8, 1), 3.226M, 0.649M, 1, 0)
            }, 12),
        }, new DateTime(2022, 8, 1), 0.7M, 1);
    }
}
