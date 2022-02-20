using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities;
using BGTG.Entities.POSEntities;
using BGTG.Entities.POSEntities.DurationByTCPToolEntities;
using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.DurationByTCPViewModels;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.POSControllers.DurationByTCPs.Commands;

public record DurationByTCPWriteRequest(DurationByTCPCreateViewModel ViewModel) : OperationResultRequestBase<DurationByTCPCreateViewModel>;

public class DurationByTCPWriteRequestHandler : OperationResultRequestHandlerBase<DurationByTCPWriteRequest, DurationByTCPCreateViewModel>
{
    private readonly IDurationByTCPService _durationByTCPService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DurationByTCPWriteRequestHandler(IDurationByTCPService durationByTCPService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _durationByTCPService = durationByTCPService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public override async Task<OperationResult<DurationByTCPCreateViewModel>> Handle(DurationByTCPWriteRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<DurationByTCPCreateViewModel>();
        operation.Result = request.ViewModel;

        var durationByTCP = _durationByTCPService.Write(request.ViewModel);

        if (durationByTCP is null)
        {
            operation.AddError(new MicroserviceNotFoundException());
            return operation;
        }

        var now = DateTime.UtcNow;
        switch (durationByTCP)
        {
            case InterpolationDurationByTCP interpolationDuration:
                var interpolationDurationByTCPEntity = _mapper.Map<InterpolationDurationByTCPEntity>(interpolationDuration);
                interpolationDurationByTCPEntity.CreatedBy = IdentityHelper.Instance.User!.Name;
                interpolationDurationByTCPEntity.CreatedAt = now;

                await UpdateConstructionObjectAsync(request.ViewModel.ObjectCipher, interpolationDurationByTCPEntity);
                break;
            case ExtrapolationDurationByTCP extrapolationDurationByTCP:
                if (extrapolationDurationByTCP is StepwiseExtrapolationDurationByTCP stepwiseExtrapolationDuration)
                {
                    var stepwiseExtrapolationDurationByTCPEntity = _mapper.Map<StepwiseExtrapolationDurationByTCPEntity>(stepwiseExtrapolationDuration);
                    stepwiseExtrapolationDurationByTCPEntity.CreatedBy = IdentityHelper.Instance.User!.Name;
                    stepwiseExtrapolationDurationByTCPEntity.CreatedAt = now;

                    await UpdateConstructionObjectAsync(request.ViewModel.ObjectCipher, stepwiseExtrapolationDurationByTCPEntity);
                    break;
                }
                else
                {
                    var extrapolationDurationByTCPEntity = _mapper.Map<ExtrapolationDurationByTCPEntity>(extrapolationDurationByTCP);
                    extrapolationDurationByTCPEntity.CreatedBy = IdentityHelper.Instance.User!.Name;
                    extrapolationDurationByTCPEntity.CreatedAt = now;

                    await UpdateConstructionObjectAsync(request.ViewModel.ObjectCipher, extrapolationDurationByTCPEntity);
                    break;
                }
            default:
                operation.AddError(new MicroserviceNotFoundException());
                return operation;
        }

        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangesResult.IsOk)
        {
            operation.AddError(new MicroserviceSaveChangesException());
            return operation;
        }

        return operation;
    }

    private async Task UpdateConstructionObjectAsync(string objectCipher, InterpolationDurationByTCPEntity interpolationDurationByTCPEntity)
    {
        var repository = _unitOfWork.GetRepository<ConstructionObjectEntity>();
        var constructionObject = await repository
            .GetFirstOrDefaultAsync(
                predicate: x => x.Cipher == objectCipher,
                include: x => x
                    .Include(x => x.POS).ThenInclude(x => x!.InterpolationDurationByTCP)
                    .Include(x => x.POS).ThenInclude(x => x!.ExtrapolationDurationByTCP)
                    .Include(x => x.POS).ThenInclude(x => x!.StepwiseExtrapolationDurationByTCP));

        if (constructionObject == null)
        {
            constructionObject = new ConstructionObjectEntity
            {
                CreatedBy = interpolationDurationByTCPEntity.CreatedBy,
                CreatedAt = interpolationDurationByTCPEntity.CreatedAt,
                Cipher = objectCipher,
                POS = new POSEntity
                {
                    InterpolationDurationByTCP = interpolationDurationByTCPEntity
                }
            };
            await repository.InsertAsync(constructionObject);
        }
        else if (constructionObject.POS == null)
        {
            constructionObject.UpdatedAt = interpolationDurationByTCPEntity.CreatedAt;
            constructionObject.UpdatedBy = interpolationDurationByTCPEntity.CreatedBy;
            constructionObject.POS = new POSEntity
            {
                InterpolationDurationByTCP = interpolationDurationByTCPEntity
            };

            repository.Update(constructionObject);
        }
        else
        {
            DeleteDurationByTCP(constructionObject);

            constructionObject.UpdatedAt = interpolationDurationByTCPEntity.CreatedAt;
            constructionObject.UpdatedBy = interpolationDurationByTCPEntity.CreatedBy;
            constructionObject.POS.InterpolationDurationByTCP = interpolationDurationByTCPEntity;

            repository.Update(constructionObject);
        }
    }

