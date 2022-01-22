using System;
using System.IO;
using BGTG.POS.Tools.DurationTools.DurationByTCPTool;
using BGTG.POS.Tools.DurationTools.DurationByTCPTool.Interfaces;
using BGTG.POS.Web.Infrastructure.Helpers;
using BGTG.POS.Web.Infrastructure.Services.Interfaces;
using BGTG.POS.Web.ViewModels.DurationByTCPViewModels;
using Microsoft.AspNetCore.Hosting;

namespace BGTG.POS.Web.Infrastructure.Services
{
    public class DurationByTCPService : IDurationByTCPService
    {
        private readonly IDurationByTCPCreator _durationByTCPCreator;
        private readonly IDurationByTCPWriter _durationByTCPWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string DurationByTCPPath = @"AppData\UserFiles\DurationByTCPFiles";
        private const string DurationByTCPTemplatesPath = @"AppData\Templates\DurationByTCPTemplates";

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

        public bool Write(DurationByTCPViewModel durationByTCPViewModel)
        {
            var durationByTCP = _durationByTCPCreator.Create(durationByTCPViewModel.PipelineMaterial, durationByTCPViewModel.PipelineDiameter,
                durationByTCPViewModel.PipelineLength, durationByTCPViewModel.AppendixKey, durationByTCPViewModel.PipelineCategoryName);

            if (durationByTCP == null)
            {
                return false;
            }

            var templatePath = GetTemplatePath(durationByTCP.DurationCalculationType);

            var savePath = GetSavePath(durationByTCPViewModel.UserFullName);

            _durationByTCPWriter.Write(durationByTCP, templatePath, savePath);

            return true;
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

            return Path.Combine(_webHostEnvironment.ContentRootPath, DurationByTCPTemplatesPath, templateFileName);
        }

        public string GetSavePath(string userFullName)
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, DurationByTCPPath, $"DurationByTCP{userFullName.RemoveBackslashes()}.docx");
        }

        public string GetFileName(string objectCipher = null)
        {
            return $"{objectCipher}ПРОД.docx";
        }
    }
}
