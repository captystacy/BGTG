namespace POS.Infrastructure.Tools.EstimateTool.Models;

public class EstimateWork : IEquatable<EstimateWork>
{
    public string WorkName { get; }
    public int Chapter { get; }
    public decimal EquipmentCost { get; }
    public decimal OtherProductsCost { get; }
    public decimal TotalCost { get; }
    public List<decimal> Percentages { get; }

    public EstimateWork(string workName, decimal equipmentCost, decimal otherProductsCost, decimal totalCost, int chapter)
    {
        WorkName = workName;
        EquipmentCost = equipmentCost;
        OtherProductsCost = otherProductsCost;
        TotalCost = totalCost;
        Chapter = chapter;
        Percentages = new List<decimal>();
    }

    public bool Equals(EstimateWork? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return WorkName == other.WorkName && Chapter == other.Chapter && EquipmentCost == other.EquipmentCost &&
               OtherProductsCost == other.OtherProductsCost && TotalCost == other.TotalCost;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((EstimateWork)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(WorkName, Chapter, EquipmentCost, OtherProductsCost, TotalCost, Percentages);
    }

    public static bool operator ==(EstimateWork left, EstimateWork right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(EstimateWork left, EstimateWork right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"{nameof(WorkName)}: {WorkName}, {nameof(Chapter)}: {Chapter}, {nameof(EquipmentCost)}: {EquipmentCost}, {nameof(OtherProductsCost)}: {OtherProductsCost}, {nameof(TotalCost)}: {TotalCost}, {nameof(Percentages)}: {Percentages}";
    }
}