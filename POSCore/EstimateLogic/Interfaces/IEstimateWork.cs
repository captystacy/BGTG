namespace POSCore.EstimateLogic.Interfaces
{
    public interface IEstimateWork
    {
        string WorkName { get; }
        double EquipmentCost { get; }
        double OtherProductsCost { get; }
        double TotalCost { get; }
    }
}
