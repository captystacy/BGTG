using POS.Infrastructure.Services.DocumentServices.WordService.Base;

namespace POS.Infrastructure.Replacers.Base
{
    public interface IEnergyAndWaterReplacer
    {
        Task Replace(IMyWordDocument document, IMyTable energyAndWaterTable);
    }
}