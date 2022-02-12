using System.IO;
using BGTG.Core;
using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.POS.DurationTools.DurationByLCTool.Interfaces;
using BGTG.POS.EstimateTool;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices.Interfaces;
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

        private const string TemplatesPath = @"AppData\Templates\DurationByLCTemplates";
        private const string UserFilesPath = @"AppData\UserFiles\DurationByLCFiles";

        public DurationByLCService(IEstimateService estimateService, IDurationByLCCreator durationByLCCreator, IDurationByLCWriter durationByLCWriter, IWebHostEnvironment webHostEnvironment)
        {
            _estimateService = estimateService;
            _durationByLCCreator = durationByLCCreator;
            _durationByLCWriter = durationByLCWriter;
            _webHostEnvironment = webHostEnvironment;
        }

        public DurationByLC Write(DurationByLCCreateViewModel viewModel, string windowsName)
        {
            _estimateService.Read(viewModel.EstimateFiles, TotalWorkChapter.TotalWork1To12Chapter);

            var durationByLC = _durationByLCCreator.Create(_estimateService.Estimate.LaborCosts, viewModel.TechnologicalLaborCosts, viewModel.WorkingDayDuration, viewModel.Shift,
                viewModel.NumberOfWorkingDays, viewModel.NumberOfEmployees, viewModel.AcceptanceTimeIncluded);

            return Write(durationByLC, windowsName);
        }

        public DurationByLC Write(DurationByLC durationByLC, string windowsName)
        {
            var templatePath = GetTemplatePath(durationByLC.RoundingIncluded, durationByLC.AcceptanceTimeIncluded);

            var savePath = GetSavePath(windowsName);

            _durationByLCWriter.Write(durationByLC, templatePath, savePath);

            return durationByLC;
        }

        private string GetTemplatePath(bool roundingIncluded, bool acceptanceTimeIncluded)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, $"Rounding{GetPlusOrMinus(roundingIncluded)}Acceptance{GetPlusOrMinus(acceptanceTimeIncluded)}.docx");
        }

        private char GetPlusOrMinus(bool condition)
        {
            return condition ? AppData.PlusChar : AppData.MinusChar;
        }

        public string GetSavePath(string windowsName)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, UserFilesPath, $"{windowsName.RemoveBackslashes()}.docx");
        }
    }
}
