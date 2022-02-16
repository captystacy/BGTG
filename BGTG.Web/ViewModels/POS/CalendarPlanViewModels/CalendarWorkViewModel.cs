using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BGTG.Entities.Core;

namespace BGTG.Web.ViewModels.POS.CalendarPlanViewModels
{
    public class CalendarWorkViewModel : IValidatableObject, IEquatable<CalendarWorkViewModel>
    {
        public virtual string WorkName { get; set; } = null!;
        public int Chapter { get; set; }
        public virtual List<decimal> Percentages { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(WorkName))
            {
                yield return new ValidationResult(AppData.WorkNameValidationMessage);
            }

            if (Chapter < 0)
            {
                yield return new ValidationResult(AppData.ChapterValidationMessage);
            }
        }

        public bool Equals(CalendarWorkViewModel? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return WorkName == other.WorkName && Chapter == other.Chapter && Percentages.SequenceEqual(other.Percentages);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CalendarWorkViewModel)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(WorkName, Chapter, Percentages);
        }

        public static bool operator ==(CalendarWorkViewModel left, CalendarWorkViewModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CalendarWorkViewModel left, CalendarWorkViewModel right)
        {
            return !Equals(left, right);
        }
    }
}
