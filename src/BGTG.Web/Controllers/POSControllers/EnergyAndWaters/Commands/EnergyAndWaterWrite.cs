using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.POSEntities.EnergyAndWaterToolEntities;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.Base;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.EnergyAndWaterViewModels;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;

namespace BGTG.Web.Controllers.POSControllers.EnergyAndWaters.Commands;

public record EnergyAndWaterWriteRequest(EnergyAndWaterCreateViewModel ViewModel) : OperationResultRequestBase<EnergyAndWaterCreateViewModel>;

public class EnergyAndWaterWriteRequestHandler : OperationResultRequestHandlerBase<EnergyAndWaterWriteRequest, EnergyAndWaterCreateViewModel>
{
    private readonly IEnergyAndWaterService _energyAndWaterService;
    private readonly IConstructionObjectService _constructionObjectService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EnergyAndWaterWriteRequestHandler(IEnergyAndWaterService energyAndWaterService, IConstructionObjectService constructionObjectService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _energyAndWaterService = energyAndWaterService;
        _constructionObjectService = constructionObjectService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public override async Task<OperationResult<EnergyAndWaterCreateViewModel>> Handle(EnergyAndWaterWriteRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<EnergyAndWaterCreateViewModel>();
        operation.Result = request.ViewModel;

        var energyAndWater = _energyAndWaterService.Write(request.ViewModel);
        var energyAndWaterEntity = _mapper.Map<EnergyAndWaterEntity>(energyAndWater);
        energyAndWaterEntity.CreatedBy = IdentityHelper.Instance.User!.Name;
        energyAndWaterEntity.CreatedAt = DateTime.UtcNow;

        await _constructionObjectService.Update(request.ViewModel.ObjectCipher, energyAndWaterEntity);

        if (!_unitOfWork.LastSaveChangesResult.IsOk)
        {
            operation.AddError(new MicroserviceSaveChangesException());
            return operation;
        }

        return operation;
    }
}