using System.Collections.Generic;

namespace POSCore.EstimateLogic
{
    public class Estimate
    {
        public IEnumerable<EstimateWork> EstimateWorks { get; }

        public Estimate(IEnumerable<EstimateWork> estimateWorks)
        {
            EstimateWorks = estimateWorks;
        }
    }
}
