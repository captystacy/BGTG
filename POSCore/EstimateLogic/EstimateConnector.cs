using POSCore.EstimateLogic.Interfaces;
using System;

namespace POSCore.EstimateLogic
{
    public class EstimateConnector : IEstimateConnector
    {
        public IEstimate Connect(IEstimate estimateVatFree, IEstimate estimateVat)
        {
            throw new NotImplementedException();
        }
    }
}
