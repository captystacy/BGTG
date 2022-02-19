using System;
using System.Globalization;
using System.Threading.Tasks;
using BGTG.Web.Controllers.ConstructionObjects.Commands;
using BGTG.Web.Controllers.ConstructionObjects.Queries;
using Calabonga.OperationResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.ConstructionObjects;

public class ConstructionObjectsController : Controller
{
    private readonly IMediator _mediator;

    public ConstructionObjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(int? pageIndex, string objectCipher) =>
        View(await _mediator.Send(new ConstructionObjectGetPagedRequest(pageIndex - 1 ?? 0, objectCipher),
            HttpContext.RequestAborted));

    public async Task<OperationResult<Guid>> Delete(Guid id) =>
        await _mediator.Send(new ConstructionObjectDeleteRequest(id), HttpContext.RequestAborted);
}