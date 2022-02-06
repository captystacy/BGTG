using AutoMapper;
using BGTG.Entities.POSEntities.DurationByLCToolEntities;
using BGTG.Web.ViewModels.POSViewModels.DurationByLCViewModels;
using Calabonga.Microservices.Core.Validators;
using Calabonga.UnitOfWork.Controllers.Factories;
using Calabonga.UnitOfWork.Controllers.Managers;

namespace BGTG.Web.Infrastructure.Engine.EntityManagers.POSEntityManagers
{
    public class DurationByLCManager : EntityManager<DurationByLCViewModel, DurationByLCEntity, DurationByLCCreateViewModel, DurationByLCUpdateViewModel>
    {
        public DurationByLCManager(IMapper mapper, 
            IViewModelFactory<DurationByLCCreateViewModel, DurationByLCUpdateViewModel> viewModelFactory,
            IEntityValidator<DurationByLCEntity> validator)
            : base(mapper, viewModelFactory, validator)
        {
        }
    }
}