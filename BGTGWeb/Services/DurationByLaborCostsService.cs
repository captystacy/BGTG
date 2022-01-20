using System.Collections.Generic;
using System.IO;
using System.Text;
using BGTGWeb.Helpers;
using BGTGWeb.Models;
using BGTGWeb.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using POS.DurationLogic.DurationByLaborCosts.Interfaces;
using POS.EstimateLogic;

namespace BGTGWeb.Services
{
    public class DurationByLaborCostsService : IDurationByLaborCostsService 
    {
        private readonly IEstimateService _estimateService;
        private readonly IDurationByLaborCostsCreator _durationByLaborCostsCreator;
        private readonly IDurationByLaborCostsWriter _durationByLaborCostsWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string RoundingTemplatePart = "Rounding";
        private const string AcceptanceTemplatePart = "Acceptance";
        private const string TemplateEndPart = "Template.docx";
        private const char PlusChar = '+';
        private const char MinusChar = '-';

        private const string DurationByLaborCostsTemplatesPath = @"Templates\DurationByLaborCostsTemplates";
        private const string DurationByLaborCostsPath = @"UsersFiles\DurationByLaborCosts";

        public DurationByLaborCostsService(IEstimateService estimateService, IDurationByLaborCostsCreator durationByLaborCostsCreator, IDurationByLaborCostsWriter durationByLaborCostsWriter, IWebHostEnvironment webHostEnvironment)
        {
            _estimateService = estimateService;
            _durationByLaborCostsCreator = durationByLaborCostsCreator;
            _durationByLaborCostsWriter = durationByLaborCostsWriter;
            _webHostEnvironment = webHostEnvironment;
        }

        public void Write(IEnumerable<IFormFile> estimateFiles, DurationByLaborCostsVM durationByLaborCostsVM, string userFullName)
        {
            _estimateService.Read(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter);

            var durationByLaborCosts = _durationByLaborCostsCreator.Create(_estimateService.Estimate.LaborCosts, durationByLaborCostsVM.TechnologicalLaborCosts, durationByLaborCostsVM.WorkingDayDuration, durationByLaborCostsVM.Shift,
                durationByLaborCostsVM.NumberOfWorkingDays, durationByLaborCostsVM.NumberOfEmployees, durationByLaborCostsVM.AcceptanceTimeIncluded);

            var templatePath = GetTemplatePath(durationByLaborCosts.RoundingIncluded, durationByLaborCosts.AcceptanceTimeIncluded);

            var savePath = GetSavePath(userFullName);

            _durationByLaborCostsWriter.Write(durationByLaborCosts, templatePath, savePath);
        }

        private string GetTemplatePath(bool roundingIncluded, bool acceptanceTimeIncluded)
        {
            var templateFileName = new StringBuilder();
            templateFileName.Append(RoundingTemplatePart);
            templateFileName.Append(GetPlusOrMinus(roundingIncluded));
            templateFileName.Append(AcceptanceTemplatePart);
            templateFileName.Append(GetPlusOrMinus(acceptanceTimeIncluded));
            templateFileName.Append(TemplateEndPart);

            return Path.Combine(_webHostEnvironment.WebRootPath, DurationByLaborCostsTemplatesPath, templateFileName.ToString());
        }

        private char GetPlusOrMinus(bool condition)
        {
            return condition ? PlusChar : MinusChar;
        }

        public string GetSavePath(string userFullName)
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, DurationByLaborCostsPath, $"DurationByLaborCosts{userFullName.RemoveBackslashes()}.docx");
        }

        public string GetFileName(string objectCipher = null)
        {
            return $"{objectCipher ?? _estimateService.Estimate.ObjectCipher}ПРОД.docx";
        }
    }
}
