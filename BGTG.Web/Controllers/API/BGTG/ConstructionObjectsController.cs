using System;
using System.Linq;
using AutoMapper;
using BGTG.Entities.BGTG;
using BGTG.Web.Infrastructure.QueryParams;
using BGTG.Web.ViewModels.BGTG;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BGTG.Web.Controllers.API.BGTG;

public class ConstructionObjectsController : ReadOnlyController<ConstructionObjectEntity, ConstructionObjectViewModel, DefaultPagedListQueryParams>
{
    public ConstructionObjectsController(
        IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(unitOfWork, mapper)
    {
    }

    protected override Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>> GetIncludes()
    {
        return x => x
            .Include(x => x.POS).ThenInclude(x => x!.CalendarPlan)
            .Include(x => x.POS).ThenInclude(x => x!.DurationByLC)
            .Include(x => x.POS).ThenInclude(x => x!.InterpolationDurationByTCP)
            .Include(x => x.POS).ThenInclude(x => x!.ExtrapolationDurationByTCP)
            .Include(x => x.POS).ThenInclude(x => x!.StepwiseExtrapolationDurationByTCP)
            .Include(x => x.POS).ThenInclude(x => x!.EnergyAndWater)!;
    }

    protected override string GetPropertyNameForOrderBy()
    {
        return "UpdatedAt";
    }
}