using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.POS.DurationByTCPToolEntities;
using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.Base;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.DurationByTCPViewModels;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;

namespace BGTG.Web.Controllers.POSControllers.DurationByTCPs.Commands;

public record DurationByTCPWriteRequest(DurationByTCPCreateViewModel ViewModel) : OperationResultRequestBase<DurationByTCPCreateViewModel>;

public class DurationByTCPWriteRequestHandler : OperationResultRequestHandlerBase<DurationByTCPWriteRequest, DurationByTCPCreateViewModel>
{
    private readonly IDurationByTCPService _durationByTCPService;
    private readonly IConstructionObjectService _constructionObjectService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DurationByTCPWriteRequestHandler(IDurationByTCPService durationByTCPService, IConstructionObjectService constructionObjectService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _durationByTCPService = durationByTCPService;
        _constructionObjectService = constructionObjectService;
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

                await _constructionObjectService.Update(request.ViewModel.ObjectCipher, interpolationDurationByTCPEntity);
                break;
            case ExtrapolationDurationByTCP extrapolationDurationByTCP:
                if (extrapolationDurationByTCP is StepwiseExtrapolationDurationByTCP stepwiseExtrapolationDuration)
                {
                    var stepwiseExtrapolationDurationByTCPEntity = _mapper.Map<StepwiseExtrapolationDurationByTCPEntity>(stepwiseExtrapolationDuration);
                    stepwiseExtrapolationDurationByTCPEntity.CreatedBy = IdentityHelper.Instance.User!.Name;
                    stepwiseExtrapolationDurationByTCPEntity.CreatedAt = now;

                    await _constructionObjectService.Update(request.ViewModel.ObjectCipher, stepwiseExtrapolationDurationByTCPEntity);
                    break;
                }
                else
                {
                    var extrapolationDurationByTCPEntity = _mapper.Map<ExtrapolationDurationByTCPEntity>(extrapolationDurationByTCP);
                    extrapolationDurationByTCPEntity.CreatedBy = IdentityHelper.Instance.User!.Name;
                    extrapolationDurationByTCPEntity.CreatedAt = now;

                    await _constructionObjectService.Update(request.ViewModel.ObjectCipher, extrapolationDurationByTCPEntity);
                    break;
                }
            default:
                operation.AddError(new MicroserviceNotFoundException());
                return operation;
        }

        if (!_unitOfWork.LastSaveChangesResult.IsOk)
        {
            operation.AddError(new MicroserviceSaveChangesException());
            return operation;
        }

        return operation;
    }
}