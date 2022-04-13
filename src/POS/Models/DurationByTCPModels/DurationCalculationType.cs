using System.ComponentModel.DataAnnotations;
using POS.Infrastructure.AppConstants;

namespace POS.Models.DurationByTCPModels
{
    public enum DurationCalculationType
    {
        [Display(Name = Constants.InterpolationStr)]
        Interpolation = 0,
        [Display(Name = Constants.ExtrapolationStr)]
        ExtrapolationAscending = 1,
        [Display(Name = Constants.ExtrapolationStr)]
        ExtrapolationDescending = 2,
        [Display(Name = Constants.StepwiseExtrapolationStr)]
        StepwiseExtrapolationAscending = 3,
        [Display(Name = Constants.StepwiseExtrapolationStr)]
        StepwiseExtrapolationDescending = 4,
    }
}