using AutoMapper;
using BGTG.Entities.POSEntities.EnergyAndWaterToolEntities;
using BGTG.Web.ViewModels.POSViewModels.EnergyAndWaterViewModels;
using Calabonga.Microservices.Core.Validators;
using Calabonga.UnitOfWork.Controllers.Factories;
using Calabonga.UnitOfWork.Controllers.Managers;

namespace BGTG.Web.Infrastructure.Engine.EntityManagers.POSEntityManagers
{
    public class EnergyAndWaterManager : EntityManager<EnergyAndWaterViewModel, EnergyAndWaterEntity, EnergyAndWaterCreateViewModel, EnergyAndWaterUpdateViewModel>
    {
        public EnergyAndWaterManager(IMapper mapper,
            IViewModelFactory<EnergyAndWaterCreateViewModel, EnergyAndWaterUpdateViewModel> viewModelFactory,
            IEntityValidator<EnergyAndWaterEntity> validator)
            : base(mapper, viewModelFactory, validator)
        {
        }
    }
}