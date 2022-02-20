using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities;
using BGTG.Entities.POSEntities;
using BGTG.Entities.POSEntities.DurationByLCToolEntities;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.DurationByLCViewModels;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.POSControllers.DurationByLCs.Commands;

public record DurationByLCWriteRequest(DurationByLCCreateViewModel ViewModel) : OperationResultRequestBase<DurationByLCCreateViewModel>;

public class DurationByLCWriteRequestHandler : OperationResultRequestHandlerBase<DurationByLCWriteRequest, DurationByLCCreateViewModel>
{
    private readonly IDurationByLCService _durationByLCService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DurationByLCWriteRequestHandler(IDurationByLCService durationByLCService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _durationByLCService = durationByLCService;
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

        await UpdateConstructionObjectAsync(request.ViewModel.ObjectCipher, durationByLCEntity);

        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangesResult.IsOk)
        {
            operation.AddError(new MicroserviceSaveChangesException());
            return operation;
        }

        return operation;
    }

    private async Task UpdateConstructionObjectAsync(string objectCipher, DurationByLCEntity durationByLCEntity)
    {
        var repository = _unitOfWork.GetRepository<ConstructionObjectEntity>();
        var constructionObject = await repository
            .GetFirstOrDefaultAsync(
                predicate: x => x.Cipher == objectCipher,
                include: x => x.Include(x => x.POS).ThenInclude(x => x!.DurationByLC));

        if (constructionObject == null)
        {
            constructionObject = new ConstructionObjectEntity
            {
                CreatedBy = durationByLCEntity.CreatedBy,
                CreatedAt = durationByLCEntity.CreatedAt,
                Cipher = objectCipher,
                POS = new POSEntity
                {
                    DurationByLC = durationByLCEntity
                }
            };
            await repository.InsertAsync(constructionObject);
        }
        else if (constructionObject.POS == null)
        {
            constructionObject.UpdatedAt = durationByLCEntity.CreatedAt;
            constructionObject.UpdatedBy = durationByLCEntity.CreatedBy;
            constructionObject.POS = new POSEntity
            {
                DurationByLC = durationByLCEntity
            };

            repository.Update(constructionObject);
        }
        else
        {
            if (constructionObject.POS.DurationByLC != null)
            {
                _unitOfWork.GetRepository<DurationByLCEntity>().Delete(constructionObject.POS.DurationByLC);
            }

            constructionObject.UpdatedAt = durationByLCEntity.CreatedAt;
            constructionObject.UpdatedBy = durationByLCEntity.CreatedBy;
            constructionObject.POS.DurationByLC = durationByLCEntity;

            repository.Update(constructionObject);
        }
    }
}
