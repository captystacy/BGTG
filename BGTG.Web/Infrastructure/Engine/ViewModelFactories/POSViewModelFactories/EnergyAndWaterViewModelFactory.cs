using System;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.POSEntities.EnergyAndWaterToolEntities;
using BGTG.Web.ViewModels.POSViewModels.EnergyAndWaterViewModels;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Factories;

namespace BGTG.Web.Infrastructure.Engine.ViewModelFactories.POSViewModelFactories
{
    public class EnergyAndWaterViewModelFactory : ViewModelFactory<EnergyAndWaterCreateViewModel, EnergyAndWaterUpdateViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<EnergyAndWaterEntity> _repository;

        public EnergyAndWaterViewModelFactory(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _repository = unitOfWork.GetRepository<EnergyAndWaterEntity>();
        }

        public override Task<OperationResult<EnergyAndWaterCreateViewModel>> GenerateForCreateAsync()
        {
            var operation = OperationResult.CreateResult<EnergyAndWaterCreateViewModel>();
            operation.Result = new EnergyAndWaterCreateViewModel();
            return Task.FromResult(operation);
        }

        public override async Task<OperationResult<EnergyAndWaterUpdateViewModel>> GenerateForUpdateAsync(Guid id)
        {
            var operation = OperationResult.CreateResult<EnergyAndWaterUpdateViewModel>();
            var entity = await _repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            var mapped = _mapper.Map<EnergyAndWaterUpdateViewModel>(entity);
            operation.Result = mapped;
            operation.AddSuccess("ViewModel generated for EnergyAndWater entity. Please see Additional information in DataObject")
                .AddData(new {Identifier = id});
            return operation;
        }
    }
}