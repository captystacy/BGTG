using System;
using System.Collections.Generic;
using System.Linq;

namespace POS.DurationLogic.DurationByTCP.TCP
{
    public class PipelineCharacteristic : IEquatable<PipelineCharacteristic>
    {
        public DiameterRange DiameterRange { get; }
        public IEnumerable<PipelineStandard> PipelineStandards { get; }

        public PipelineCharacteristic(DiameterRange diameterRange, IEnumerable<PipelineStandard> pipelineStandards)
        {
            DiameterRange = diameterRange;
            PipelineStandards = pipelineStandards;
        }

        public bool Equals(PipelineCharacteristic other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(DiameterRange, other.DiameterRange) &&
                   PipelineStandards.SequenceEqual(other.PipelineStandards);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PipelineCharacteristic) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DiameterRange, PipelineStandards);
        }

        public static bool operator ==(PipelineCharacteristic left, PipelineCharacteristic right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PipelineCharacteristic left, PipelineCharacteristic right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{nameof(DiameterRange)}: {DiameterRange}, {nameof(PipelineStandards)}: {PipelineStandards}";
        }
    }
}
