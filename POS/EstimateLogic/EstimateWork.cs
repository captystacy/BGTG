using System;
using System.Collections.Generic;
using System.Linq;

namespace POS.EstimateLogic
{
    public class EstimateWork : IEquatable<EstimateWork>
    {
        public string WorkName { get; }
        public int Chapter { get; }
        public decimal EquipmentCost { get; }
        public decimal OtherProductsCost { get; }
        public decimal TotalCost { get; }
        public List<decimal> Percentages { get; }

        public EstimateWork(string workName, decimal equipmentCost, decimal otherProductsCost, decimal totalCost, int chapter)
        {
            WorkName = workName;
            EquipmentCost = equipmentCost;
            OtherProductsCost = otherProductsCost;
            TotalCost = totalCost;
            Chapter = chapter;
            Percentages = new List<decimal>();
        }

        public bool Equals(EstimateWork other)
        {
            if (other == null)
            {
                return false;
            }

            return WorkName == other.WorkName
                && Chapter == other.Chapter
                && EquipmentCost == other.EquipmentCost
                && OtherProductsCost == other.OtherProductsCost
                && TotalCost == other.TotalCost;
        }

        public override bool Equals(object obj) => Equals(obj as EstimateWork);

        public override int GetHashCode() => HashCode.Combine(WorkName, Chapter, EquipmentCost, OtherProductsCost, TotalCost);

        public static bool operator ==(EstimateWork estimateWork1, EstimateWork estimateWork2)
        {
            if (estimateWork1 is null)
            {
                return estimateWork2 is null;
            }

            return estimateWork1.Equals(estimateWork2);
        }

        public static bool operator !=(EstimateWork estimateWork1, EstimateWork estimateWork2) => !(estimateWork1 == estimateWork2);

        public override string ToString()
        {
            return string.Join(", ", WorkName, Chapter, EquipmentCost, OtherProductsCost, TotalCost)
                + (Percentages.Any() ? ", [" + string.Join(", ", Percentages) + "]" : string.Empty);
        }
    }
}
