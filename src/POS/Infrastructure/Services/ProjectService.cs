using POS.Infrastructure.Helpers;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly IECPProjectWriter _ecpProjectWriter;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private const string TemplatesPath = @"Infrastructure\Templates\ProjectTemplates";

    public ProjectService(IECPProjectWriter ecpProjectWriter, IWebHostEnvironment webHostEnvironment)
    {
        _ecpProjectWriter = ecpProjectWriter;
        _webHostEnvironment = webHostEnvironment;
    }

    public MemoryStream? Write(ProjectViewModel dto)
    {
        var durationByLCFile = dto.CalculationFiles.FirstOrDefault(x => x.FileName.Contains("Продолжительность по трудозатратам"));

        if (durationByLCFile is null)
        {
            return null;
        }

        var calendarPlanFile = dto.CalculationFiles.FirstOrDefault(x => x.FileName.Contains("Календарный план"));

        if (calendarPlanFile is null)
        {
            return null;
        }

        var energyAndWaterFile = dto.CalculationFiles.FirstOrDefault(x => x.FileName.Contains("Энергия и вода"));

        if (energyAndWaterFile is null)
        {
            return null;
        }

        var durationByLCStream = durationByLCFile.OpenReadStream();
        var numberOfEmployees = _ecpProjectWriter.GetNumberOfEmployees(durationByLCStream);

        var templatePath = GetTemplatePath(dto, numberOfEmployees);

        return _ecpProjectWriter.Write(durationByLCStream, calendarPlanFile.OpenReadStream(), energyAndWaterFile.OpenReadStream(), dto.ObjectCipher, templatePath);
    }

    private string GetTemplatePath(ProjectViewModel dto, int numberOfEmployees)
    {
        var templateFileName = $"HouseholdTown{TemplateHelper.GetPlusOrMinus(dto.HouseholdTownIncluded)}.doc";
        return Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath,
            dto.ProjectTemplate.ToString(), dto.ChiefProjectEngineer.ToString(),
            dto.ProjectEngineer.ToString(), $"Employees{numberOfEmployees}", templateFileName);
    }
}