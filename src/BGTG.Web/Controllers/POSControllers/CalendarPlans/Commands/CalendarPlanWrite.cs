using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities;
using BGTG.Entities.POSEntities;
using BGTG.Entities.POSEntities.CalendarPlanToolEntities;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.POSControllers.CalendarPlans.Commands;

public record CalendarPlanWriteRequest(CalendarPlanCreateViewModel ViewModel) : OperationResultRequestBase<CalendarPlanCreateViewModel>;

public class CalendarPlanWriteRequestHandler : OperationResultRequestHandlerBase<CalendarPlanWriteRequest, CalendarPlanCreateViewModel>
{
    private readonly ICalendarPlanService _calendarPlanService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CalendarPlanWriteRequestHandler(ICalendarPlanService calendarPlanService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _calendarPlanService = calendarPlanService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public override async Task<OperationResult<CalendarPlanCreateViewModel>> Handle(CalendarPlanWriteRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<CalendarPlanCreateViewModel>();
        operation.Result = request.ViewModel;

        var calendarPlan = _calendarPlanService.Write(request.ViewModel);
        var calendarPlanEntity = _mapper.Map<CalendarPlanEntity>(calendarPlan);
        calendarPlanEntity.CreatedBy = IdentityHelper.Instance.User!.Name;
        calendarPlanEntity.CreatedAt = DateTime.UtcNow;

        await UpdateConstructionObjectAsync(request.ViewModel.ObjectCipher, calendarPlanEntity);

        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangesResult.IsOk)
        {
            operation.AddError(new MicroserviceSaveChangesException());
            return operation;
        }

        return operation;
    }

    public async Task UpdateConstructionObjectAsync(string objectCipher, CalendarPlanEntity calendarPlanEntity)
    {
        var repository = _unitOfWork.GetRepository<ConstructionObjectEntity>();
        var constructionObject = await repository
            .GetFirstOrDefaultAsync(
                predicate: x => x.Cipher == objectCipher,
                include: x => x.Include(x => x.POS).ThenInclude(x => x!.CalendarPlan));

        if (constructionObject == null)
        {
            constructionObject = new ConstructionObjectEntity
            {
                CreatedBy = calendarPlanEntity.CreatedBy,
                CreatedAt = calendarPlanEntity.CreatedAt,
                Cipher = objectCipher,
                POS = new POSEntity
                {
                    CalendarPlan = calendarPlanEntity
                }
            };

            await repository.InsertAsync(constructionObject);
        }
        else if (constructionObject.POS == null)
        {
            constructionObject.UpdatedAt = calendarPlanEntity.CreatedAt;
            constructionObject.UpdatedBy = calendarPlanEntity.CreatedBy;
            constructionObject.POS = new POSEntity
            {
                CalendarPlan = calendarPlanEntity
            };

            repository.Update(constructionObject);
        }
        else
        {
            if (constructionObject.POS.CalendarPlan != null)
            {
                _unitOfWork.GetRepository<CalendarPlanEntity>().Delete(constructionObject.POS.CalendarPlan);
            }

            constructionObject.UpdatedAt = calendarPlanEntity.CreatedAt;
            constructionObject.UpdatedBy = calendarPlanEntity.CreatedBy;
            constructionObject.POS.CalendarPlan = calendarPlanEntity;

            repository.Update(constructionObject);
        }
    }
}