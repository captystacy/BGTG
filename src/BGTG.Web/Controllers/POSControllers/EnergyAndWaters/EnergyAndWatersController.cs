using System;
using System.Threading.Tasks;
using BGTG.Entities.Core;
using BGTG.Web.Controllers.POSControllers.EnergyAndWaters.Commands;
using BGTG.Web.Infrastructure.Attributes;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.EnergyAndWaterViewModels;
using Calabonga.OperationResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.POSControllers.EnergyAndWaters;

public class EnergyAndWatersController : Controller
{
    private readonly IEnergyAndWaterService _energyAndWaterService;
    private readonly IMediator _mediator;

    public EnergyAndWatersController(IEnergyAndWaterService energyAndWaterService, IMediator mediator)
    {
        _energyAndWaterService = energyAndWaterService;
        _mediator = mediator;
    }

    [ValidateModelState]
    public async Task<OperationResult<EnergyAndWaterCreateViewModel>> Write(EnergyAndWaterCreateViewModel viewModel) => 
        await _mediator.Send(new EnergyAndWaterWriteRequest(viewModel), HttpContext.RequestAborted);

    public async Task<ActionResult<OperationResult<Guid>>> WriteById(Guid id) => 
        await _mediator.Send(new EnergyAndWaterWriteByIdRequest(id), HttpContext.RequestAborted);

    public async Task<ActionResult<OperationResult<Guid>>> Delete(Guid id) => 
        await _mediator.Send(new EnergyAndWaterDeleteRequest(id), HttpContext.RequestAborted);

    public IActionResult Download()
    {
        var path = _energyAndWaterService.GetSavePath();
        return PhysicalFile(path, AppData.DocxMimeType, AppData.EnergyAndWaterDownloadFileName);
    }
}