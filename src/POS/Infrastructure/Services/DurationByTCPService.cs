using POS.DomainModels.DurationByTCPDomainModels;
using POS.Infrastructure.Creators.Base;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Infrastructure.Services;

public class DurationByTCPService : IDurationByTCPService
{
    private readonly IDurationByTCPCreator _durationByTCPCreator;
    private readonly IDurationByTCPWriter _durationByTCPWriter;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private const string TemplatesPath = @"Infrastructure\Templates\DurationByTCPTemplates";

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

    public MemoryStream? Write(DurationByTCPViewModel viewModel)
    {
        var durationByTCP = _durationByTCPCreator.Create(viewModel.PipelineMaterial, viewModel.PipelineDiameter,
            viewModel.PipelineLength, viewModel.AppendixKey, viewModel.PipelineCategoryName);

        if (durationByTCP is null)
        {
            return null;
        }

        var templatePath = GetTemplatePath(durationByTCP.DurationCalculationType);

        return _durationByTCPWriter.Write(durationByTCP, templatePath);
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
}