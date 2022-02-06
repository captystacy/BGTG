using System;
using System.Collections.Generic;
using System.Linq;

namespace BGTG.POS.DurationTools.DurationByTCPTool.TCP
{
    public class PipelineComponent : IEquatable<PipelineComponent>
    {
        public IEnumerable<string> PipelineMaterials { get; }
        public IEnumerable<PipelineCharacteristic> PipelineCharacteristics { get; }

        public PipelineComponent(IEnumerable<string> pipelineMaterials, IEnumerable<PipelineCharacteristic> pipelineCharacteristics)
        {
            PipelineMaterials = pipelineMaterials;
            PipelineCharacteristics = pipelineCharacteristics;
        }

        public bool Equals(PipelineComponent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return PipelineMaterials.SequenceEqual(other.PipelineMaterials) &&
                   PipelineCharacteristics.SequenceEqual(other.PipelineCharacteristics);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PipelineComponent)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PipelineMaterials, PipelineCharacteristics);
        }

        public static bool operator ==(PipelineComponent left, PipelineComponent right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PipelineComponent left, PipelineComponent right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{nameof(PipelineMaterials)}: {PipelineMaterials}, {nameof(PipelineCharacteristics)}: {PipelineCharacteristics}";
        }
    }
}
