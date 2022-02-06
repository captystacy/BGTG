using System;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities;
using BGTG.Web.ViewModels.ConstructionObjectViewModels;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Factories;

namespace BGTG.Web.Infrastructure.Engine.ViewModelFactories
{
    public class ConstructionObjectViewModelFactory : ViewModelFactory<ConstructionObjectCreateViewModel, ConstructionObjectUpdateViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ConstructionObjectEntity> _repository;

        public ConstructionObjectViewModelFactory(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _repository = unitOfWork.GetRepository<ConstructionObjectEntity>();
        }

        public override Task<OperationResult<ConstructionObjectCreateViewModel>> GenerateForCreateAsync()
        {
            var operation = OperationResult.CreateResult<ConstructionObjectCreateViewModel>();
            operation.Result = new ConstructionObjectCreateViewModel();
            return Task.FromResult(operation);
        }

        public override async Task<OperationResult<ConstructionObjectUpdateViewModel>> GenerateForUpdateAsync(Guid id)
        {
            var operation = OperationResult.CreateResult<ConstructionObjectUpdateViewModel>();
            var entity = await _repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            var mapped = _mapper.Map<ConstructionObjectUpdateViewModel>(entity);
            operation.Result = mapped;
            operation.AddSuccess("ViewModel generated for ConstructionObject entity. Please see Additional information in DataObject")
                .AddData(new {Identifier = id});
            return operation;
        }
    }
}