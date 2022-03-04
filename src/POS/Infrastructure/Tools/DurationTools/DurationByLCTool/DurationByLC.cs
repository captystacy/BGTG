namespace POS.Infrastructure.Tools.DurationTools.DurationByLCTool;

public class DurationByLC : IEquatable<DurationByLC>
{
    public decimal Duration { get; }
    public decimal TotalLaborCosts { get; }
    public decimal EstimateLaborCosts { get; }
    public virtual decimal TechnologicalLaborCosts { get; }
    public decimal WorkingDayDuration { get; }
    public decimal Shift { get; }
    public decimal NumberOfWorkingDays { get; }
    public int NumberOfEmployees { get; }
    public decimal RoundedDuration { get; }
    public decimal TotalDuration { get; }
    public decimal PreparatoryPeriod { get; }
    public decimal AcceptanceTime { get; }
    public bool AcceptanceTimeIncluded { get; }
    public bool RoundingIncluded { get; }

    public DurationByLC(decimal duration, decimal totalLaborCosts, decimal estimateLaborCosts, decimal technologicalLaborCosts, decimal workingDayDuration, decimal shift,
        decimal numberOfWorkingDays, int numberOfEmployees, decimal totalDuration, decimal preparatoryPeriod, decimal roundedDuration,
        decimal acceptanceTime, bool acceptanceTimeIncluded, bool roundingIncluded)
    {
        Duration = duration;
        EstimateLaborCosts = estimateLaborCosts;
        TechnologicalLaborCosts = technologicalLaborCosts;
        TotalLaborCosts = totalLaborCosts;
        WorkingDayDuration = workingDayDuration;
        Shift = shift;
        NumberOfWorkingDays = numberOfWorkingDays;
        NumberOfEmployees = numberOfEmployees;
        RoundedDuration = roundedDuration;
        TotalDuration = totalDuration;
        PreparatoryPeriod = preparatoryPeriod;
        AcceptanceTime = acceptanceTime;
        AcceptanceTimeIncluded = acceptanceTimeIncluded;
        RoundingIncluded = roundingIncluded;
    }

    public bool Equals(DurationByLC? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Duration == other.Duration &&
               TotalLaborCosts == other.TotalLaborCosts && EstimateLaborCosts == other.EstimateLaborCosts &&
               TechnologicalLaborCosts == other.TechnologicalLaborCosts &&
               WorkingDayDuration == other.WorkingDayDuration && Shift == other.Shift &&
               NumberOfWorkingDays == other.NumberOfWorkingDays && NumberOfEmployees == other.NumberOfEmployees &&
               RoundedDuration == other.RoundedDuration && TotalDuration == other.TotalDuration &&
               PreparatoryPeriod == other.PreparatoryPeriod && AcceptanceTime == other.AcceptanceTime &&
               AcceptanceTimeIncluded == other.AcceptanceTimeIncluded && RoundingIncluded == other.RoundingIncluded;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((DurationByLC) obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Duration);
        hashCode.Add(TotalLaborCosts);
        hashCode.Add(EstimateLaborCosts);
        hashCode.Add(TechnologicalLaborCosts);
        hashCode.Add(WorkingDayDuration);
        hashCode.Add(Shift);
        hashCode.Add(NumberOfWorkingDays);
        hashCode.Add(NumberOfEmployees);
        hashCode.Add(RoundedDuration);
        hashCode.Add(TotalDuration);
        hashCode.Add(PreparatoryPeriod);
        hashCode.Add(AcceptanceTime);
        hashCode.Add(AcceptanceTimeIncluded);
        hashCode.Add(RoundingIncluded);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(DurationByLC left, DurationByLC right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(DurationByLC left, DurationByLC right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"{nameof(Duration)}: {Duration}, {nameof(TotalLaborCosts)}: {TotalLaborCosts}, {nameof(EstimateLaborCosts)}: {EstimateLaborCosts}, {nameof(TechnologicalLaborCosts)}: {TechnologicalLaborCosts}, {nameof(WorkingDayDuration)}: {WorkingDayDuration}, {nameof(Shift)}: {Shift}, {nameof(NumberOfWorkingDays)}: {NumberOfWorkingDays}, {nameof(NumberOfEmployees)}: {NumberOfEmployees}, {nameof(RoundedDuration)}: {RoundedDuration}, {nameof(TotalDuration)}: {TotalDuration}, {nameof(PreparatoryPeriod)}: {PreparatoryPeriod}, {nameof(AcceptanceTime)}: {AcceptanceTime}, {nameof(AcceptanceTimeIncluded)}: {AcceptanceTimeIncluded}, {nameof(RoundingIncluded)}: {RoundingIncluded}";
    }
}