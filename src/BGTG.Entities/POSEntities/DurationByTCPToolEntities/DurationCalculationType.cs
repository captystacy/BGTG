using System.ComponentModel.DataAnnotations;

namespace BGTG.Entities.POSEntities.DurationByTCPToolEntities;

public enum DurationCalculationType
{
    [Display(Name = "интерполяции")]
    Interpolation = 0,
    [Display(Name = "экстраполяции")]
    ExtrapolationAscending = 1,
    [Display(Name = "экстраполяции")]
    ExtrapolationDescending = 2,
    [Display(Name = "ступенчатой экстраполяции")]
    StepwiseExtrapolationAscending = 3,
    [Display(Name = "ступенчатой экстраполяции")]
    StepwiseExtrapolationDescending = 4,
}