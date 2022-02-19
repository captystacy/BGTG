using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.POSEntities.DurationByLCToolEntities;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.Base;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.DurationByLCViewModels;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;

namespace BGTG.Web.Controllers.POSControllers.DurationByLCs.Commands;

public record DurationByLCWriteRequest(DurationByLCCreateViewModel ViewModel) : OperationResultRequestBase<DurationByLCCreateViewModel>;

public class DurationByLCWriteRequestHandler : OperationResultRequestHandlerBase<DurationByLCWriteRequest, DurationByLCCreateViewModel>
{
    private readonly IDurationByLCService _durationByLCService;
    private readonly IConstructionObjectService _constructionObjectService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DurationByLCWriteRequestHandler(IDurationByLCService durationByLCService, IConstructionObjectService constructionObjectService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _durationByLCService = durationByLCService;
        _constructionObjectService = constructionObjectService;
        _durationByLCService = durationByLCService;
        _constructionObjectService = constructionObjectService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public override async Task<OperationResult<DurationByLCCreateViewModel>> Handle(DurationByLCWriteRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<DurationByLCCreateViewModel>();
        operation.Result = request.ViewModel;

        var durationByLC = _durationByLCService.Write(request.ViewModel);
        var durationByLCEntity = _mapper.Map<DurationByLCEntity>(durationByLC);
        durationByLCEntity.CreatedBy = IdentityHelper.Instance.User!.Name;
        durationByLCEntity.CreatedAt = DateTime.UtcNow;

        await _constructionObjectService.Update(request.ViewModel.ObjectCipher, durationByLCEntity);

        if (!_unitOfWork.LastSaveChangesResult.IsOk)
        {
            operation.AddError(new MicroserviceSaveChangesException());
            return operation;
        }

        return operation;
    }
}
