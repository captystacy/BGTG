using System.Threading.Tasks;
using BGTG.Entities.POS.CalendarPlanToolEntities;
using BGTG.Entities.POS.DurationByLCToolEntities;
using BGTG.Entities.POS.DurationByTCPToolEntities;
using BGTG.Entities.POS.EnergyAndWaterToolEntities;

namespace BGTG.Web.Infrastructure.Services.BGTG.Base;

public interface IConstructionObjectService
{
    Task Update(string objectCipher, CalendarPlanEntity calendarPlan);
    Task Update(string objectCipher, DurationByLCEntity durationByLC);
    Task Update(string objectCipher, InterpolationDurationByTCPEntity interpolationDurationByTCP);
    Task Update(string objectCipher, ExtrapolationDurationByTCPEntity extrapolationDurationByTCP);
    Task Update(string objectCipher, StepwiseExtrapolationDurationByTCPEntity stepwiseExtrapolationDurationByTCP);
    Task Update(string objectCipher, EnergyAndWaterEntity energyAndWater);
}