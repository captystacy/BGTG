using System;
using System.Collections.Generic;

namespace POSCore.EstimateLogic
{
    public class Estimate
    {
        public DateTime ConstructionStartDate { get; }
        public decimal ConstructionDuration { get; }
        public List<EstimateWork> EstimateWorks { get; }

        public Estimate(List<EstimateWork> estimateWorks, DateTime constructionStartDate, decimal constructionDuration)
        {
            EstimateWorks = estimateWorks;
            ConstructionStartDate = constructionStartDate;
            ConstructionDuration = constructionDuration;
        }
    }
}
