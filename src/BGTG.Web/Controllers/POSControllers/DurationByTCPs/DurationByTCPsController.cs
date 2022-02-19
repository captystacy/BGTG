using System;
using System.Threading.Tasks;
using BGTG.Entities.Core;
using BGTG.Web.Controllers.POSControllers.DurationByTCPs.Commands;
using BGTG.Web.Infrastructure.Attributes;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.DurationByTCPViewModels;
using Calabonga.OperationResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.POSControllers.DurationByTCPs;

public class DurationByTCPsController : Controller
{
    private readonly IDurationByTCPService _durationByTCPService;
    private readonly IMediator _mediator;

    public DurationByTCPsController(IDurationByTCPService durationByTCPService, IMediator mediator)
    {
        _durationByTCPService = durationByTCPService;
        _mediator = mediator;
    }

    [ValidateModelState]
    public async Task<OperationResult<DurationByTCPCreateViewModel>> Write(DurationByTCPCreateViewModel viewModel) => 
        await _mediator.Send(new DurationByTCPWriteRequest(viewModel), HttpContext.RequestAborted);

    public async Task<OperationResult<Guid>> WriteById(Guid id) => 
        await _mediator.Send(new DurationByTCPWriteByIdRequest(id), HttpContext.RequestAborted);

    public async Task<OperationResult<Guid>> DeleteInterpolation(Guid id) => 
        await _mediator.Send(new InterpolationDurationByTCPDeleteRequest(id), HttpContext.RequestAborted);

    public async Task<OperationResult<Guid>> DeleteExtrapolation(Guid id) => 
        await _mediator.Send(new ExtrapolationDurationByTCPDeleteRequest(id), HttpContext.RequestAborted);

    public async Task<OperationResult<Guid>> DeleteStepwiseExtrapolation(Guid id) => 
        await _mediator.Send(new StepwiseExtrapolationDurationByTCPDeleteRequest(id), HttpContext.RequestAborted);

    public IActionResult Download()
    {
        var path = _durationByTCPService.GetSavePath();
        return PhysicalFile(path, AppData.DocxMimeType, AppData.DurationByTCPDownloadFileName);
    }
}