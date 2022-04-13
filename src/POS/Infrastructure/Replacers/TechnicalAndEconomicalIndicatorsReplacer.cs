using POS.Infrastructure.Replacers.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Models;

namespace POS.Infrastructure.Replacers
{
    public class TechnicalAndEconomicalIndicatorsReplacer : ITechnicalAndEconomicalIndicatorsReplacer
    {
        private const string TotalDurationPattern = "%TD%";
        private const string PreparatoryPeriodPattern = "%PP%";
        private const string AcceptanceTimePattern = "%AT%";
        private const string TotalLaborCostsPattern = "%TLC%";

        public Task Replace(IMyWordDocument document, DurationByLC durationByLC)
        {
            document.Replace(TotalDurationPattern, durationByLC.TotalDuration.ToString());
            document.Replace(PreparatoryPeriodPattern, durationByLC.PreparatoryPeriod.ToString() );
            document.Replace(AcceptanceTimePattern, durationByLC.AcceptanceTime.ToString());
            document.Replace(TotalLaborCostsPattern, durationByLC.TotalLaborCosts.ToString());

            return Task.CompletedTask;
        }
    }
}
