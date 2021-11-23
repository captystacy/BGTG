namespace POSCore.EstimateLogic
{
    public class EstimateWork
    {
        public string WorkName { get; }
        public int Chapter { get; }
        public decimal EquipmentCost { get; }
        public decimal OtherProductsCost { get; }
        public decimal TotalCost { get; }

        public EstimateWork(string workName, decimal equipmentCost, decimal otherProductsCost, decimal totalCost, int chapter)
        {
            WorkName = workName;
            EquipmentCost = equipmentCost;
            OtherProductsCost = otherProductsCost;
            TotalCost = totalCost;
            Chapter = chapter;
        }
    }
}
