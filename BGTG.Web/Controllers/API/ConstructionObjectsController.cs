using System;
using System.Linq;
using AutoMapper;
using BGTG.Entities;
using BGTG.Web.Infrastructure.Settings;
using BGTG.Web.ViewModels.ConstructionObjectViewModels;
using Calabonga.Microservices.Core.QueryParams;
using Calabonga.Microservices.Core.Validators;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Calabonga.UnitOfWork.Controllers.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;

namespace BGTG.Web.Controllers.API
{
    public class ConstructionObjectsController : WritableController<ConstructionObjectViewModel, ConstructionObjectEntity, ConstructionObjectCreateViewModel, ConstructionObjectUpdateViewModel,
        PagedListQueryParams>
    {
        private readonly CurrentAppSettings _appSettings;

        public ConstructionObjectsController(
            IMapper mapper,
            IEntityManagerFactory entityManagerFactory,
            IUnitOfWork unitOfWork,
            IOptions<CurrentAppSettings> appSettings)
            : base(entityManagerFactory, unitOfWork, mapper)
        {
            _appSettings = appSettings.Value;
        }

        protected override Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>> GetIncludes()
        {
            return x => x
                .Include(x => x.POS).ThenInclude(x => x.CalendarPlan)
                .Include(x => x.POS).ThenInclude(x => x.DurationByLC)
                .Include(x => x.POS).ThenInclude(x => x.InterpolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.ExtrapolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.StepwiseExtrapolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.EnergyAndWater);
        }

        protected override PermissionValidationResult ValidateQueryParams(PagedListQueryParams queryParams)
        {
            if (queryParams.PageSize <= 0)
            {
                queryParams.PageSize = _appSettings.PageSize;
                queryParams.SortDirection = QueryParamsSortDirection.Descending;
            }

            return base.ValidateQueryParams(queryParams);
        }

        protected override string GetPropertyNameForOrderBy()
        {
            return "UpdatedAt";
        }
    }
}
