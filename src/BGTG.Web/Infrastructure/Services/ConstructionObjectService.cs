using System.Threading.Tasks;
using BGTG.Entities;
using BGTG.Entities.POSEntities;
using BGTG.Entities.POSEntities.CalendarPlanToolEntities;
using BGTG.Entities.POSEntities.DurationByLCToolEntities;
using BGTG.Entities.POSEntities.DurationByTCPToolEntities;
using BGTG.Entities.POSEntities.EnergyAndWaterToolEntities;
using BGTG.Web.Infrastructure.Services.Base;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Infrastructure.Services;

public class ConstructionObjectService : IConstructionObjectService
{
    private readonly IUnitOfWork _unitOfWork;

    public ConstructionObjectService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Update(string objectCipher, CalendarPlanEntity calendarPlan)
    {
        var repository = _unitOfWork.GetRepository<ConstructionObjectEntity>();
        var constructionObject = await repository
            .GetFirstOrDefaultAsync(
                predicate: x => x.Cipher == objectCipher,
                include: x => x.Include(x => x.POS).ThenInclude(x => x!.CalendarPlan));

        if (constructionObject == null)
        {
            constructionObject = new ConstructionObjectEntity
            {
                CreatedBy = calendarPlan.CreatedBy,
                CreatedAt = calendarPlan.CreatedAt,
                Cipher = objectCipher,
                POS = new POSEntity
                {
                    CalendarPlan = calendarPlan
                }
            };

            await repository.InsertAsync(constructionObject);
        }
        else if (constructionObject.POS == null)
        {
            constructionObject.UpdatedAt = calendarPlan.CreatedAt;
            constructionObject.UpdatedBy = calendarPlan.CreatedBy;
            constructionObject.POS = new POSEntity
            {
                CalendarPlan = calendarPlan
            };

            repository.Update(constructionObject);
        }
        else
        {
            if (constructionObject.POS.CalendarPlan != null)
            {
                _unitOfWork.GetRepository<CalendarPlanEntity>().Delete(constructionObject.POS.CalendarPlan);
            }

            constructionObject.UpdatedAt = calendarPlan.CreatedAt;
            constructionObject.UpdatedBy = calendarPlan.CreatedBy;
            constructionObject.POS.CalendarPlan = calendarPlan;

            repository.Update(constructionObject);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task Update(string objectCipher, DurationByLCEntity durationByLC)
    {
        var repository = _unitOfWork.GetRepository<ConstructionObjectEntity>();
        var constructionObject = await repository
            .GetFirstOrDefaultAsync(
                predicate: x => x.Cipher == objectCipher,
                include: x => x.Include(x => x.POS).ThenInclude(x => x!.DurationByLC));

        if (constructionObject == null)
        {
            constructionObject = new ConstructionObjectEntity
            {
                CreatedBy = durationByLC.CreatedBy,
                CreatedAt = durationByLC.CreatedAt,
                Cipher = objectCipher,
                POS = new POSEntity
                {
                    DurationByLC = durationByLC
                }
            };
            await repository.InsertAsync(constructionObject);
        }
        else if (constructionObject.POS == null)
        {
            constructionObject.UpdatedAt = durationByLC.CreatedAt;
            constructionObject.UpdatedBy = durationByLC.CreatedBy;
            constructionObject.POS = new POSEntity
            {
                DurationByLC = durationByLC
            };

            repository.Update(constructionObject);
        }
        else
        {
            if (constructionObject.POS.DurationByLC != null)
            {
                _unitOfWork.GetRepository<DurationByLCEntity>().Delete(constructionObject.POS.DurationByLC);
            }

            constructionObject.UpdatedAt = durationByLC.CreatedAt;
            constructionObject.UpdatedBy = durationByLC.CreatedBy;
            constructionObject.POS.DurationByLC = durationByLC;

            repository.Update(constructionObject);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task Update(string objectCipher, InterpolationDurationByTCPEntity interpolationDurationByTCP)
    {
        var repository = _unitOfWork.GetRepository<ConstructionObjectEntity>();
        var constructionObject = await repository
            .GetFirstOrDefaultAsync(
                predicate: x => x.Cipher == objectCipher,
                include: x => x
                    .Include(x => x.POS).ThenInclude(x => x!.InterpolationDurationByTCP)
                    .Include(x => x.POS).ThenInclude(x => x!.ExtrapolationDurationByTCP)
                    .Include(x => x.POS).ThenInclude(x => x!.StepwiseExtrapolationDurationByTCP));

        if (constructionObject == null)
        {
            constructionObject = new ConstructionObjectEntity
            {
                CreatedBy = interpolationDurationByTCP.CreatedBy,
                CreatedAt = interpolationDurationByTCP.CreatedAt,
                Cipher = objectCipher,
                POS = new POSEntity
                {
                    InterpolationDurationByTCP = interpolationDurationByTCP
                }
            };
            await repository.InsertAsync(constructionObject);
        }
        else if (constructionObject.POS == null)
        {
            constructionObject.UpdatedAt = interpolationDurationByTCP.CreatedAt;
            constructionObject.UpdatedBy = interpolationDurationByTCP.CreatedBy;
            constructionObject.POS = new POSEntity
            {
                InterpolationDurationByTCP = interpolationDurationByTCP
            };

            repository.Update(constructionObject);
        }
        else
        {
            DeleteDurationByTCP(constructionObject);

            constructionObject.UpdatedAt = interpolationDurationByTCP.CreatedAt;
            constructionObject.UpdatedBy = interpolationDurationByTCP.CreatedBy;
            constructionObject.POS.InterpolationDurationByTCP = interpolationDurationByTCP;

            repository.Update(constructionObject);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task Update(string objectCipher, ExtrapolationDurationByTCPEntity extrapolationDurationByTCP)
    {
        var repository = _unitOfWork.GetRepository<ConstructionObjectEntity>();
        var constructionObject = await repository
            .GetFirstOrDefaultAsync(
                predicate: x => x.Cipher == objectCipher,
                include: x => x
                    .Include(x => x.POS).ThenInclude(x => x!.InterpolationDurationByTCP)
                    .Include(x => x.POS).ThenInclude(x => x!.ExtrapolationDurationByTCP)
                    .Include(x => x.POS).ThenInclude(x => x!.StepwiseExtrapolationDurationByTCP));

        if (constructionObject == null)
        {
            constructionObject = new ConstructionObjectEntity
            {
                CreatedBy = extrapolationDurationByTCP.CreatedBy,
                CreatedAt = extrapolationDurationByTCP.CreatedAt,
                Cipher = objectCipher,
                POS = new POSEntity
                {
                    ExtrapolationDurationByTCP = extrapolationDurationByTCP
                }
            };
            await repository.InsertAsync(constructionObject);
        }
        else if (constructionObject.POS == null)
        {
            constructionObject.UpdatedAt = extrapolationDurationByTCP.CreatedAt;
            constructionObject.UpdatedBy = extrapolationDurationByTCP.CreatedBy;
            constructionObject.POS = new POSEntity
            {
                ExtrapolationDurationByTCP = extrapolationDurationByTCP
            };

            repository.Update(constructionObject);
        }
        else
        {
            DeleteDurationByTCP(constructionObject);

            constructionObject.UpdatedAt = extrapolationDurationByTCP.CreatedAt;
            constructionObject.UpdatedBy = extrapolationDurationByTCP.CreatedBy;
            constructionObject.POS.ExtrapolationDurationByTCP = extrapolationDurationByTCP;

            repository.Update(constructionObject);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task Update(string objectCipher, StepwiseExtrapolationDurationByTCPEntity stepwiseExtrapolationDurationByTCP)
    {
        var repository = _unitOfWork.GetRepository<ConstructionObjectEntity>();
        var constructionObject = await repository
            .GetFirstOrDefaultAsync(
                predicate: x => x.Cipher == objectCipher,
                include: x => x
                    .Include(x => x.POS).ThenInclude(x => x!.InterpolationDurationByTCP)
                    .Include(x => x.POS).ThenInclude(x => x!.ExtrapolationDurationByTCP)
                    .Include(x => x.POS).ThenInclude(x => x!.StepwiseExtrapolationDurationByTCP));

        if (constructionObject == null)
        {
            constructionObject = new ConstructionObjectEntity
            {
                CreatedBy = stepwiseExtrapolationDurationByTCP.CreatedBy,
                CreatedAt = stepwiseExtrapolationDurationByTCP.CreatedAt,
                Cipher = objectCipher,
                POS = new POSEntity
                {
                    StepwiseExtrapolationDurationByTCP = stepwiseExtrapolationDurationByTCP
                }
            };
            await repository.InsertAsync(constructionObject);
        }
        else if (constructionObject.POS == null)
        {
            constructionObject.UpdatedAt = stepwiseExtrapolationDurationByTCP.CreatedAt;
            constructionObject.UpdatedBy = stepwiseExtrapolationDurationByTCP.CreatedBy;
            constructionObject.POS = new POSEntity
            {
                StepwiseExtrapolationDurationByTCP = stepwiseExtrapolationDurationByTCP
            };

            repository.Update(constructionObject);
        }
        else
        {
            DeleteDurationByTCP(constructionObject);

            constructionObject.UpdatedAt = stepwiseExtrapolationDurationByTCP.CreatedAt;
            constructionObject.UpdatedBy = stepwiseExtrapolationDurationByTCP.CreatedBy;
            constructionObject.POS.StepwiseExtrapolationDurationByTCP = stepwiseExtrapolationDurationByTCP;

            repository.Update(constructionObject);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    private void DeleteDurationByTCP(ConstructionObjectEntity constructionObject)
    {
        if (constructionObject.POS!.InterpolationDurationByTCP != null)
        {
            _unitOfWork.GetRepository<InterpolationDurationByTCPEntity>().Delete(constructionObject.POS.InterpolationDurationByTCP);
        }

        if (constructionObject.POS.ExtrapolationDurationByTCP != null)
        {
            _unitOfWork.GetRepository<ExtrapolationDurationByTCPEntity>().Delete(constructionObject.POS.ExtrapolationDurationByTCP);
        }

        if (constructionObject.POS.StepwiseExtrapolationDurationByTCP != null)
        {
            _unitOfWork.GetRepository<StepwiseExtrapolationDurationByTCPEntity>().Delete(constructionObject.POS.StepwiseExtrapolationDurationByTCP);
        }
    }

    public async Task Update(string objectCipher, EnergyAndWaterEntity energyAndWater)
    {
        var repository = _unitOfWork.GetRepository<ConstructionObjectEntity>();
        var constructionObject = await repository
            .GetFirstOrDefaultAsync(
                predicate: x => x.Cipher == objectCipher,
                include: x => x.Include(x => x.POS).ThenInclude(x => x!.EnergyAndWater));

        if (constructionObject == null)
        {
            constructionObject = new ConstructionObjectEntity()
            {
                CreatedBy = energyAndWater.CreatedBy,
                CreatedAt = energyAndWater.CreatedAt,
                Cipher = objectCipher,
                POS = new POSEntity
                {
                    EnergyAndWater = energyAndWater
                }
            };
            await repository.InsertAsync(constructionObject);
        }
        else if (constructionObject.POS == null)
        {
            constructionObject.UpdatedAt = energyAndWater.CreatedAt;
            constructionObject.UpdatedBy = energyAndWater.CreatedBy;
            constructionObject.POS = new POSEntity
            {
                EnergyAndWater = energyAndWater
            };

            repository.Update(constructionObject);
        }
        else
        {
            if (constructionObject.POS.EnergyAndWater != null)
            {
                _unitOfWork.GetRepository<EnergyAndWaterEntity>().Delete(constructionObject.POS.EnergyAndWater);
            }

            constructionObject.UpdatedAt = energyAndWater.CreatedAt;
            constructionObject.UpdatedBy = energyAndWater.CreatedBy;
            constructionObject.POS.EnergyAndWater = energyAndWater;

            repository.Update(constructionObject);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}