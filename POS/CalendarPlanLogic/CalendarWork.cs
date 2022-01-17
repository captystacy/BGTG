using System;
using System.Collections.Generic;
using System.Linq;

namespace POS.CalendarPlanLogic
{
    public class CalendarWork : IEquatable<CalendarWork>
    {
        public string WorkName { get; set; }
        public decimal TotalCost { get; }
        public decimal TotalCostIncludingCAIW { get; }
        public IEnumerable<ConstructionMonth> ConstructionMonths { get; }
        public int EstimateChapter { get; }

        public CalendarWork(string workName, decimal totalCost, decimal totalCostIncludingCAIW, IEnumerable<ConstructionMonth> constructionMonths, int estimateChapter)
        {
            WorkName = workName;
            TotalCost = totalCost;
            TotalCostIncludingCAIW = totalCostIncludingCAIW;
            ConstructionMonths = constructionMonths;
            EstimateChapter = estimateChapter;
        }

        public bool Equals(CalendarWork other)
        {
            if (other == null || !ConstructionMonths.SequenceEqual(other.ConstructionMonths))
            {
                return false;
            }

            return WorkName == other.WorkName
                && TotalCost == other.TotalCost
                && TotalCostIncludingCAIW == other.TotalCostIncludingCAIW
                && EstimateChapter == other.EstimateChapter;
        }

        public override bool Equals(object obj) => Equals(obj as CalendarWork);

        public override int GetHashCode() => HashCode.Combine(WorkName, TotalCost, TotalCostIncludingCAIW,
            ConstructionMonths, EstimateChapter);

        public static bool operator ==(CalendarWork calendarWork1, CalendarWork calendarWork2)
        {
            if (calendarWork1 is null)
            {
                return calendarWork2 is null;
            }

            return calendarWork1.Equals(calendarWork2);
        }

        public static bool operator !=(CalendarWork calendarWork1, CalendarWork calendarWork2) => !(calendarWork1 == calendarWork2);

        public override string ToString()
        {
            return string.Join(", ", WorkName, TotalCost, TotalCostIncludingCAIW, EstimateChapter)
                + (ConstructionMonths.Any() ? ", [" + string.Join(", ", ConstructionMonths) + "]" : string.Empty);
        }
    }
}
