using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Models;

namespace POS.Infrastructure.Replacers.Base
{
    public interface ITechnicalAndEconomicalIndicatorsReplacer
    {
        Task Replace(IMyWordDocument document, DurationByLC durationByLC);
    }
}