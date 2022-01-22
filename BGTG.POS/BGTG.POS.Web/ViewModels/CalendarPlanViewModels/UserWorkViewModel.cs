using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BGTG.POS.Core;

namespace BGTG.POS.Web.ViewModels.CalendarPlanViewModels
{
    public class UserWorkViewModel : IValidatableObject, IEquatable<UserWorkViewModel>
    {
        public virtual string WorkName { get; set; }
        public int Chapter { get; set; }
        public virtual List<decimal> Percentages { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(WorkName))
            {
                yield return new ValidationResult(AppData.Messages.EmptyValuesAreUnacceptable);
            }

            if (Chapter < 0)
            {
                yield return new ValidationResult(AppData.Messages.NegativeValuesAreUnacceptable);
            }
        }

        public bool Equals(UserWorkViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return WorkName == other.WorkName && Chapter == other.Chapter &&
                   Percentages.SequenceEqual(other.Percentages);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserWorkViewModel)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(WorkName, Chapter, Percentages);
        }

        public static bool operator ==(UserWorkViewModel left, UserWorkViewModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UserWorkViewModel left, UserWorkViewModel right)
        {
            return !Equals(left, right);
        }
    }
}
