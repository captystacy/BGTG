using System.Collections.Generic;

namespace POSCore.EstimateLogic.Interfaces
{
    public interface IEstimateConnector
    {
        Estimate Connect(List<Estimate> estimates);
    }
}
