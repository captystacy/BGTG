using POS.Infrastructure.Replacers.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;

namespace POS.Infrastructure.Replacers
{
    public class EnergyAndWaterReplacer : IEnergyAndWaterReplacer
    {
        private const string EnergyAndWaterTablePattern = "%ENERGY_AND_WATER_TABLE%";

        public Task Replace(IMyWordDocument document, IMyTable energyAndWaterTable)
        {
            document.ReplaceTextWithTable(EnergyAndWaterTablePattern, energyAndWaterTable);
            return Task.CompletedTask;
        }
    }
}
