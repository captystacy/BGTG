namespace POS.DomainModels.CalendarPlanDomainModels;

public class CalendarPlan : IEquatable<CalendarPlan>
{
    public IEnumerable<CalendarWork> PreparatoryCalendarWorks { get; }
    public IEnumerable<CalendarWork> MainCalendarWorks { get; }
    public DateTime ConstructionStartDate { get; }
    public decimal ConstructionDuration { get; }
    public int ConstructionDurationCeiling { get; }

    public CalendarPlan(IEnumerable<CalendarWork> preparatoryCalendarWorks, IEnumerable<CalendarWork> mainCalendarWorks, DateTime constructionStartDate, decimal constructionDuration, int constructionDurationCeiling)
    {
        PreparatoryCalendarWorks = preparatoryCalendarWorks;
        MainCalendarWorks = mainCalendarWorks;
        ConstructionStartDate = constructionStartDate;
        ConstructionDuration = constructionDuration;
        ConstructionDurationCeiling = constructionDurationCeiling;
    }

    public bool Equals(CalendarPlan? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return PreparatoryCalendarWorks.SequenceEqual(other.PreparatoryCalendarWorks) &&
               MainCalendarWorks.SequenceEqual(other.MainCalendarWorks) &&
               ConstructionStartDate.Equals(other.ConstructionStartDate) &&
               ConstructionDuration == other.ConstructionDuration &&
               ConstructionDurationCeiling == other.ConstructionDurationCeiling;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((CalendarPlan)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PreparatoryCalendarWorks, MainCalendarWorks, ConstructionStartDate, ConstructionDuration, ConstructionDurationCeiling);
    }

    public static bool operator ==(CalendarPlan? left, CalendarPlan? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(CalendarPlan? left, CalendarPlan? right)
    {
        return !Equals(left, right);
    }
}