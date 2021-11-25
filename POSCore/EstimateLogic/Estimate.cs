using System.Collections.Generic;

namespace POSCore.EstimateLogic
{
    public class Estimate
    {
        public List<EstimateWork> EstimateWorks { get; }

        public Estimate(List<EstimateWork> estimateWorks)
        {
            EstimateWorks = estimateWorks;
        }
    }
}
