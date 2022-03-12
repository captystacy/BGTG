using POS.Infrastructure.Helpers;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.ProjectTool;
using POS.ViewModels;

namespace POS.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly IECPProjectWriter _ecpProjectWriter;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private const string TemplatesPath = @"Templates\ProjectTemplates";

    public ProjectService(IECPProjectWriter ecpProjectWriter, IWebHostEnvironment webHostEnvironment)
    {
        _ecpProjectWriter = ecpProjectWriter;
        _webHostEnvironment = webHostEnvironment;
    }

    public MemoryStream? Write(ProjectViewModel viewModel)
    {
        var durationByLCFile = viewModel.CalculationFiles.FirstOrDefault(x => x.FileName.Contains("Продолжительность по трудозатратам"));

        if (durationByLCFile is null)
        {
            return null;
        }

        var calendarPlanFile = viewModel.CalculationFiles.FirstOrDefault(x => x.FileName.Contains("Календарный план"));

        if (calendarPlanFile is null)
        {
            return null;
        }

        var energyAndWaterFile = viewModel.CalculationFiles.FirstOrDefault(x => x.FileName.Contains("Энергия и вода"));

        if (energyAndWaterFile is null)
        {
            return null;
        }

        var durationByLCStream = durationByLCFile.OpenReadStream();
        var numberOfEmployees = _ecpProjectWriter.GetNumberOfEmployees(durationByLCStream);

        var templatePath = GetTemplatePath(viewModel, numberOfEmployees);

        return _ecpProjectWriter.Write(durationByLCStream, calendarPlanFile.OpenReadStream(), energyAndWaterFile.OpenReadStream(), viewModel.ObjectCipher, templatePath);
    }

    private string GetTemplatePath(ProjectViewModel viewModel, int numberOfEmployees)
    {
        var templateFileName = $"HouseholdTown{TemplateHelper.GetPlusOrMinus(viewModel.HouseholdTownIncluded)}.docx";
        var templatePath = Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath,
            viewModel.ProjectTemplate.ToString(), viewModel.ChiefProjectEngineer.ToString(),
            viewModel.ProjectEngineer.ToString(), $"Employees{numberOfEmployees}", templateFileName);

        if (!File.Exists(templatePath))
        {
            return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath,
                viewModel.ProjectTemplate.ToString(), viewModel.ChiefProjectEngineer.ToString(),
                AppData.Unknown, $"Employees{numberOfEmployees}", templateFileName);
        }

        return templatePath;
    }
}