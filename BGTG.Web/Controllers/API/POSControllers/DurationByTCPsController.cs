using System;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Core;
using BGTG.Core.Exceptions;
using BGTG.Data.CustomRepositories.Interfaces;
using BGTG.Entities.POSEntities.DurationByTCPToolEntities;
using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.Web.Infrastructure.Services.Interfaces;
using BGTG.Web.Infrastructure.Services.POSServices.Interfaces;
using BGTG.Web.ViewModels.POSViewModels.DurationByTCPViewModels;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.API.POSControllers
{
    [Route("api/[controller]")]
    public class DurationByTCPsController : UnitOfWorkController
    {
        private readonly IMapper _mapper;
        private readonly IDurationByTCPService _durationByTCPService;
        private readonly IConstructionObjectRepository _constructionObjectRepository;
        private readonly IRepository<InterpolationDurationByTCPEntity> _interpolationDurationByTCPRepository;
        private readonly IRepository<ExtrapolationDurationByTCPEntity> _extrapolationDurationByTCPRepository;
        private readonly IRepository<StepwiseExtrapolationDurationByTCPEntity> _stepwiseExtrapolationDurationByTCPRepository;

        public DurationByTCPsController(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IDurationByTCPService durationByTCPService,
            IConstructionObjectRepository constructionObjectRepository)
            : base(unitOfWork)
        {
            _durationByTCPService = durationByTCPService;
            _constructionObjectRepository = constructionObjectRepository;
            _mapper = mapper;
            _interpolationDurationByTCPRepository = unitOfWork.GetRepository<InterpolationDurationByTCPEntity>();
            _extrapolationDurationByTCPRepository = unitOfWork.GetRepository<ExtrapolationDurationByTCPEntity>();
            _stepwiseExtrapolationDurationByTCPRepository = unitOfWork.GetRepository<StepwiseExtrapolationDurationByTCPEntity>();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<OperationResult<string>>> Write([FromForm] DurationByTCPCreateViewModel viewModel)
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

            var durationByTCP = _durationByTCPService.Write(viewModel, User.Identity.Name);

            if (durationByTCP == null)
            {
                operationResult.AddError(AppData.DurationByTCPBadParametersValidationMessage);
                return OperationResultBeforeReturn(operationResult);
            }

            var constructionObject = await _constructionObjectRepository.GetFirstOrDefaultAsync(x => x.Cipher == viewModel.ObjectCipher, null, x => x
                   .Include(x => x.POS).ThenInclude(x => x.InterpolationDurationByTCP)
                   .Include(x => x.POS).ThenInclude(x => x.ExtrapolationDurationByTCP)
                   .Include(x => x.POS).ThenInclude(x => x.StepwiseExtrapolationDurationByTCP));

            if (constructionObject?.POS.InterpolationDurationByTCP != null)
            {
                _interpolationDurationByTCPRepository.Delete(constructionObject.POS.InterpolationDurationByTCP);
            }

            if (constructionObject?.POS.ExtrapolationDurationByTCP != null)
            {
                _extrapolationDurationByTCPRepository.Delete(constructionObject.POS.ExtrapolationDurationByTCP);
            }

            if (constructionObject?.POS.StepwiseExtrapolationDurationByTCP != null)
            {
                _stepwiseExtrapolationDurationByTCPRepository.Delete(constructionObject.POS.StepwiseExtrapolationDurationByTCP);
            }

            await UnitOfWork.SaveChangesAsync();

            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                operationResult.AddError(UnitOfWork.LastSaveChangesResult.Exception);
                return OperationResultBeforeReturn(operationResult);
            }

            var now = new DateTime(DateTime.Now.Ticks);
            switch (durationByTCP)
            {
                case InterpolationDurationByTCP interpolationDuration:
                    var interpolationDurationByTCPEntity = _mapper.Map<InterpolationDurationByTCPEntity>(interpolationDuration);
                    interpolationDurationByTCPEntity.CreatedBy = User.Identity.Name;
                    interpolationDurationByTCPEntity.CreatedAt = now;

                    await _constructionObjectRepository.Update(viewModel.ObjectCipher, interpolationDurationByTCPEntity);
                    break;
                case ExtrapolationDurationByTCP extrapolationDurationByTCP:
                    if (extrapolationDurationByTCP is StepwiseExtrapolationDurationByTCP stepwiseExtrapolationDuration)
                    {
                        var stepwiseExtrapolationDurationByTCPEntity = _mapper.Map<StepwiseExtrapolationDurationByTCPEntity>(stepwiseExtrapolationDuration);
                        stepwiseExtrapolationDurationByTCPEntity.CreatedBy = User.Identity.Name;
                        stepwiseExtrapolationDurationByTCPEntity.CreatedAt = now;

                        await _constructionObjectRepository.Update(viewModel.ObjectCipher, stepwiseExtrapolationDurationByTCPEntity);
                        break;
                    }
                    else
                    {
                        var extrapolationDurationByTCPEntity = _mapper.Map<ExtrapolationDurationByTCPEntity>(extrapolationDurationByTCP);
                        extrapolationDurationByTCPEntity.CreatedBy = User.Identity.Name;
                        extrapolationDurationByTCPEntity.CreatedAt = now;

                        await _constructionObjectRepository.Update(viewModel.ObjectCipher, extrapolationDurationByTCPEntity);
                        break;
                    }
                default:
                    operationResult.AddError(AppData.DurationByTCPUnknown);
                    return OperationResultBeforeReturn(operationResult);
            }

            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                operationResult.AddError(UnitOfWork.LastSaveChangesResult.Exception);
                return OperationResultBeforeReturn(operationResult);
            }

            operationResult.Result = string.Empty;
            return OperationResultBeforeReturn(operationResult);
        }

        [HttpPost("[action]/{id:guid}")]
        public async Task<ActionResult<OperationResult<string>>> WriteById(Guid id)
        {
            var operationResult = OperationResult.CreateResult<string>();

            operationResult.Result = string.Empty;

            var interpolationDurationByTCPEntity = await _interpolationDurationByTCPRepository.GetFirstOrDefaultAsync(x => x.Id == id, null, x => x.Include(x => x.CalculationPipelineStandards));
            if (interpolationDurationByTCPEntity != null)
            {
                var interpolationDurationByTCP = _mapper.Map<InterpolationDurationByTCP>(interpolationDurationByTCPEntity);

                _durationByTCPService.Write(interpolationDurationByTCP, User.Identity.Name);

                return OperationResultBeforeReturn(operationResult);
            }

            var extrapolationDurationByTCPEntity = await _extrapolationDurationByTCPRepository.GetFirstOrDefaultAsync(x => x.Id == id, null, x => x.Include(x => x.CalculationPipelineStandards));
            if (extrapolationDurationByTCPEntity != null)
            {
                var extrapolationDurationByTCP = _mapper.Map<ExtrapolationDurationByTCP>(extrapolationDurationByTCPEntity);

                _durationByTCPService.Write(extrapolationDurationByTCP, User.Identity.Name);

                return OperationResultBeforeReturn(operationResult);
            }

            var stepwiseExtrapolationDurationByTCPEntity = await _stepwiseExtrapolationDurationByTCPRepository.GetFirstOrDefaultAsync(x => x.Id == id, null,
                x => x.Include(x => x.CalculationPipelineStandards).Include(x => x.StepwisePipelineStandard));
            if (stepwiseExtrapolationDurationByTCPEntity != null)
            {
                var stepwiseExtrapolationDurationByTCP = _mapper.Map<StepwiseExtrapolationDurationByTCP>(stepwiseExtrapolationDurationByTCPEntity);

                _durationByTCPService.Write(stepwiseExtrapolationDurationByTCP, User.Identity.Name);

                return OperationResultBeforeReturn(operationResult);
            }

            operationResult.AddError(AppData.BadDurationByTCPId);
            return OperationResultBeforeReturn(operationResult);
        }

        [HttpGet("[action]")]
        public IActionResult Download()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var path = _durationByTCPService.GetSavePath(User.Identity.Name);

            return PhysicalFile(path, AppData.DocxMimeType, AppData.DurationByTCPDownloadFileName);
        }

        [HttpDelete("[action]/{id:guid}")]
        public async Task<ActionResult<OperationResult<string>>> DeleteItem(Guid id)
        {
            var operationResult = OperationResult.CreateResult<string>();

            operationResult.Result = string.Empty;

            var interpolationDurationByTCPEntity = await _interpolationDurationByTCPRepository.FindAsync(id);
            if (interpolationDurationByTCPEntity != null)
            {
                _interpolationDurationByTCPRepository.Delete(interpolationDurationByTCPEntity);

                await UnitOfWork.SaveChangesAsync();

                if (!UnitOfWork.LastSaveChangesResult.IsOk)
                {
                    operationResult.AddError(UnitOfWork.LastSaveChangesResult.Exception);
                    return OperationResultBeforeReturn(operationResult);
                }

                return OperationResultBeforeReturn(operationResult);
            }

            var extrapolationDurationByTCPEntity = await _extrapolationDurationByTCPRepository.FindAsync(id);
            if (extrapolationDurationByTCPEntity != null)
            {
                _extrapolationDurationByTCPRepository.Delete(extrapolationDurationByTCPEntity);

                await UnitOfWork.SaveChangesAsync();

                if (!UnitOfWork.LastSaveChangesResult.IsOk)
                {
                    operationResult.AddError(UnitOfWork.LastSaveChangesResult.Exception);
                    return OperationResultBeforeReturn(operationResult);
                }

                return OperationResultBeforeReturn(operationResult);
            }

            var stepwiseExtrapolationDurationByTCPEntity = await _stepwiseExtrapolationDurationByTCPRepository.FindAsync(id);
            if (stepwiseExtrapolationDurationByTCPEntity != null)
            {
                _stepwiseExtrapolationDurationByTCPRepository.Delete(stepwiseExtrapolationDurationByTCPEntity);

                await UnitOfWork.SaveChangesAsync();

                if (!UnitOfWork.LastSaveChangesResult.IsOk)
                {
                    operationResult.AddError(UnitOfWork.LastSaveChangesResult.Exception);
                    return OperationResultBeforeReturn(operationResult);
                }

                return OperationResultBeforeReturn(operationResult);
            }

            operationResult.AddError(new MicroserviceNotFoundException());
            return OperationResultBeforeReturn(operationResult);
        }
    }
}
