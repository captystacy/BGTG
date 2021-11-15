using System.Collections.Generic;

namespace POSCore.EstimateLogic.Interfaces
{
    public interface IEstimate
    {
        IEnumerable<IEstimateWork> EstimateWorks { get; }
    }
}
