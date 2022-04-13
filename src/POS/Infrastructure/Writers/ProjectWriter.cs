using System.Globalization;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Factories.Base;
using POS.Infrastructure.Replacers.Base;
using POS.Infrastructure.Services.DocumentServices;
using POS.Infrastructure.Writers.Base;
using POS.Models;
using POS.ViewModels;

namespace POS.Infrastructure.Writers
{
    public class ProjectWriter : IProjectWriter
    {
        private readonly IMyWordDocumentFactory _documentFactory;
        private readonly IProjectReplacer _projectReplacer;
        private readonly IDurationByLCReplacer _durationByLCReplacer;
        private readonly ICalendarPlanReplacer _calendarPlanReplacer;
        private readonly IEnergyAndWaterReplacer _energyAndWaterReplacer;
        private readonly IEngineerReplacer _engineerReplacer;
        private readonly IEmployeesNeedReplacer _employeesNeedReplacer;
        private readonly ITechnicalAndEconomicalIndicatorsReplacer _technicalAndEconomicalIndicatorsReplacer;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string TemplatesPath = @"Infrastructure\Templates\ProjectTemplates";

        public ProjectWriter(
            IMyWordDocumentFactory documentFactory, 
            IProjectReplacer projectReplacer,
            IDurationByLCReplacer durationByLCReplacer,
            ICalendarPlanReplacer calendarPlanReplacer,
            IEnergyAndWaterReplacer energyAndWaterReplacer,
            IEngineerReplacer engineerReplacer,
            IEmployeesNeedReplacer employeesNeedReplacer,
            ITechnicalAndEconomicalIndicatorsReplacer technicalAndEconomicalIndicatorsReplacer,
            IWebHostEnvironment webHostEnvironment)
        {
            _documentFactory = documentFactory;
            _projectReplacer = projectReplacer;
            _durationByLCReplacer = durationByLCReplacer;
            _calendarPlanReplacer = calendarPlanReplacer;
            _energyAndWaterReplacer = energyAndWaterReplacer;
            _engineerReplacer = engineerReplacer;
            _employeesNeedReplacer = employeesNeedReplacer;
            _technicalAndEconomicalIndicatorsReplacer = technicalAndEconomicalIndicatorsReplacer;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<MemoryStream> GetProjectStream(ProjectViewModel viewModel, DateTime constructionStartDate, EmployeesNeed employeesNeed, DurationByLC durationByLC)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            var templatePath = Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, $"{viewModel.ProjectTemplate}ProjectTemplate.doc");

            var durationByLCStream = viewModel.CalculationFiles.First(x => x.FileName.Contains(Constants.DurationByLCFileName)).OpenReadStream();
            var durationByLCDocument = await _documentFactory.CreateAsync(durationByLCStream);

            var calendarPlanStream = viewModel.CalculationFiles.First(x => x.FileName.Contains(Constants.CalendarPlanFileName)).OpenReadStream();
            var calendarPlanDocument = await _documentFactory.CreateAsync(calendarPlanStream);

            var energyAndWaterStream = viewModel.CalculationFiles.First(x => x.FileName.Contains(Constants.EnergyAndWaterFileName)).OpenReadStream();
            var energyAndWaterDocument = await _documentFactory.CreateAsync(energyAndWaterStream);

            using var projectDocument = await _documentFactory.CreateAsync(templatePath);

            var tasks = new List<Task>
            {
                _projectReplacer.ReplaceObjectCipher(projectDocument, viewModel.ObjectCipher),
                _projectReplacer.ReplaceCurrentDate(projectDocument),
                _projectReplacer.ReplaceConstructionStartDate(projectDocument, constructionStartDate.ToString(Constants.DateTimeMonthStrAndYearFormat)),
                _projectReplacer.ReplaceConstructionYear(projectDocument, constructionStartDate.Year.ToString()),
                _engineerReplacer.ReplaceSecondNameAndSignature(projectDocument, viewModel.ProjectEngineer, TypeOfEngineer.ProjectEngineer),
                _engineerReplacer.ReplaceSecondNameAndSignature(projectDocument, viewModel.NormalInspectionEngineer, TypeOfEngineer.NormalInspectionProjectEngineer),
                _engineerReplacer.ReplaceSecondNameAndSignature(projectDocument, viewModel.ChiefEngineer, TypeOfEngineer.ChiefEngineer),
                _engineerReplacer.ReplaceSecondNameAndSignature(projectDocument, viewModel.ChiefProjectEngineer, TypeOfEngineer.ChiefProjectEngineer),
                _durationByLCReplacer.Replace(projectDocument, durationByLCDocument),
                _calendarPlanReplacer.Replace(projectDocument, calendarPlanDocument.Sections[0].Tables[0], calendarPlanDocument.Sections[0].Tables[1]),
                _energyAndWaterReplacer.Replace(projectDocument, energyAndWaterDocument.Sections[0].Tables[0]),
                _employeesNeedReplacer.Replace(projectDocument, employeesNeed),
                _technicalAndEconomicalIndicatorsReplacer.Replace(projectDocument, durationByLC)
            };

            await Task.WhenAll(tasks);

            var memoryStream = new MemoryStream();
            projectDocument.SaveAs(memoryStream, MyFileFormat.Doc);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }
    }
}