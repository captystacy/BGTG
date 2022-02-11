using System;
using System.IO;
using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.POS.DurationTools.DurationByTCPTool.Interfaces;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.Interfaces;
using BGTG.Web.Infrastructure.Services.POSServices.Interfaces;
using BGTG.Web.ViewModels.POSViewModels.DurationByTCPViewModels;
using Microsoft.AspNetCore.Hosting;

namespace BGTG.Web.Infrastructure.Services.POSServices
{
    public class DurationByTCPService : IDurationByTCPService
    {
        private readonly IDurationByTCPCreator _durationByTCPCreator;
        private readonly IDurationByTCPWriter _durationByTCPWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string TemplatesPath = @"AppData\Templates\DurationByTCPTemplates";
        private const string UserFilesPath = @"AppData\UserFiles\DurationByTCPFiles";

        private const string InterpolationTemplateFileName = "InterpolationTemplate.docx";
        private const string ExtrapolationAscendingTemplateFileName = "ExtrapolationAscendingTemplate.docx";
        private const string ExtrapolationDescendingTemplateFileName = "ExtrapolationDescendingTemplate.docx";
        private const string StepwiseExtrapolationAscendingTemplateFileName = "StepwiseExtrapolationAscendingTemplate.docx";
        private const string StepwiseExtrapolationDescendingTemplateFileName = "StepwiseExtrapolationDescendingTemplate.docx";

        public DurationByTCPService(IDurationByTCPCreator durationByTCPCreator, IDurationByTCPWriter durationByTCPWriter, IWebHostEnvironment webHostEnvironment)
        {
            _durationByTCPCreator = durationByTCPCreator;
            _durationByTCPWriter = durationByTCPWriter;
            _webHostEnvironment = webHostEnvironment;
        }

        public DurationByTCP Write(DurationByTCPCreateViewModel viewModel, string windowsName)
        {
            var durationByTCP = _durationByTCPCreator.Create(viewModel.PipelineMaterial, viewModel.PipelineDiameter,
                viewModel.PipelineLength, viewModel.AppendixKey, viewModel.PipelineCategoryName);

            if (durationByTCP == null)
            {
                return null;
            }

            return Write(durationByTCP, windowsName);
        }

        public DurationByTCP Write(DurationByTCP durationByTCP, string windowsName)
        {
            var templatePath = GetTemplatePath(durationByTCP.DurationCalculationType);

            var savePath = GetSavePath(windowsName);

            _durationByTCPWriter.Write(durationByTCP, templatePath, savePath);

            return durationByTCP;
        }

        private string GetTemplatePath(DurationCalculationType durationCalculationType)
        {
            var templateFileName = durationCalculationType switch
            {
                DurationCalculationType.Interpolation => InterpolationTemplateFileName,
                DurationCalculationType.ExtrapolationAscending => ExtrapolationAscendingTemplateFileName,
                DurationCalculationType.ExtrapolationDescending => ExtrapolationDescendingTemplateFileName,
                DurationCalculationType.StepwiseExtrapolationAscending => StepwiseExtrapolationAscendingTemplateFileName,
                DurationCalculationType.StepwiseExtrapolationDescending => StepwiseExtrapolationDescendingTemplateFileName,
                _ => throw new ArgumentOutOfRangeException()
            };

            return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, templateFileName);
        }

        public string GetSavePath(string windowsName)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, UserFilesPath, $"DurationByTCP{windowsName.RemoveBackslashes()}.docx");
        }
    }
}
