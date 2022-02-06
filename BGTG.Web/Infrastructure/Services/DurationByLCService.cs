using System.IO;
using System.Text;
using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.POS.DurationTools.DurationByLCTool.Interfaces;
using BGTG.POS.EstimateTool;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.Interfaces;
using BGTG.Web.ViewModels.POSViewModels.DurationByLCViewModels;
using Microsoft.AspNetCore.Hosting;

namespace BGTG.Web.Infrastructure.Services
{
    public class DurationByLCService : IDurationByLCService
    {
        private readonly IEstimateService _estimateService;
        private readonly IDurationByLCCreator _durationByLCCreator;
        private readonly IDurationByLCWriter _durationByLCWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string RoundingTemplatePart = "Rounding";
        private const string AcceptanceTemplatePart = "Acceptance";
        private const string TemplateEndPart = "Template.docx";
        private const char PlusChar = '+';
        private const char MinusChar = '-';

        private const string DurationByLCTemplatesPath = @"AppData\Templates\DurationByLCTemplates";
        private const string DurationByLCPath = @"AppData\UserFiles\DurationByLCFiles";

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
            var templateFileName = new StringBuilder();
            templateFileName.Append(RoundingTemplatePart);
            templateFileName.Append(GetPlusOrMinus(roundingIncluded));
            templateFileName.Append(AcceptanceTemplatePart);
            templateFileName.Append(GetPlusOrMinus(acceptanceTimeIncluded));
            templateFileName.Append(TemplateEndPart);

            return Path.Combine(_webHostEnvironment.ContentRootPath, DurationByLCTemplatesPath, templateFileName.ToString());
        }

        private char GetPlusOrMinus(bool condition)
        {
            return condition ? PlusChar : MinusChar;
        }

        public string GetSavePath(string windowsName)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, DurationByLCPath, $"DurationByLC{windowsName.RemoveBackslashes()}.docx");
        }
    }
}
