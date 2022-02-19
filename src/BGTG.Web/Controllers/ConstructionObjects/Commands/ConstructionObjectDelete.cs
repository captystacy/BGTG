using System;
using System.Threading;
using System.Threading.Tasks;
using BGTG.Entities;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;

namespace BGTG.Web.Controllers.ConstructionObjects.Commands;

public record ConstructionObjectDeleteRequest(Guid Id) : OperationResultRequestBase<Guid>;

public class ConstructionObjectDeleteRequestHandler : OperationResultRequestHandlerBase<ConstructionObjectDeleteRequest, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public ConstructionObjectDeleteRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<OperationResult<Guid>> Handle(ConstructionObjectDeleteRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<Guid>();
        operation.Result = request.Id;

        var repository = _unitOfWork.GetRepository<ConstructionObjectEntity>();
        var constructionObjectEntity = await repository.FindAsync(request.Id);

        if (constructionObjectEntity == null)
        {
            operation.AddError(new MicroserviceNotFoundException());
            return operation;
        }

        repository.Delete(constructionObjectEntity);

        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangesResult.IsOk)
        {
            operation.AddError(new MicroserviceSaveChangesException());
            return operation;
        }

        return operation;
    }
}
