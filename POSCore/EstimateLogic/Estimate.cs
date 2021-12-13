using System;
using System.Collections.Generic;

namespace POSCore.EstimateLogic
{
    public class Estimate
    {
        public string ObjectCipher { get; set; }
        public int LaborCosts { get; }
        public DateTime ConstructionStartDate { get; }
        public decimal ConstructionDuration { get; }
        public List<EstimateWork> PreparatoryEstimateWorks { get; }
        public List<EstimateWork> MainEstimateWorks { get; }

        public Estimate(List<EstimateWork> preparatoryEstimateWorks, List<EstimateWork> mainEstimateWorks, DateTime constructionStartDate, decimal constructionDuration, string objectCipher, int laborCosts)
        {
            PreparatoryEstimateWorks = preparatoryEstimateWorks;
            MainEstimateWorks = mainEstimateWorks;
            ConstructionStartDate = constructionStartDate;
            ConstructionDuration = constructionDuration;
            ObjectCipher = objectCipher;
            LaborCosts = laborCosts;
        }
    }
}
