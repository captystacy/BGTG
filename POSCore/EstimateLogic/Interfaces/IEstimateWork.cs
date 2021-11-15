namespace POSCore.EstimateLogic.Interfaces
{
    public interface IEstimateWork
    {
        double EquipmentCost { get; }
        double OtherProductsCost { get; }
        double TotalCost { get; }
    }
}
