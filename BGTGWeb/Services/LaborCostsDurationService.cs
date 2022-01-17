using System.Collections.Generic;
using System.IO;
using System.Text;
using BGTGWeb.Helpers;
using BGTGWeb.Models;
using BGTGWeb.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using POS.EstimateLogic;
using POS.LaborCostsDurationLogic.Interfaces;

namespace BGTGWeb.Services
{
    public class LaborCostsDurationService : ILaborCostsDurationService
    {
        private readonly IEstimateService _estimateService;
        private readonly ILaborCostsDurationCreator _laborCostsDurationCreator;
        private readonly ILaborCostsDurationWriter _laborCostsDurationWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string _roundingTemplatePart = "Rounding";
        private const string _acceptanceTemplatePart = "Acceptance";
        private const string _templateEndPart = "Template.docx";
        private const char _plusChar = '+';
        private const char _minusChar = '-';

        private const string _laborCostsDurationTemplatesPath = "Templates\\LaborCostsDurationTemplates";
        private const string _laborCostsDurationsPath = "UsersFiles\\LaborCostsDurations";

        public LaborCostsDurationService(IEstimateService estimateService, ILaborCostsDurationCreator laborCostsDurationCreator, ILaborCostsDurationWriter laborCostsDurationWriter, IWebHostEnvironment webHostEnvironment)
        {
            _estimateService = estimateService;
            _laborCostsDurationCreator = laborCostsDurationCreator;
            _laborCostsDurationWriter = laborCostsDurationWriter;
            _webHostEnvironment = webHostEnvironment;
        }

        public void WriteLaborCostsDuration(IEnumerable<IFormFile> estimateFiles, LaborCostsDurationVM laborCostsDurationVM, string userFullName)
        {
            _estimateService.ReadEstimateFiles(estimateFiles, TotalWorkChapter.TotalWork1To12Chapter);
            var laborCostsDuration = _laborCostsDurationCreator.Create(_estimateService.Estimate.LaborCosts, laborCostsDurationVM.WorkingDayDuration, laborCostsDurationVM.Shift,
                laborCostsDurationVM.NumberOfWorkingDays, laborCostsDurationVM.NumberOfEmployees, laborCostsDurationVM.AcceptanceTimeIncluded, laborCostsDurationVM.TechnologicalLaborCosts);
            var templateFileName = GetLaborCostsDurationTemplateFileName(laborCostsDuration.RoundingIncluded, laborCostsDuration.AcceptanceTimeIncluded, laborCostsDurationVM.TechnologicalLaborCosts);
            var templatePath = Path.Combine(_webHostEnvironment.WebRootPath, _laborCostsDurationTemplatesPath, templateFileName);
            var savePath = Path.Combine(GetLaborCostsDurationsPath(), GetLaborCostsDurationFileName(userFullName));
            _laborCostsDurationWriter.Write(laborCostsDuration, templatePath, savePath);
        }

        private string GetLaborCostsDurationTemplateFileName(bool roundingIncluded, bool acceptanceTimeIncluded, decimal technologicalLaborCosts)
        {
            var templateFileName = new StringBuilder();
            templateFileName.Append(_roundingTemplatePart);
            templateFileName.Append(GetPlusOrMinus(roundingIncluded));
            templateFileName.Append(_acceptanceTemplatePart);
            templateFileName.Append(GetPlusOrMinus(acceptanceTimeIncluded));
            templateFileName.Append(_templateEndPart);
            return templateFileName.ToString();
        }

        private char GetPlusOrMinus(bool condition)
        {
            return condition ? _plusChar : _minusChar;
        }

        public string GetLaborCostsDurationsPath()
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, _laborCostsDurationsPath);
        }

        public string GetLaborCostsDurationFileName(string userFullName)
        {
            return $"LaborCostsDuration{userFullName.RemoveBackslashes()}.docx";
        }

        public string GetDownloadLaborCostsDurationFileName()
        {
            return $"{_estimateService.Estimate.ObjectCipher}ПРОД.docx";
        }
    }
}
