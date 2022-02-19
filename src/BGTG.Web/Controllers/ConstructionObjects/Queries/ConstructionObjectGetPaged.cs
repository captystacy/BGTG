using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities;
using BGTG.Web.ViewModels;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Base;
using Calabonga.OperationResults;
using Calabonga.PredicatesBuilder;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.ConstructionObjects.Queries;

public class ConstructionObjectGetPagedRequest : OperationResultRequestBase<IPagedList<ConstructionObjectViewModel>>
{
    public ConstructionObjectGetPagedRequest(int pageIndex,  string? objectCipher)
    {
        PageIndex = pageIndex;
        ObjectCipher = objectCipher;
    }

    public int PageIndex { get; }

    public int PageSize => 20;

    public string? ObjectCipher { get; }
}

public class ConstructionObjectGetPagedRequestHandler : OperationResultRequestHandlerBase<ConstructionObjectGetPagedRequest, IPagedList<ConstructionObjectViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ConstructionObjectGetPagedRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public override async Task<OperationResult<IPagedList<ConstructionObjectViewModel>>> Handle(ConstructionObjectGetPagedRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<IPagedList<ConstructionObjectViewModel>>();

        var predicate = BuildPredicate(request);
        var items = await _unitOfWork.GetRepository<ConstructionObjectEntity>()
            .GetPagedListAsync(
                predicate,
                include: i => i
                    .Include(x => x.POS).ThenInclude(x => x!.CalendarPlan)
                    .Include(x => x.POS).ThenInclude(x => x!.DurationByLC)
                    .Include(x => x.POS).ThenInclude(x => x!.InterpolationDurationByTCP)
                    .Include(x => x.POS).ThenInclude(x => x!.ExtrapolationDurationByTCP)
                    .Include(x => x.POS).ThenInclude(x => x!.StepwiseExtrapolationDurationByTCP)
                    .Include(x => x.POS).ThenInclude(x => x!.EnergyAndWater),
                orderBy: o => o.OrderByDescending(x => x.UpdatedAt),
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                cancellationToken: cancellationToken);

        operation.Result = _mapper.Map<IPagedList<ConstructionObjectViewModel>>(items);
        return operation;
    }

    private Expression<Func<ConstructionObjectEntity, bool>> BuildPredicate(ConstructionObjectGetPagedRequest request)
    {
        var predicate = PredicateBuilder.True<ConstructionObjectEntity>();

        if (!string.IsNullOrWhiteSpace(request.ObjectCipher))
        {
            predicate = predicate.And(x => x.Cipher.Contains(request.ObjectCipher));
        }

        return predicate;
    }
}
