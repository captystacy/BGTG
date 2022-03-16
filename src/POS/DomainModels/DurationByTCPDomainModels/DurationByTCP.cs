using POS.DomainModels.DurationByTCPDomainModels.TCPDomainModels;

namespace POS.DomainModels.DurationByTCPDomainModels;

public abstract class DurationByTCP : IEquatable<DurationByTCP>
{
    public string PipelineMaterial { get; }
    public int PipelineDiameter { get; }
    public string PipelineDiameterPresentation { get; }
    public decimal PipelineLength { get; }
    public IEnumerable<PipelineStandard> CalculationPipelineStandards { get; }
    public DurationCalculationType DurationCalculationType { get; }
    public decimal Duration { get; }
    public decimal RoundedDuration { get; }
    public decimal PreparatoryPeriod { get; }
    public char AppendixKey { get; }
    public int AppendixPage { get; }

    protected DurationByTCP(string pipelineMaterial, int pipelineDiameter, string pipelineDiameterPresentation, decimal pipelineLength, IEnumerable<PipelineStandard> calculationPipelineStandards, DurationCalculationType durationCalculationType, decimal duration, decimal roundedDuration, decimal preparatoryPeriod, char appendixKey, int appendixPage)
    {
        PipelineMaterial = pipelineMaterial;
        PipelineDiameterPresentation = pipelineDiameterPresentation;
        PipelineLength = pipelineLength;
        CalculationPipelineStandards = calculationPipelineStandards;
        DurationCalculationType = durationCalculationType;
        Duration = duration;
        RoundedDuration = roundedDuration;
        PreparatoryPeriod = preparatoryPeriod;
        AppendixKey = appendixKey;
        AppendixPage = appendixPage;
        PipelineDiameter = pipelineDiameter;
    }

    public bool Equals(DurationByTCP? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return PipelineMaterial == other.PipelineMaterial && PipelineDiameter == other.PipelineDiameter &&
               PipelineDiameterPresentation == other.PipelineDiameterPresentation &&
               PipelineLength == other.PipelineLength &&
               CalculationPipelineStandards.SequenceEqual(other.CalculationPipelineStandards) &&
               DurationCalculationType == other.DurationCalculationType && Duration == other.Duration &&
               RoundedDuration == other.RoundedDuration && PreparatoryPeriod == other.PreparatoryPeriod &&
               AppendixKey == other.AppendixKey && AppendixPage == other.AppendixPage;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((DurationByTCP)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(PipelineMaterial);
        hashCode.Add(PipelineDiameter);
        hashCode.Add(PipelineDiameterPresentation);
        hashCode.Add(PipelineLength);
        hashCode.Add(CalculationPipelineStandards);
        hashCode.Add((int)DurationCalculationType);
        hashCode.Add(Duration);
        hashCode.Add(RoundedDuration);
        hashCode.Add(PreparatoryPeriod);
        hashCode.Add(AppendixKey);
        hashCode.Add(AppendixPage);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(DurationByTCP left, DurationByTCP right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(DurationByTCP left, DurationByTCP right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"{nameof(PipelineMaterial)}: {PipelineMaterial}, {nameof(PipelineDiameter)}: {PipelineDiameter}, {nameof(PipelineDiameterPresentation)}: {PipelineDiameterPresentation}, {nameof(PipelineLength)}: {PipelineLength}, {nameof(CalculationPipelineStandards)}: {CalculationPipelineStandards}, {nameof(DurationCalculationType)}: {DurationCalculationType}, {nameof(Duration)}: {Duration}, {nameof(RoundedDuration)}: {RoundedDuration}, {nameof(PreparatoryPeriod)}: {PreparatoryPeriod}, {nameof(AppendixKey)}: {AppendixKey}, {nameof(AppendixPage)}: {AppendixPage}";
    }
}