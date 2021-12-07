using System;
using System.Collections.Generic;

namespace POSCore.EstimateLogic
{
    public class Estimate
    {
        public string ObjectCipher { get; set; }
        public DateTime ConstructionStartDate { get; }
        public decimal ConstructionDuration { get; }
        public List<EstimateWork> EstimateWorks { get; }

        public Estimate(List<EstimateWork> estimateWorks, DateTime constructionStartDate, decimal constructionDuration, string objectCipher)
        {
            EstimateWorks = estimateWorks;
            ConstructionStartDate = constructionStartDate;
            ConstructionDuration = constructionDuration;
            ObjectCipher = objectCipher;
        }
    }
}
