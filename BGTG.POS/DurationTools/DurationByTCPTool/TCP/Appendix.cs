using System;
using System.Collections.Generic;

namespace BGTG.POS.DurationTools.DurationByTCPTool.TCP
{
    public class Appendix : IEquatable<Appendix>
    {
        public string Name { get; }
        public int Page { get; }
        public char Key { get; }
        public IEnumerable<PipelineCategory> PipelineCategories { get; }

        public Appendix(string name, int page, char key, IEnumerable<PipelineCategory> pipelineCategories)
        {
            Name = name;
            Page = page;
            Key = key;
            PipelineCategories = pipelineCategories;
        }

        public bool Equals(Appendix? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Page == other.Page && Key == other.Key && Equals(PipelineCategories, other.PipelineCategories);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Appendix)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Page, Key, PipelineCategories);
        }

        public static bool operator ==(Appendix left, Appendix right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Appendix left, Appendix right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Page)}: {Page}, {nameof(Key)}: {Key}, {nameof(PipelineCategories)}: {PipelineCategories}";
        }
    }
}
