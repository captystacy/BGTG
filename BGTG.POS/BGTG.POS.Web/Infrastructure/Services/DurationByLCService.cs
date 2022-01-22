using System.IO;
using System.Text;
using BGTG.POS.Tools.DurationTools.DurationByLCTool.Interfaces;
using BGTG.POS.Tools.EstimateTool;
using BGTG.POS.Web.Infrastructure.Helpers;
using BGTG.POS.Web.Infrastructure.Services.Interfaces;
using BGTG.POS.Web.ViewModels.DurationByLCViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BGTG.POS.Web.Infrastructure.Services
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

        public void Write(IFormFileCollection estimateFiles, DurationByLCViewModel durationByLCViewModel, string userFullName)
        {
            _estimateService.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter);

            var durationByLC = _durationByLCCreator.Create(_estimateService.Estimate.LaborCosts, durationByLCViewModel.TechnologicalLaborCosts, durationByLCViewModel.WorkingDayDuration, durationByLCViewModel.Shift,
                durationByLCViewModel.NumberOfWorkingDays, durationByLCViewModel.NumberOfEmployees, durationByLCViewModel.AcceptanceTimeIncluded);

            var templatePath = GetTemplatePath(durationByLC.RoundingIncluded, durationByLC.AcceptanceTimeIncluded);

            var savePath = GetSavePath(userFullName);

            _durationByLCWriter.Write(durationByLC, templatePath, savePath);
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

        public string GetSavePath(string userFullName)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, DurationByLCPath, $"DurationByLC{userFullName.RemoveBackslashes()}.docx");
        }

        public string GetFileName(string objectCipher = null)
        {
            return $"{objectCipher ?? _estimateService.Estimate.ObjectCipher}ПРОД.docx";
        }
    }
}