    private async Task UpdateConstructionObjectAsync(string objectCipher, ExtrapolationDurationByTCPEntity extrapolationDurationByTCPEntity)
    {
        var repository = _unitOfWork.GetRepository<ConstructionObjectEntity>();
        var constructionObject = await repository
            .GetFirstOrDefaultAsync(
                predicate: x => x.Cipher == objectCipher,
                include: x => x
                    .Include(x => x.POS).ThenInclude(x => x!.InterpolationDurationByTCP)
                    .Include(x => x.POS).ThenInclude(x => x!.ExtrapolationDurationByTCP)
                    .Include(x => x.POS).ThenInclude(x => x!.StepwiseExtrapolationDurationByTCP));

        if (constructionObject == null)
        {
            constructionObject = new ConstructionObjectEntity
            {
                CreatedBy = extrapolationDurationByTCPEntity.CreatedBy,
                CreatedAt = extrapolationDurationByTCPEntity.CreatedAt,
                Cipher = objectCipher,
                POS = new POSEntity
                {
                    ExtrapolationDurationByTCP = extrapolationDurationByTCPEntity
                }
            };
            await repository.InsertAsync(constructionObject);
        }
        else if (constructionObject.POS == null)
        {
            constructionObject.UpdatedAt = extrapolationDurationByTCPEntity.CreatedAt;
            constructionObject.UpdatedBy = extrapolationDurationByTCPEntity.CreatedBy;
            constructionObject.POS = new POSEntity
            {
                ExtrapolationDurationByTCP = extrapolationDurationByTCPEntity
            };

            repository.Update(constructionObject);
        }
        else
        {
            DeleteDurationByTCP(constructionObject);

            constructionObject.UpdatedAt = extrapolationDurationByTCPEntity.CreatedAt;
            constructionObject.UpdatedBy = extrapolationDurationByTCPEntity.CreatedBy;
            constructionObject.POS.ExtrapolationDurationByTCP = extrapolationDurationByTCPEntity;

            repository.Update(constructionObject);
        }
    }

    private async Task UpdateConstructionObjectAsync(string objectCipher, StepwiseExtrapolationDurationByTCPEntity stepwiseExtrapolationDurationByTCPEntity)
    {
        var repository = _unitOfWork.GetRepository<ConstructionObjectEntity>();
        var constructionObject = await repository
            .GetFirstOrDefaultAsync(
                predicate: x => x.Cipher == objectCipher,
                include: x => x
                    .Include(x => x.POS).ThenInclude(x => x!.InterpolationDurationByTCP)
                    .Include(x => x.POS).ThenInclude(x => x!.ExtrapolationDurationByTCP)
                    .Include(x => x.POS).ThenInclude(x => x!.StepwiseExtrapolationDurationByTCP));

        if (constructionObject == null)
        {
            constructionObject = new ConstructionObjectEntity
            {
                CreatedBy = stepwiseExtrapolationDurationByTCPEntity.CreatedBy,
                CreatedAt = stepwiseExtrapolationDurationByTCPEntity.CreatedAt,
                Cipher = objectCipher,
                POS = new POSEntity
                {
                    StepwiseExtrapolationDurationByTCP = stepwiseExtrapolationDurationByTCPEntity
                }
            };
            await repository.InsertAsync(constructionObject);
        }
        else if (constructionObject.POS == null)
        {
            constructionObject.UpdatedAt = stepwiseExtrapolationDurationByTCPEntity.CreatedAt;
            constructionObject.UpdatedBy = stepwiseExtrapolationDurationByTCPEntity.CreatedBy;
            constructionObject.POS = new POSEntity
            {
                StepwiseExtrapolationDurationByTCP = stepwiseExtrapolationDurationByTCPEntity
            };

            repository.Update(constructionObject);
        }
        else
        {
            DeleteDurationByTCP(constructionObject);

            constructionObject.UpdatedAt = stepwiseExtrapolationDurationByTCPEntity.CreatedAt;
            constructionObject.UpdatedBy = stepwiseExtrapolationDurationByTCPEntity.CreatedBy;
            constructionObject.POS.StepwiseExtrapolationDurationByTCP = stepwiseExtrapolationDurationByTCPEntity;

            repository.Update(constructionObject);
        }
    }

    private void DeleteDurationByTCP(ConstructionObjectEntity constructionObject)
    {
        if (constructionObject.POS!.InterpolationDurationByTCP != null)
        {
            _unitOfWork.GetRepository<InterpolationDurationByTCPEntity>().Delete(constructionObject.POS.InterpolationDurationByTCP);
        }

        if (constructionObject.POS.ExtrapolationDurationByTCP != null)
        {
            _unitOfWork.GetRepository<ExtrapolationDurationByTCPEntity>().Delete(constructionObject.POS.ExtrapolationDurationByTCP);
        }

        if (constructionObject.POS.StepwiseExtrapolationDurationByTCP != null)
        {
            _unitOfWork.GetRepository<StepwiseExtrapolationDurationByTCPEntity>().Delete(constructionObject.POS.StepwiseExtrapolationDurationByTCP);
        }
    }
}