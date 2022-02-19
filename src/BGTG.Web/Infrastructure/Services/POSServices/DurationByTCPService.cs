using System;
using System.IO;
using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.POS.DurationTools.DurationByTCPTool.Base;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.DurationByTCPViewModels;
using Microsoft.AspNetCore.Hosting;

namespace BGTG.Web.Infrastructure.Services.POSServices
{
    public class DurationByTCPService : IDurationByTCPService
    {
        private readonly IDurationByTCPCreator _durationByTCPCreator;
        private readonly IDurationByTCPWriter _durationByTCPWriter;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string TemplatesPath = @"AppData\Templates\POSTemplates\DurationByTCPTemplates";
        private const string UserFilesPath = @"AppData\UserFiles\POSFiles\DurationByTCPFiles";

        private const string InterpolationTemplateFileName = "Interpolation.docx";
        private const string ExtrapolationAscendingTemplateFileName = "ExtrapolationAscending.docx";
        private const string ExtrapolationDescendingTemplateFileName = "ExtrapolationDescending.docx";
        private const string StepwiseExtrapolationAscendingTemplateFileName = "StepwiseExtrapolationAscending.docx";
        private const string StepwiseExtrapolationDescendingTemplateFileName = "StepwiseExtrapolationDescending.docx";

        public DurationByTCPService(IDurationByTCPCreator durationByTCPCreator, IDurationByTCPWriter durationByTCPWriter, IWebHostEnvironment webHostEnvironment)
        {
            _durationByTCPCreator = durationByTCPCreator;
            _durationByTCPWriter = durationByTCPWriter;
            _webHostEnvironment = webHostEnvironment;
        }

        public DurationByTCP? Write(DurationByTCPCreateViewModel viewModel)
        {
            var durationByTCP = _durationByTCPCreator.Create(viewModel.PipelineMaterial, viewModel.PipelineDiameter,
                viewModel.PipelineLength, viewModel.AppendixKey, viewModel.PipelineCategoryName);

            if (durationByTCP is null)
            {
                return null;
            }

            return Write(durationByTCP);
        }

        public DurationByTCP Write(DurationByTCP durationByTCP)
        {
            var templatePath = GetTemplatePath(durationByTCP.DurationCalculationType);

            var savePath = GetSavePath();

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

        public string GetSavePath()
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, UserFilesPath, $"{IdentityHelper.Instance.User!.Name!.RemoveBackslashes()}.docx");
        }
    }
}
