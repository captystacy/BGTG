using System;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.POSEntities.DurationByLCToolEntities;
using BGTG.Web.ViewModels.POSViewModels.DurationByLCViewModels;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Factories;

namespace BGTG.Web.Infrastructure.Engine.ViewModelFactories.POSViewModelFactories
{
    public class DurationByLCViewModelFactory : ViewModelFactory<DurationByLCCreateViewModel, DurationByLCUpdateViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<DurationByLCEntity> _repository;

        public DurationByLCViewModelFactory(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _repository = unitOfWork.GetRepository<DurationByLCEntity>();
        }

        public override Task<OperationResult<DurationByLCCreateViewModel>> GenerateForCreateAsync()
        {
            var operation = OperationResult.CreateResult<DurationByLCCreateViewModel>();
            operation.Result = new DurationByLCCreateViewModel();
            return Task.FromResult(operation);
        }

        public override async Task<OperationResult<DurationByLCUpdateViewModel>> GenerateForUpdateAsync(Guid id)
        {
            var operation = OperationResult.CreateResult<DurationByLCUpdateViewModel>();
            var entity = await _repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            var mapped = _mapper.Map<DurationByLCUpdateViewModel>(entity);
            operation.Result = mapped;
            operation.AddSuccess("ViewModel generated for DurationByLC entity. Please see Additional information in DataObject").AddData(new { Identifier = id });
            return operation;
        }
    }
}