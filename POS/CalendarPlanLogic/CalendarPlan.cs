using System;
using System.Collections.Generic;
using System.Linq;

namespace POS.CalendarPlanLogic
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
            if (other == null)
            {
                return false;
            }

            return PreparatoryCalendarWorks.SequenceEqual(other.PreparatoryCalendarWorks)
                && MainCalendarWorks.SequenceEqual(other.MainCalendarWorks)
                && ConstructionStartDate == other.ConstructionStartDate
                && ConstructionDurationCeiling == other.ConstructionDurationCeiling;
        }

        public override bool Equals(object obj) => Equals(obj as CalendarPlan);

        public override int GetHashCode() => HashCode.Combine(PreparatoryCalendarWorks, MainCalendarWorks, ConstructionStartDate, ConstructionDurationCeiling);

        public static bool operator ==(CalendarPlan calendarPlan1, CalendarPlan calendarPlan2)
        {
            if (calendarPlan1 is null)
            {
                return calendarPlan2 is null;
            }

            return calendarPlan1.Equals(calendarPlan2);
        }

        public static bool operator !=(CalendarPlan calendarPlan1, CalendarPlan calendarPlan2) => !(calendarPlan1 == calendarPlan2);

        public override string ToString()
        {
            return "[" + string.Join("; ", PreparatoryCalendarWorks) + "], "
                + "[" + string.Join("; ", MainCalendarWorks) + "], "
                + string.Join(", ", ConstructionStartDate, ConstructionDurationCeiling);
        }
    }
}
