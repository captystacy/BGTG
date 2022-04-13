using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Models;

namespace POS.Infrastructure.Appenders.Base
{
    public interface IEnergyAndWaterAppender
    {
        Task<IMyTable> AppendAsync(IMySection section, EnergyAndWater energyAndWater);
    }
}