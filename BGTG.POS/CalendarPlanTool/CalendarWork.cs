using System;
using System.Collections.Generic;
using System.Linq;

namespace BGTG.POS.CalendarPlanTool
{
    public class CalendarWork : IEquatable<CalendarWork>
    {
        public string WorkName { get; }
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
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return WorkName == other.WorkName && TotalCost == other.TotalCost &&
                   TotalCostIncludingCAIW == other.TotalCostIncludingCAIW &&
                   ConstructionMonths.SequenceEqual(other.ConstructionMonths) && EstimateChapter == other.EstimateChapter;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CalendarWork)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(WorkName, TotalCost, TotalCostIncludingCAIW, ConstructionMonths, EstimateChapter);
        }

        public static bool operator ==(CalendarWork left, CalendarWork right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CalendarWork left, CalendarWork right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{nameof(WorkName)}: {WorkName}, {nameof(TotalCost)}: {TotalCost}, {nameof(TotalCostIncludingCAIW)}: {TotalCostIncludingCAIW}, {nameof(ConstructionMonths)}: {ConstructionMonths}, {nameof(EstimateChapter)}: {EstimateChapter}";
        }
    }
}
