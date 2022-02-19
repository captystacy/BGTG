using System;
using System.Threading.Tasks;
using BGTG.Entities.Core;
using BGTG.Web.Controllers.POSControllers.DurationByLCs.Commands;
using BGTG.Web.Infrastructure.Attributes;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.DurationByLCViewModels;
using Calabonga.OperationResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.POSControllers.DurationByLCs;

public class DurationByLCsController : Controller
{
    private readonly IDurationByLCService _durationByLCService;
    private readonly IMediator _mediator;

    public DurationByLCsController(IDurationByLCService durationByLCService, IMediator mediator)
    {
        _durationByLCService = durationByLCService;
        _mediator = mediator;
    }

    [ValidateModelState]
    public async Task<OperationResult<DurationByLCCreateViewModel>> Write(DurationByLCCreateViewModel viewModel) => 
        await _mediator.Send(new DurationByLCWriteRequest(viewModel), HttpContext.RequestAborted);

    public async Task<OperationResult<Guid>> WriteById(Guid id) => 
        await _mediator.Send(new DurationByLCWriteByIdRequest(id), HttpContext.RequestAborted);

    public async Task<OperationResult<Guid>> Delete(Guid id) => 
        await _mediator.Send(new DurationByLCDeleteRequest(id), HttpContext.RequestAborted);

    public IActionResult Download()
    {
        var path = _durationByLCService.GetSavePath();
        return PhysicalFile(path, AppData.DocxMimeType, AppData.DurationByLCDownloadFileName);
    }
}