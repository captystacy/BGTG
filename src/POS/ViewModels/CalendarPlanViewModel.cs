using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using POS.Infrastructure.Tools.EstimateTool.Models;

namespace POS.ViewModels;

public class CalendarPlanViewModel : IValidatableObject, IEquatable<CalendarPlanViewModel>
{
    [JsonIgnore] public IFormFileCollection EstimateFiles { get; set; } = null!;
    public DateTime ConstructionStartDate { get; set; }
    public decimal ConstructionDuration { get; set; }
    public virtual List<CalendarWorkViewModel> CalendarWorks { get; set; } = null!;
    public TotalWorkChapter TotalWorkChapter { get; set; }

    public override string ToString()
    {
        return $"{nameof(EstimateFiles)}: {EstimateFiles}, {nameof(ConstructionStartDate)}: {ConstructionStartDate}, {nameof(ConstructionDuration)}: {ConstructionDuration}, {nameof(CalendarWorks)}: {CalendarWorks}, {nameof(TotalWorkChapter)}: {TotalWorkChapter}";
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EstimateFiles.Count == 0)
        {
            yield return new ValidationResult(AppData.EstimateFilesValidationMessage);
        }

        if (ConstructionStartDate.Year <= 1900)
        {
            yield return new ValidationResult(AppData.ConstructionStartDateValidationMessage);
        }
    }

    public bool Equals(CalendarPlanViewModel? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(EstimateFiles, other.EstimateFiles) &&
               ConstructionStartDate.Equals(other.ConstructionStartDate) &&
               ConstructionDuration == other.ConstructionDuration &&
               CalendarWorks.SequenceEqual(other.CalendarWorks) &&
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
        return HashCode.Combine(EstimateFiles, ConstructionStartDate, ConstructionDuration, CalendarWorks, (int)TotalWorkChapter);
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