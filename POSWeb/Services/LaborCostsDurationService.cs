using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using POSCore.DurationLogic.LaborCosts.Interfaces;
using POSWeb.Helpers;
using POSWeb.Models;
using POSWeb.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace POSWeb.Services
{
    public class LaborCostsDurationService : ILaborCostsDurationService
    {
        private readonly IEstimateService _estimateService;
        private readonly ILaborCostsDurationCreator _laborCostsDurationCreator;
        private readonly ILaborCostsDurationWriter _laborCostsDurationWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string _roundingTemplatePart = "Rounding";
        private const string _acceptanceTemplatePart = "Acceptance";
        private const string _techcostsTemplatePart = "Techcosts";
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
            _estimateService.ReadEstimateFiles(estimateFiles);
            var laborCostsDuration = _laborCostsDurationCreator.Create(_estimateService.Estimate.LaborCosts + laborCostsDurationVM.TechnologicalLaborCosts, laborCostsDurationVM.WorkingDayDuration, laborCostsDurationVM.Shift,
                laborCostsDurationVM.NumberOfWorkingDays, laborCostsDurationVM.NumberOfEmployees, laborCostsDurationVM.AcceptanceTimeIncluded);
            var templateFileName = GetLaborCostsDurationTemplateFileName(laborCostsDuration.RoundingIncluded, laborCostsDuration.AcceptanceTimeIncluded, laborCostsDurationVM.TechnologicalLaborCosts);
            var templatePath = Path.Combine(_webHostEnvironment.WebRootPath, _laborCostsDurationTemplatesPath, templateFileName);
            var savePath = GetLaborCostsDurationsPath();
            var fileName = GetLaborCostsDurationFileName(userFullName);
            _laborCostsDurationWriter.Write(laborCostsDuration, templatePath, savePath, fileName);
        }

        private string GetLaborCostsDurationTemplateFileName(bool roundingIncluded, bool acceptanceTimeIncluded, decimal technologistsLaborCosts)
        {
            var templateFileName = new StringBuilder();
            templateFileName.Append(_roundingTemplatePart);
            templateFileName.Append(GetPlusOrMinus(roundingIncluded));
            templateFileName.Append(_acceptanceTemplatePart);
            templateFileName.Append(GetPlusOrMinus(acceptanceTimeIncluded));
            templateFileName.Append(_techcostsTemplatePart);
            templateFileName.Append(GetPlusOrMinus(technologistsLaborCosts > 0));
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
