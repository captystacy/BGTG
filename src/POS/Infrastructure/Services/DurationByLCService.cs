using POS.Infrastructure.Helpers;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.DurationTools.DurationByLCTool;
using POS.Infrastructure.Tools.DurationTools.DurationByLCTool.Base;
using POS.Infrastructure.Tools.EstimateTool.Models;
using POS.ViewModels;

namespace POS.Infrastructure.Services;

public class DurationByLCService : IDurationByLCService
{
    private readonly IEstimateService _estimateService;
    private readonly IDurationByLCCreator _durationByLCCreator;
    private readonly IDurationByLCWriter _durationByLCWriter;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private const string TemplatesPath = @"AppData\Templates\POSTemplates\DurationByLCTemplates";

    public DurationByLCService(IEstimateService estimateService, IDurationByLCCreator durationByLCCreator, IDurationByLCWriter durationByLCWriter, IWebHostEnvironment webHostEnvironment)
    {
        _estimateService = estimateService;
        _durationByLCCreator = durationByLCCreator;
        _durationByLCWriter = durationByLCWriter;
        _webHostEnvironment = webHostEnvironment;
    }

    public DurationByLC Write(DurationByLCCreateViewModel viewModel)
    {
        _estimateService.Read(viewModel.EstimateFiles, TotalWorkChapter.TotalWork1To12Chapter);

        var durationByLC = _durationByLCCreator.Create(_estimateService.Estimate.LaborCosts, viewModel.TechnologicalLaborCosts, viewModel.WorkingDayDuration, viewModel.Shift,
            viewModel.NumberOfWorkingDays, viewModel.NumberOfEmployees, viewModel.AcceptanceTimeIncluded);

        return Write(durationByLC);
    }

    public DurationByLC Write(DurationByLC durationByLC)
    {
        var templatePath = GetTemplatePath(durationByLC.RoundingIncluded, durationByLC.AcceptanceTimeIncluded);

        _durationByLCWriter.Write(durationByLC, templatePath);

        return durationByLC;
    }

    private string GetTemplatePath(bool roundingIncluded, bool acceptanceTimeIncluded)
    {
        return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, $"Rounding{TemplateHelper.GetPlusOrMinus(roundingIncluded)}Acceptance{TemplateHelper.GetPlusOrMinus(acceptanceTimeIncluded)}.docx");
    }
}