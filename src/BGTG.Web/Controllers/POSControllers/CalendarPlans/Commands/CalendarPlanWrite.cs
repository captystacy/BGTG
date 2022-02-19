using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.POS.CalendarPlanToolEntities;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.Base;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;

namespace BGTG.Web.Controllers.POSControllers.CalendarPlans.Commands;

public record CalendarPlanWriteRequest(CalendarPlanCreateViewModel ViewModel) : OperationResultRequestBase<CalendarPlanCreateViewModel>;

public class CalendarPlanWriteRequestHandler : OperationResultRequestHandlerBase<CalendarPlanWriteRequest, CalendarPlanCreateViewModel>
{
    private readonly ICalendarPlanService _calendarPlanService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConstructionObjectService _constructionObjectService;

    public CalendarPlanWriteRequestHandler(ICalendarPlanService calendarPlanService, IMapper mapper, IUnitOfWork unitOfWork, IConstructionObjectService constructionObjectService)
    {
        _calendarPlanService = calendarPlanService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _constructionObjectService = constructionObjectService;
    }

    public override async Task<OperationResult<CalendarPlanCreateViewModel>> Handle(CalendarPlanWriteRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<CalendarPlanCreateViewModel>();
        operation.Result = request.ViewModel;

        var calendarPlan = _calendarPlanService.Write(request.ViewModel);
        var calendarPlanEntity = _mapper.Map<CalendarPlanEntity>(calendarPlan);
        calendarPlanEntity.CreatedBy = IdentityHelper.Instance.User!.Name;
        calendarPlanEntity.CreatedAt = DateTime.UtcNow;

        await _constructionObjectService.Update(request.ViewModel.ObjectCipher, calendarPlanEntity);

        if (!_unitOfWork.LastSaveChangesResult.IsOk)
        {
            operation.AddError(new MicroserviceSaveChangesException());
            return operation;
        }

        return operation;
    }
}