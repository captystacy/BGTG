using System.Collections.Generic;

namespace POS.EstimateLogic.Interfaces
{
    public interface IEstimateConnector
    {
        Estimate Connect(List<Estimate> estimates);
    }
}
