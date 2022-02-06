using AutoMapper;
using BGTG.Entities;
using BGTG.Web.ViewModels.ConstructionObjectViewModels;
using Calabonga.Microservices.Core.Validators;
using Calabonga.UnitOfWork.Controllers.Factories;
using Calabonga.UnitOfWork.Controllers.Managers;

namespace BGTG.Web.Infrastructure.Engine.EntityManagers
{
    public class ConstructionObjectManager : EntityManager<ConstructionObjectViewModel, ConstructionObjectEntity, ConstructionObjectCreateViewModel, ConstructionObjectUpdateViewModel>
    {
        public ConstructionObjectManager(IMapper mapper,
            IViewModelFactory<ConstructionObjectCreateViewModel, ConstructionObjectUpdateViewModel> viewModelFactory,
            IEntityValidator<ConstructionObjectEntity> validator)
            : base(mapper, viewModelFactory, validator)
        {
        }
    }
}