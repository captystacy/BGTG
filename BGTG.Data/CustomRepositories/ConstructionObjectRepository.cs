using System;
using System.Threading.Tasks;
using BGTG.Data.CustomRepositories.Interfaces;
using BGTG.Entities;
using BGTG.Entities.POSEntities;
using BGTG.Entities.POSEntities.CalendarPlanToolEntities;
using BGTG.Entities.POSEntities.DurationByLCToolEntities;
using BGTG.Entities.POSEntities.DurationByTCPToolEntities;
using BGTG.Entities.POSEntities.EnergyAndWaterToolEntities;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Data.CustomRepositories
{
    public class ConstructionObjectRepository : Repository<ConstructionObjectEntity>, IConstructionObjectRepository
    {
        public ConstructionObjectRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task Update(string objectCipher, CalendarPlanEntity calendarPlan)
        {
            var constructionObjectEntity = await DbSet.Include(x => x.POS).ThenInclude(x => x.CalendarPlan).FirstOrDefaultAsync(x => x.Cipher == objectCipher);

            if (constructionObjectEntity == null)
            {
                constructionObjectEntity = new ConstructionObjectEntity()
                {
                    CreatedBy = calendarPlan.CreatedBy,
                    CreatedAt = calendarPlan.CreatedAt,
                    Cipher = objectCipher,
                    POS = new POSEntity()
                    {
                        CalendarPlan = calendarPlan
                    }
                };
                await DbSet.AddAsync(constructionObjectEntity);
            }
            else
            {
                constructionObjectEntity.POS.CalendarPlan = calendarPlan;
                constructionObjectEntity.UpdatedAt = calendarPlan.CreatedAt;
                constructionObjectEntity.UpdatedBy = calendarPlan.CreatedBy;
                DbSet.Update(constructionObjectEntity);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task Update(string objectCipher, DurationByLCEntity durationByLC)
        {
            var constructionObjectEntity = await DbSet.Include(x => x.POS).ThenInclude(x => x.DurationByLC).FirstOrDefaultAsync(x => x.Cipher == objectCipher);

            if (constructionObjectEntity == null)
            {
                constructionObjectEntity = new ConstructionObjectEntity()
                {
                    CreatedBy = durationByLC.CreatedBy,
                    CreatedAt = durationByLC.CreatedAt,
                    Cipher = objectCipher,
                    POS = new POSEntity()
                    {
                        DurationByLC = durationByLC
                    }
                };
                await DbSet.AddAsync(constructionObjectEntity);
            }
            else
            {
                constructionObjectEntity.POS.DurationByLC = durationByLC;
                constructionObjectEntity.UpdatedAt = durationByLC.CreatedAt;
                constructionObjectEntity.UpdatedBy = durationByLC.CreatedBy;
                DbSet.Update(constructionObjectEntity);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task Update(string objectCipher, InterpolationDurationByTCPEntity interpolationDurationByTCP)
        {
            var constructionObjectEntity = await DbSet.Include(x => x.POS).ThenInclude(x => x.InterpolationDurationByTCP).FirstOrDefaultAsync(x => x.Cipher == objectCipher);

            if (constructionObjectEntity == null)
            {
                constructionObjectEntity = new ConstructionObjectEntity()
                {
                    CreatedBy = interpolationDurationByTCP.CreatedBy,
                    CreatedAt = interpolationDurationByTCP.CreatedAt,
                    Cipher = objectCipher,
                    POS = new POSEntity()
                    {
                        InterpolationDurationByTCP = interpolationDurationByTCP
                    }
                };
                await DbSet.AddAsync(constructionObjectEntity);
            }
            else
            {
                constructionObjectEntity.POS.InterpolationDurationByTCP = interpolationDurationByTCP;
                constructionObjectEntity.UpdatedAt = interpolationDurationByTCP.CreatedAt;
                constructionObjectEntity.UpdatedBy = interpolationDurationByTCP.CreatedBy;
                DbSet.Update(constructionObjectEntity);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task Update(string objectCipher, ExtrapolationDurationByTCPEntity extrapolationDurationByTCP)
        {
            var constructionObjectEntity = await DbSet.Include(x => x.POS).ThenInclude(x => x.ExtrapolationDurationByTCP).FirstOrDefaultAsync(x => x.Cipher == objectCipher);

            if (constructionObjectEntity == null)
            {
                constructionObjectEntity = new ConstructionObjectEntity()
                {
                    CreatedBy = extrapolationDurationByTCP.CreatedBy,
                    CreatedAt = extrapolationDurationByTCP.CreatedAt,
                    Cipher = objectCipher,
                    POS = new POSEntity()
                    {
                        ExtrapolationDurationByTCP = extrapolationDurationByTCP
                    }
                };
                await DbSet.AddAsync(constructionObjectEntity);
            }
            else
            {
                constructionObjectEntity.POS.ExtrapolationDurationByTCP = extrapolationDurationByTCP;
                constructionObjectEntity.UpdatedAt = extrapolationDurationByTCP.CreatedAt;
                constructionObjectEntity.UpdatedBy = extrapolationDurationByTCP.CreatedBy;
                DbSet.Update(constructionObjectEntity);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task Update(string objectCipher, StepwiseExtrapolationDurationByTCPEntity stepwiseExtrapolationDurationByTCP)
        {
            var constructionObjectEntity = await DbSet.Include(x => x.POS).ThenInclude(x => x.StepwiseExtrapolationDurationByTCP).FirstOrDefaultAsync(x => x.Cipher == objectCipher);

            if (constructionObjectEntity == null)
            {
                constructionObjectEntity = new ConstructionObjectEntity()
                {
                    CreatedBy = stepwiseExtrapolationDurationByTCP.CreatedBy,
                    CreatedAt = stepwiseExtrapolationDurationByTCP.CreatedAt,
                    Cipher = objectCipher,
                    POS = new POSEntity()
                    {
                        StepwiseExtrapolationDurationByTCP = stepwiseExtrapolationDurationByTCP
                    }
                };
                await DbSet.AddAsync(constructionObjectEntity);
            }
            else
            {
                constructionObjectEntity.POS.StepwiseExtrapolationDurationByTCP = stepwiseExtrapolationDurationByTCP;
                constructionObjectEntity.UpdatedAt = stepwiseExtrapolationDurationByTCP.CreatedAt;
                constructionObjectEntity.UpdatedBy = stepwiseExtrapolationDurationByTCP.CreatedBy;
                DbSet.Update(constructionObjectEntity);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task Update(string objectCipher, EnergyAndWaterEntity energyAndWater)
        {
            var constructionObjectEntity = await DbSet.Include(x => x.POS).ThenInclude(x => x.EnergyAndWater).FirstOrDefaultAsync(x => x.Cipher == objectCipher);

            if (constructionObjectEntity == null)
            {
                constructionObjectEntity = new ConstructionObjectEntity()
                {
                    CreatedBy = energyAndWater.CreatedBy,
                    CreatedAt = energyAndWater.CreatedAt,
                    Cipher = objectCipher,
                    POS = new POSEntity()
                    {
                        EnergyAndWater = energyAndWater
                    }
                };
                await DbSet.AddAsync(constructionObjectEntity);
            }
            else
            {
                constructionObjectEntity.POS.EnergyAndWater = energyAndWater;
                constructionObjectEntity.UpdatedAt = energyAndWater.CreatedAt;
                constructionObjectEntity.UpdatedBy = energyAndWater.CreatedBy;
                DbSet.Update(constructionObjectEntity);
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
