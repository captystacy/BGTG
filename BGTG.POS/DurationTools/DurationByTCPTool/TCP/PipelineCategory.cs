using System;
using System.Collections.Generic;
using System.Linq;

namespace BGTG.POS.DurationTools.DurationByTCPTool.TCP
{
    public class PipelineCategory : IEquatable<PipelineCategory>
    {
        public string Name { get; }
        public IEnumerable<PipelineComponent> PipelineComponents { get; }

        public PipelineCategory(string name, IEnumerable<PipelineComponent> pipelineComponents)
        {
            Name = name;
            PipelineComponents = pipelineComponents;
        }

        public bool Equals(PipelineCategory other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && PipelineComponents.SequenceEqual(other.PipelineComponents);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PipelineCategory) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, PipelineComponents);
        }

        public static bool operator ==(PipelineCategory left, PipelineCategory right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PipelineCategory left, PipelineCategory right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(PipelineComponents)}: {PipelineComponents}";
        }
    }
}