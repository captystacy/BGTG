using System;
using System.Threading.Tasks;
using BGTG.Entities.Core;
using BGTG.Web.Controllers.POSControllers.POSes.Commands;
using BGTG.Web.Infrastructure.Attributes;
using BGTG.Web.Infrastructure.Providers.POSProviders.Base;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels;
using Calabonga.OperationResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.POSControllers.POSes;

public class POSController : Controller
{
    private readonly IProjectProvider _projectProvider;
    private readonly ITableOfContentsService _tableOfContentsService;
    private readonly ITitlePageService _titlePageService;
    private readonly IMediator _mediator;

    public POSController(IProjectProvider projectProvider, ITableOfContentsService tableOfContentsService, ITitlePageService titlePageService, IMediator mediator)
    {
        _projectProvider = projectProvider;
        _tableOfContentsService = tableOfContentsService;
        _titlePageService = titlePageService;
        _mediator = mediator;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<OperationResult<Guid>> Delete(Guid id) => 
        await _mediator.Send(new POSDeleteRequest(id), HttpContext.RequestAborted);

    [ValidateModelState]
    public async Task<OperationResult<ProjectViewModel>> WriteProject(ProjectViewModel viewModel) => 
        await _projectProvider.Write(viewModel);

    public IActionResult DownloadProject()
    {
        var path = _projectProvider.GetSavePath();
        return PhysicalFile(path, AppData.DocxMimeType, AppData.ECPProjectDownloadFileName);
    }

    [ValidateModelState]
    public OperationResult<TableOfContentsViewModel> WriteTableOfContents(TableOfContentsViewModel viewModel)
    {
        var operation = OperationResult.CreateResult<TableOfContentsViewModel>();
        operation.Result = viewModel;

        _tableOfContentsService.Write(viewModel);

        return operation;
    }

    public IActionResult DownloadTableOfContents()
    {
        var path = _tableOfContentsService.GetSavePath();
        return PhysicalFile(path, AppData.DocxMimeType, AppData.TableOfContentsDownloadFileName);
    }

    [ValidateModelState]
    public OperationResult<TitlePageViewModel> WriteTitlePage(TitlePageViewModel viewModel)
    {
        var operation = OperationResult.CreateResult<TitlePageViewModel>();
        operation.Result = viewModel;

        _titlePageService.Write(viewModel);

        return operation;
    }

    public IActionResult DownloadTitlePage()
    {
        var path = _titlePageService.GetSavePath();
        return PhysicalFile(path, AppData.DocxMimeType, AppData.TitlePageDownloadFileName);
    }
}