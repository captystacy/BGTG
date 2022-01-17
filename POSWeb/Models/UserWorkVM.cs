using System;
using System.Collections.Generic;
using System.Linq;
using POSWeb.Models.Attributes;

namespace POSWeb.Models
{
    public class UserWorkVM
    {
        public virtual string WorkName { get; set; }
        public int Chapter { get; set; }
        public virtual List<decimal> Percentages { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as UserWorkVM;

            if (other == null)
            {
                return false;
            }

            return WorkName == other.WorkName
                && Chapter == other.Chapter
                && Percentages.SequenceEqual(other.Percentages);

        }

        public override int GetHashCode() => HashCode.Combine(WorkName, Chapter, Percentages);
    }
}
