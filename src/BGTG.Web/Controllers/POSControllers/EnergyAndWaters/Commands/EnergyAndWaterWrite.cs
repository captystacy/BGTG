using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities;
using BGTG.Entities.POSEntities;
using BGTG.Entities.POSEntities.EnergyAndWaterToolEntities;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.EnergyAndWaterViewModels;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.POSControllers.EnergyAndWaters.Commands;

public record EnergyAndWaterWriteRequest(EnergyAndWaterCreateViewModel ViewModel) : OperationResultRequestBase<EnergyAndWaterCreateViewModel>;

public class EnergyAndWaterWriteRequestHandler : OperationResultRequestHandlerBase<EnergyAndWaterWriteRequest, EnergyAndWaterCreateViewModel>
{
    private readonly IEnergyAndWaterService _energyAndWaterService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EnergyAndWaterWriteRequestHandler(IEnergyAndWaterService energyAndWaterService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _energyAndWaterService = energyAndWaterService;
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

        await UpdateConstructionObject(request.ViewModel.ObjectCipher, energyAndWaterEntity);

        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangesResult.IsOk)
        {
            operation.AddError(new MicroserviceSaveChangesException());
            return operation;
        }

        return operation;
    }

    private async Task UpdateConstructionObject(string objectCipher, EnergyAndWaterEntity energyAndWaterEntity)
    {
        var repository = _unitOfWork.GetRepository<ConstructionObjectEntity>();
        var constructionObject = await repository
            .GetFirstOrDefaultAsync(
                predicate: x => x.Cipher == objectCipher,
                include: x => x.Include(x => x.POS).ThenInclude(x => x!.EnergyAndWater));

        if (constructionObject == null)
        {
            constructionObject = new ConstructionObjectEntity()
            {
                CreatedBy = energyAndWaterEntity.CreatedBy,
                CreatedAt = energyAndWaterEntity.CreatedAt,
                Cipher = objectCipher,
                POS = new POSEntity
                {
                    EnergyAndWater = energyAndWaterEntity
                }
            };
            await repository.InsertAsync(constructionObject);
        }
        else if (constructionObject.POS == null)
        {
            constructionObject.UpdatedAt = energyAndWaterEntity.CreatedAt;
            constructionObject.UpdatedBy = energyAndWaterEntity.CreatedBy;
            constructionObject.POS = new POSEntity
            {
                EnergyAndWater = energyAndWaterEntity
            };

            repository.Update(constructionObject);
        }
        else
        {
            if (constructionObject.POS.EnergyAndWater != null)
            {
                _unitOfWork.GetRepository<EnergyAndWaterEntity>().Delete(constructionObject.POS.EnergyAndWater);
            }

            constructionObject.UpdatedAt = energyAndWaterEntity.CreatedAt;
            constructionObject.UpdatedBy = energyAndWaterEntity.CreatedBy;
            constructionObject.POS.EnergyAndWater = energyAndWaterEntity;

            repository.Update(constructionObject);
        }
    }
}