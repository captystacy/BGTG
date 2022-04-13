using POS.Models.EstimateModels;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using POS.Infrastructure.AppConstants;

namespace POS.ViewModels
{
    public class CalendarPlanViewModel : IValidatableObject, IEquatable<CalendarPlanViewModel>
    {
        [JsonIgnore] public IFormFileCollection EstimateFiles { get; set; } = null!;
        public DateTime ConstructionStartDate { get; set; }
        public decimal ConstructionDuration { get; set; }
        public List<CalendarWorkViewModel> PreparatoryCalendarWorks { get; set; } = null!;
        public List<CalendarWorkViewModel> MainCalendarWorks { get; set; } = null!;
        public TotalWorkChapter TotalWorkChapter { get; set; }

        public override string ToString()
        {
            return $"{nameof(EstimateFiles)}: {EstimateFiles}, {nameof(ConstructionStartDate)}: {ConstructionStartDate}, {nameof(ConstructionDuration)}: {ConstructionDuration}, {nameof(MainCalendarWorks)}: {MainCalendarWorks}, {nameof(TotalWorkChapter)}: {TotalWorkChapter}";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EstimateFiles.Count == 0)
            {
                yield return new ValidationResult("Estimate files was not found");
            }

            if (ConstructionStartDate.Year <= 1900)
            {
                yield return new ValidationResult("Construction start date year could not be less than 1900");
            }

            if (PreparatoryCalendarWorks.Count < 0)
            {
                yield return new ValidationResult("Preparatory calendar works were empty");
            }

            if (!PreparatoryCalendarWorks.Exists(x => x.WorkName == Constants.PreparatoryTemporaryBuildingsWorkName))
            {
                yield return new ValidationResult("TemporaryBuildings calendar work was not found");
            }

            if (MainCalendarWorks.Count < 0)
            {
                yield return new ValidationResult("Main calendar works were empty");
            }

            if (!MainCalendarWorks.Exists(x => x.WorkName == Constants.MainOtherExpensesWorkName))
            {
                yield return new ValidationResult("Other expenses work was not found");
            }
        }

        public bool Equals(CalendarPlanViewModel? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(EstimateFiles, other.EstimateFiles) &&
                   ConstructionStartDate.Equals(other.ConstructionStartDate) &&
                   ConstructionDuration == other.ConstructionDuration &&
                   MainCalendarWorks.SequenceEqual(other.MainCalendarWorks) &&
                   TotalWorkChapter == other.TotalWorkChapter;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CalendarPlanViewModel)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EstimateFiles, ConstructionStartDate, ConstructionDuration, MainCalendarWorks, (int)TotalWorkChapter);
        }

        public static bool operator ==(CalendarPlanViewModel left, CalendarPlanViewModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CalendarPlanViewModel left, CalendarPlanViewModel right)
        {
            return !Equals(left, right);
        }
    }
}