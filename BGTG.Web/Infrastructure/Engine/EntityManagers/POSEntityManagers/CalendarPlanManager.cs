using AutoMapper;
using BGTG.Entities.POSEntities.CalendarPlanToolEntities;
using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;
using Calabonga.Microservices.Core.Validators;
using Calabonga.UnitOfWork.Controllers.Factories;
using Calabonga.UnitOfWork.Controllers.Managers;

namespace BGTG.Web.Infrastructure.Engine.EntityManagers.POSEntityManagers
{
    public class CalendarPlanManager : EntityManager<CalendarPlanViewModel, CalendarPlanEntity, CalendarPlanCreateViewModel, CalendarPlanUpdateViewModel>
    {
        public CalendarPlanManager(IMapper mapper, IViewModelFactory<CalendarPlanCreateViewModel, CalendarPlanUpdateViewModel> viewModelFactory,
            IEntityValidator<CalendarPlanEntity> validator)
            : base(mapper, viewModelFactory, validator)
        {
        }
    }
}