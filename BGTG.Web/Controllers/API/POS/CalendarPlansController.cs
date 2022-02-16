using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.Core;
using BGTG.Entities.POS.CalendarPlanToolEntities;
using BGTG.POS.CalendarPlanTool;
using BGTG.Web.Infrastructure.Attributes;
using BGTG.Web.Infrastructure.Services.BGTG.Base;
using BGTG.Web.Infrastructure.Services.POS.Base;
using BGTG.Web.ViewModels.POS.CalendarPlanViewModels;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.API.POS;

[Route("api/[controller]")]
public class CalendarPlansController : UnitOfWorkController
{
    private readonly IMapper _mapper;
    private readonly ICalendarPlanService _calendarPlanService;
    private readonly IConstructionObjectService _constructionObjectService;

    public CalendarPlansController(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ICalendarPlanService calendarPlanService,
        IConstructionObjectService constructionObjectService)
        : base(unitOfWork)
    {
        _mapper = mapper;
        _calendarPlanService = calendarPlanService;
        _constructionObjectService = constructionObjectService;
    }

    [HttpPost("[action]")]
    [ProducesResponseType(200)]
    [ValidateModelState]
    public ActionResult<OperationResult<CalendarPlanCreateViewModel>> GetCalendarPlanCreateViewmodel([FromForm] CalendarPlanPreCreateViewModel viewModel)
    {
        var calendarPlanCreateViewModel = _calendarPlanService.GetCalendarPlanCreateViewModel(viewModel);

        return OperationResultSuccess(calendarPlanCreateViewModel);
    }

    [HttpPost("[action]")]
    [ProducesResponseType(200)]
    [ValidateModelState]
    public ActionResult<OperationResult<IEnumerable<decimal>>> GetTotalPercentages([FromForm] CalendarPlanCreateViewModel viewModel)
    {
        var totalPercentages = _calendarPlanService.GetTotalPercentages(viewModel);

        return OperationResultSuccess(totalPercentages);
    }

    [HttpPost("[action]")]
    [ProducesResponseType(200)]
    [ValidateModelState]
    public async Task<ActionResult<OperationResult<CalendarPlanCreateViewModel>>> Write([FromForm] CalendarPlanCreateViewModel viewModel)
    {
        var calendarPlan = _calendarPlanService.Write(viewModel);
        var calendarPlanEntity = _mapper.Map<CalendarPlanEntity>(calendarPlan);
        calendarPlanEntity.CreatedBy = GetUserIdentityFromRequest();
        calendarPlanEntity.CreatedAt = DateTime.UtcNow;

        await _constructionObjectService.Update(viewModel.ObjectCipher, calendarPlanEntity);

        if (!UnitOfWork.LastSaveChangesResult.IsOk)
        {
            return OperationResultError(viewModel, new MicroserviceDatabaseException());
        }

        return OperationResultSuccess(viewModel);
    }

    [HttpPost("[action]/{id:guid}")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<OperationResult<Guid>>> WriteById(Guid id)
    {
        var calendarPlanEntity = await UnitOfWork.GetRepository<CalendarPlanEntity>().GetFirstOrDefaultAsync(
            predicate: x => x.Id == id,
            include: x => x
                 .Include(x => x.MainCalendarWorks).ThenInclude(x => x.ConstructionMonths)
                 .Include(x => x.PreparatoryCalendarWorks).ThenInclude(x => x.ConstructionMonths));

        if (calendarPlanEntity == null)
        {
            return OperationResultError(id);
        }

        var calendarPlan = _mapper.Map<CalendarPlan>(calendarPlanEntity);

        _calendarPlanService.Write(calendarPlan);

        return OperationResultSuccess(id);
    }

    [HttpDelete("[action]/{id:guid}")]
    [ProducesResponseType(200)]
    public virtual async Task<ActionResult<OperationResult<Guid>>> DeleteItem(Guid id)
    {
        var repository = UnitOfWork.GetRepository<CalendarPlanEntity>();
        var calendarPlanEntity = await repository.FindAsync(id);

        if (calendarPlanEntity == null)
        {
            return OperationResultError(id, new MicroserviceNotFoundException());
        }

        repository.Delete(calendarPlanEntity);

        await UnitOfWork.SaveChangesAsync();

        if (!UnitOfWork.LastSaveChangesResult.IsOk)
        {
            return OperationResultError(id, new MicroserviceDatabaseException());
        }

        return OperationResultSuccess(id);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(200)]
    public IActionResult Download()
    {
        var path = _calendarPlanService.GetSavePath();

        return PhysicalFile(path, AppData.DocxMimeType, AppData.CalendarPlanDownloadFileName);
    }
}