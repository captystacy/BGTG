using System.ComponentModel.DataAnnotations;

namespace POS.ViewModels
{
    public class CalendarWorkViewModel : IValidatableObject, IEquatable<CalendarWorkViewModel>
    {
        public string WorkName { get; set; } = null!;
        public int Chapter { get; set; }
        public List<decimal> Percentages { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(WorkName))
            {
                yield return new ValidationResult("Work name could not be empty");
            }

            if (Percentages.Count == 0)
            {
                yield return new ValidationResult("Calendar work percentages were empty");
            }

            if (Percentages.Exists(x => x < 0))
            {
                yield return new ValidationResult("Calendar work percentage could not be less than zero");
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