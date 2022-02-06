using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using BGTG.Core;
using BGTG.POS.EstimateTool;
using BGTG.Web.ViewModels.POSViewModels.CalendarWorkViewModels;
using Calabonga.EntityFrameworkCore.Entities.Base;
using Microsoft.AspNetCore.Http;

namespace BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels
{
    public class CalendarPlanCreateViewModel : IViewModel, IValidatableObject, IEquatable<CalendarPlanCreateViewModel>
    {
        [JsonIgnore]
        public IFormFileCollection EstimateFiles { get; set; }
        public string ObjectCipher { get; set; }
        public DateTime ConstructionStartDate { get; set; }
        public decimal ConstructionDuration { get; set; }
        public int ConstructionDurationCeiling { get; set; }
        public virtual List<CalendarWorkViewModel> CalendarWorkViewModels { get; set; }
        public TotalWorkChapter TotalWorkChapter { get; set; }

        public override string ToString()
        {
            return $"{nameof(EstimateFiles)}: {EstimateFiles}, {nameof(ObjectCipher)}: {ObjectCipher}, {nameof(ConstructionStartDate)}: {ConstructionStartDate}, {nameof(ConstructionDuration)}: {ConstructionDuration}, {nameof(ConstructionDurationCeiling)}: {ConstructionDurationCeiling}, {nameof(CalendarWorkViewModels)}: {CalendarWorkViewModels}, {nameof(TotalWorkChapter)}: {TotalWorkChapter}";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EstimateFiles == null || EstimateFiles.Count == 0)
            {
                yield return new ValidationResult(AppData.EstimateFilesValidationMessage);
            }

            if (string.IsNullOrEmpty(ObjectCipher) || !Regex.IsMatch(ObjectCipher, AppData.ObjectCipherRegexPattern))
            {
                yield return new ValidationResult(AppData.ObjectCipherValidationMessage);
            }

            if (ConstructionStartDate.Year <= 1900)
            {
                yield return new ValidationResult(AppData.ConstructionStartDateValidationMessage);
            }

            if (ConstructionDurationCeiling < 1 || ConstructionDurationCeiling > 21)
            {
                yield return new ValidationResult(AppData.ConstructionDurationCeilingValidationMessage);
            }
        }

        public bool Equals(CalendarPlanCreateViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(EstimateFiles, other.EstimateFiles) && ObjectCipher == other.ObjectCipher &&
                   ConstructionStartDate.Equals(other.ConstructionStartDate) &&
                   ConstructionDuration == other.ConstructionDuration &&
                   ConstructionDurationCeiling == other.ConstructionDurationCeiling &&
                   CalendarWorkViewModels.SequenceEqual(other.CalendarWorkViewModels) &&
                   TotalWorkChapter == other.TotalWorkChapter;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CalendarPlanCreateViewModel)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EstimateFiles, ObjectCipher, ConstructionStartDate, ConstructionDuration, ConstructionDurationCeiling, CalendarWorkViewModels, (int)TotalWorkChapter);
        }

        public static bool operator ==(CalendarPlanCreateViewModel left, CalendarPlanCreateViewModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CalendarPlanCreateViewModel left, CalendarPlanCreateViewModel right)
        {
            return !Equals(left, right);
        }
    }
}
