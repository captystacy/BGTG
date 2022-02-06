using System;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.POSEntities.CalendarPlanToolEntities;
using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Factories;

namespace BGTG.Web.Infrastructure.Engine.ViewModelFactories.POSViewModelFactories
{
    public class CalendarPlanViewModelFactory : ViewModelFactory<CalendarPlanCreateViewModel, CalendarPlanUpdateViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<CalendarPlanEntity> _repository;

        public CalendarPlanViewModelFactory(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _repository = unitOfWork.GetRepository<CalendarPlanEntity>();
        }

        public override Task<OperationResult<CalendarPlanCreateViewModel>> GenerateForCreateAsync()
        {
            var operation = OperationResult.CreateResult<CalendarPlanCreateViewModel>();
            operation.Result = new CalendarPlanCreateViewModel();
            return Task.FromResult(operation);
        }

        public override async Task<OperationResult<CalendarPlanUpdateViewModel>> GenerateForUpdateAsync(Guid id)
        {
            var operation = OperationResult.CreateResult<CalendarPlanUpdateViewModel>();
            var entity = await _repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            var mapped = _mapper.Map<CalendarPlanUpdateViewModel>(entity);
            operation.Result = mapped;
            operation.AddSuccess("ViewModel generated for CalendarPlan entity. Please see Additional information in DataObject").AddData(new { Identifier = id });
            return operation;
        }
    }
}