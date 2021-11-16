namespace POSCore.EstimateLogic.Interfaces
{
    public interface IEstimateConnector
    {
        Estimate Connect(Estimate estimateVatFree, Estimate estimateVat);
    }
}
