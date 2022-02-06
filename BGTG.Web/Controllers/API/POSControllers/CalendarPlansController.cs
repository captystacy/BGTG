using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Core;
using BGTG.Data.CustomRepositories.Interfaces;
using BGTG.Entities.POSEntities.CalendarPlanToolEntities;
using BGTG.POS.CalendarPlanTool;
using BGTG.Web.Infrastructure.Services.Interfaces;
using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;
using Calabonga.Microservices.Core.QueryParams;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Calabonga.UnitOfWork.Controllers.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.API.POSControllers
{
    [Route("api/[controller]")]
    public class CalendarPlansController : WritableController<CalendarPlanViewModel, CalendarPlanEntity,
        CalendarPlanCreateViewModel, CalendarPlanUpdateViewModel, PagedListQueryParams>
    {
        private readonly ICalendarPlanService _calendarPlanService;
        private readonly IConstructionObjectRepository _constructionObjectRepository;

        public CalendarPlansController(
            IMapper mapper,
            IEntityManagerFactory entityManagerFactory,
            IUnitOfWork unitOfWork,
            ICalendarPlanService calendarPlanService,
            IConstructionObjectRepository constructionObjectRepository)
            : base(entityManagerFactory, unitOfWork, mapper)
        {
            _calendarPlanService = calendarPlanService;
            _constructionObjectRepository = constructionObjectRepository;
        }

        [HttpPost("[action]")]
        public ActionResult<OperationResult<CalendarPlanCreateViewModel>> GetCalendarPlanCreateViewmodel([FromForm] CalendarPlanPreCreateViewModel viewModel)
        {
            var operationResult = OperationResult.CreateResult<CalendarPlanCreateViewModel>();

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Root.Errors)
                {
                    operationResult.AddError(error.ErrorMessage);
                }
                return OperationResultBeforeReturn(operationResult);
            }

            operationResult.Result = _calendarPlanService.GetCalendarPlanCreateViewModel(viewModel);
            return OperationResultBeforeReturn(operationResult);
        }

        [HttpPost("[action]")]
        public ActionResult<OperationResult<IEnumerable<decimal>>> GetTotalPercentages([FromForm] CalendarPlanCreateViewModel viewModel)
        {
            var operationResult = OperationResult.CreateResult<IEnumerable<decimal>>();

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Root.Errors)
                {
                    operationResult.AddError(error.ErrorMessage);
                }
                return OperationResultBeforeReturn(operationResult);
            }

            operationResult.Result = _calendarPlanService.GetTotalPercentages(viewModel);
            return OperationResultBeforeReturn(operationResult);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<OperationResult<string>>> Write([FromForm] CalendarPlanCreateViewModel viewModel)
        {
            var operationResult = OperationResult.CreateResult<string>();

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Root.Errors)
                {
                    operationResult.AddError(error.ErrorMessage);
                }
                return OperationResultBeforeReturn(operationResult);
            }

            var calendarPlan = _calendarPlanService.Write(viewModel, User.Identity.Name);
            var calendarPlanEntity = CurrentMapper.Map<CalendarPlanEntity>(calendarPlan);
            calendarPlanEntity.CreatedBy = User.Identity.Name;
            calendarPlanEntity.CreatedAt = new DateTime(DateTime.Now.Ticks);

            await _constructionObjectRepository.Update(viewModel.ObjectCipher, calendarPlanEntity);

            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                operationResult.AddError(UnitOfWork.LastSaveChangesResult.Exception);
                return OperationResultBeforeReturn(operationResult);
            }

            operationResult.Result = string.Empty;
            return OperationResultBeforeReturn(operationResult);
        }

        [HttpGet("[action]")]
        public IActionResult Download()
        {
            var path = _calendarPlanService.GetSavePath(User.Identity.Name);

            return PhysicalFile(path, AppData.DocxMimeType, AppData.CalendarPlanDownloadFileName);
        }

        [HttpPost("[action]/{id:guid}")]
        public async Task<ActionResult<OperationResult<string>>> WriteById(Guid id)
        {
            var operationResult = OperationResult.CreateResult<string>();

            var calendarPlanEntity = await Repository.GetFirstOrDefaultAsync(x => x.Id == id, null,
                x => x
                    .Include(x => x.MainCalendarWorks).ThenInclude(x => x.ConstructionMonths)
                    .Include(x => x.PreparatoryCalendarWorks).ThenInclude(x => x.ConstructionMonths));

            if (calendarPlanEntity == null)
            {
                operationResult.AddError(AppData.BadCalendarPlanId);
                return OperationResultBeforeReturn(operationResult);
            }

            var calendarPlan = CurrentMapper.Map<CalendarPlan>(calendarPlanEntity);

            _calendarPlanService.Write(calendarPlan, User.Identity.Name);

            operationResult.Result = string.Empty;
            return OperationResultBeforeReturn(operationResult);
        }
    }
}
