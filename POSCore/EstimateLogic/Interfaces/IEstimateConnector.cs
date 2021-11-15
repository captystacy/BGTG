namespace POSCore.EstimateLogic.Interfaces
{
    public interface IEstimateConnector
    {
        IEstimate Connect(IEstimate estimateVatFree, IEstimate estimateVat);
    }
}
