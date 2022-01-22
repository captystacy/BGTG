using System;
using System.Collections.Generic;
using System.Linq;

namespace BGTG.POS.Tools.CalendarPlanTool
{
    public class CalendarPlan : IEquatable<CalendarPlan>
    {
        public IEnumerable<CalendarWork> PreparatoryCalendarWorks { get; }
        public IEnumerable<CalendarWork> MainCalendarWorks { get; }
        public DateTime ConstructionStartDate { get; }
        public int ConstructionDurationCeiling { get; }

        public CalendarPlan(IEnumerable<CalendarWork> preparatoryCalendarWorks, IEnumerable<CalendarWork> mainCalendarWorks, DateTime constructionStartDate, int constructionDurationCeiling)
        {
            PreparatoryCalendarWorks = preparatoryCalendarWorks;
            MainCalendarWorks = mainCalendarWorks;
            ConstructionStartDate = constructionStartDate;
            ConstructionDurationCeiling = constructionDurationCeiling;
        }

        public bool Equals(CalendarPlan other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return PreparatoryCalendarWorks.SequenceEqual(other.PreparatoryCalendarWorks) &&
                   MainCalendarWorks.SequenceEqual(other.MainCalendarWorks) &&
                   ConstructionStartDate.Equals(other.ConstructionStartDate) &&
                   ConstructionDurationCeiling == other.ConstructionDurationCeiling;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CalendarPlan)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PreparatoryCalendarWorks, MainCalendarWorks, ConstructionStartDate, ConstructionDurationCeiling);
        }

        public static bool operator ==(CalendarPlan left, CalendarPlan right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CalendarPlan left, CalendarPlan right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{nameof(PreparatoryCalendarWorks)}: {PreparatoryCalendarWorks}, {nameof(MainCalendarWorks)}: {MainCalendarWorks}, {nameof(ConstructionStartDate)}: {ConstructionStartDate}, {nameof(ConstructionDurationCeiling)}: {ConstructionDurationCeiling}";
        }
    }
}
