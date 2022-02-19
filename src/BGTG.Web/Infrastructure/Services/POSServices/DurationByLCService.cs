using System.IO;
using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.POS.DurationTools.DurationByLCTool.Base;
using BGTG.POS.EstimateTool;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.DurationByLCViewModels;
using Microsoft.AspNetCore.Hosting;

namespace BGTG.Web.Infrastructure.Services.POSServices
{
    public class DurationByLCService : IDurationByLCService
    {
        private readonly IEstimateService _estimateService;
        private readonly IDurationByLCCreator _durationByLCCreator;
        private readonly IDurationByLCWriter _durationByLCWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string TemplatesPath = @"AppData\Templates\POSTemplates\DurationByLCTemplates";
        private const string UserFilesPath = @"AppData\UserFiles\POSFiles\DurationByLCFiles";

        public DurationByLCService(IEstimateService estimateService, IDurationByLCCreator durationByLCCreator, IDurationByLCWriter durationByLCWriter, IWebHostEnvironment webHostEnvironment)
        {
            _estimateService = estimateService;
            _durationByLCCreator = durationByLCCreator;
            _durationByLCWriter = durationByLCWriter;
            _webHostEnvironment = webHostEnvironment;
        }

        public DurationByLC Write(DurationByLCCreateViewModel viewModel)
        {
            _estimateService.Read(viewModel.EstimateFiles, TotalWorkChapter.TotalWork1To12Chapter);

            var durationByLC = _durationByLCCreator.Create(_estimateService.Estimate.LaborCosts, viewModel.TechnologicalLaborCosts, viewModel.WorkingDayDuration, viewModel.Shift,
                viewModel.NumberOfWorkingDays, viewModel.NumberOfEmployees, viewModel.AcceptanceTimeIncluded);

            return Write(durationByLC);
        }

        public DurationByLC Write(DurationByLC durationByLC)
        {
            var templatePath = GetTemplatePath(durationByLC.RoundingIncluded, durationByLC.AcceptanceTimeIncluded);

            var savePath = GetSavePath();

            _durationByLCWriter.Write(durationByLC, templatePath, savePath);

            return durationByLC;
        }

        private string GetTemplatePath(bool roundingIncluded, bool acceptanceTimeIncluded)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, $"Rounding{TemplateHelper.GetPlusOrMinus(roundingIncluded)}Acceptance{TemplateHelper.GetPlusOrMinus(acceptanceTimeIncluded)}.docx");
        }

        public string GetSavePath()
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, UserFilesPath, $"{IdentityHelper.Instance.User!.Name!.RemoveBackslashes()}.docx");
        }
    }
}
