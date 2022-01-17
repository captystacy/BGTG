using System;
using System.Collections.Generic;
using System.Linq;

namespace POSCore.EstimateLogic
{
    public class Estimate : IEquatable<Estimate>
    {
        public string ObjectCipher { get; set; }
        public int LaborCosts { get; }
        public DateTime ConstructionStartDate { get; set; }
        public int ConstructionDurationCeiling { get; set; }
        public IEnumerable<EstimateWork> PreparatoryEstimateWorks { get; }
        public IEnumerable<EstimateWork> MainEstimateWorks { get; }

        public Estimate(IEnumerable<EstimateWork> preparatoryEstimateWorks,
            IEnumerable<EstimateWork> mainEstimateWorks,
            DateTime constructionStartDate,
            int constructionDurationCeiling,
            string objectCipher,
            int laborCosts)
        {
            PreparatoryEstimateWorks = preparatoryEstimateWorks;
            MainEstimateWorks = mainEstimateWorks;
            ConstructionStartDate = constructionStartDate;
            ConstructionDurationCeiling = constructionDurationCeiling;
            ObjectCipher = objectCipher;
            LaborCosts = laborCosts;
        }

        public bool Equals(Estimate other)
        {
            if (other == null
                || !PreparatoryEstimateWorks.SequenceEqual(other.PreparatoryEstimateWorks)
                || !MainEstimateWorks.SequenceEqual(other.MainEstimateWorks))
            {
                return false;
            }

            return ObjectCipher == other.ObjectCipher
                   && LaborCosts == other.LaborCosts
                   && ConstructionStartDate == other.ConstructionStartDate
                   && ConstructionDurationCeiling == other.ConstructionDurationCeiling;
        }

        public override bool Equals(object obj) => Equals(obj as Estimate);

        public override int GetHashCode() => HashCode.Combine(ObjectCipher, LaborCosts, ConstructionStartDate, ConstructionDurationCeiling,
            PreparatoryEstimateWorks, MainEstimateWorks);

        public static bool operator ==(Estimate estimate1, Estimate estimate2)
        {
            if (estimate1 is null)
            {
                return estimate2 is null;
            }

            return estimate1.Equals(estimate2);
        }

        public static bool operator !=(Estimate estimate1, Estimate estimate2) => !(estimate1 == estimate2);

        public override string ToString()
        {
            return string.Join(", ", ObjectCipher, LaborCosts, ConstructionStartDate, ConstructionDurationCeiling);
        }
    }
}
