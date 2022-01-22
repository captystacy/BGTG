﻿using System;
using System.Collections.Generic;
using System.Linq;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.POS.Tools.EstimateTool
{
    public class Estimate : Identity, IEquatable<Estimate>
    {
        public string ObjectCipher { get; set; }
        public int LaborCosts { get; }
        public DateTime ConstructionStartDate { get; set; }
        public decimal ConstructionDuration { get; set; }
        public int ConstructionDurationCeiling { get; set; }
        public IEnumerable<EstimateWork> PreparatoryEstimateWorks { get; }
        public IEnumerable<EstimateWork> MainEstimateWorks { get; }

        public Estimate(IEnumerable<EstimateWork> preparatoryEstimateWorks,
            IEnumerable<EstimateWork> mainEstimateWorks,
            DateTime constructionStartDate,
            decimal constructionDuration,
            int constructionDurationCeiling,
            string objectCipher,
            int laborCosts)
        {
            PreparatoryEstimateWorks = preparatoryEstimateWorks;
            MainEstimateWorks = mainEstimateWorks;
            ConstructionStartDate = constructionStartDate;
            ConstructionDuration = constructionDuration;
            ConstructionDurationCeiling = constructionDurationCeiling;
            ObjectCipher = objectCipher;
            LaborCosts = laborCosts;
        }

        public bool Equals(Estimate other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ObjectCipher == other.ObjectCipher && LaborCosts == other.LaborCosts &&
                   ConstructionStartDate.Equals(other.ConstructionStartDate) &&
                   ConstructionDuration == other.ConstructionDuration &&
                   ConstructionDurationCeiling == other.ConstructionDurationCeiling &&
                   PreparatoryEstimateWorks.SequenceEqual(other.PreparatoryEstimateWorks) &&
                   MainEstimateWorks.SequenceEqual(other.MainEstimateWorks);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Estimate)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ObjectCipher, LaborCosts, ConstructionStartDate, ConstructionDuration, ConstructionDurationCeiling, PreparatoryEstimateWorks, MainEstimateWorks);
        }

        public static bool operator ==(Estimate left, Estimate right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Estimate left, Estimate right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{nameof(ObjectCipher)}: {ObjectCipher}, {nameof(LaborCosts)}: {LaborCosts}, {nameof(ConstructionStartDate)}: {ConstructionStartDate}, {nameof(ConstructionDuration)}: {ConstructionDuration}, {nameof(ConstructionDurationCeiling)}: {ConstructionDurationCeiling}, {nameof(PreparatoryEstimateWorks)}: {PreparatoryEstimateWorks}, {nameof(MainEstimateWorks)}: {MainEstimateWorks}";
        }
    }
}