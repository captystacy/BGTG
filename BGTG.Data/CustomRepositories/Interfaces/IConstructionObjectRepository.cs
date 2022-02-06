using System.Threading.Tasks;
using BGTG.Entities;
using BGTG.Entities.POSEntities.CalendarPlanToolEntities;
using BGTG.Entities.POSEntities.DurationByLCToolEntities;
using BGTG.Entities.POSEntities.DurationByTCPToolEntities;
using BGTG.Entities.POSEntities.EnergyAndWaterToolEntities;
using Calabonga.UnitOfWork;

namespace BGTG.Data.CustomRepositories.Interfaces
{
    public interface IConstructionObjectRepository : IRepository<ConstructionObjectEntity>
    {
        Task Update(string objectCipher, CalendarPlanEntity calendarPlan);
        Task Update(string objectCipher, DurationByLCEntity durationByLC);
        Task Update(string objectCipher, InterpolationDurationByTCPEntity interpolationDurationByTCP);
        Task Update(string objectCipher, ExtrapolationDurationByTCPEntity extrapolationDurationByTCP);
        Task Update(string objectCipher, StepwiseExtrapolationDurationByTCPEntity stepwiseExtrapolationDurationByTCP);
        Task Update(string objectCipher, EnergyAndWaterEntity energyAndWater);
    }
}