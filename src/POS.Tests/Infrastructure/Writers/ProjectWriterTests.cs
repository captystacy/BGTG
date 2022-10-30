using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Moq;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Infrastructure.Writers;
using POS.Models;
using POS.Tests.Helpers;
using POS.Tests.Helpers.Factories;
using POS.Tests.Helpers.Replacers;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using POS.ViewModels;
using Xunit;

namespace POS.Tests.Infrastructure.Writers
{
    public class ProjectWriterTests
    {

        public ProjectWriterTests()
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
        }

        [Fact]
        public async Task ItShould_replace_project_values()
        {
            // arrange

            var durationByLCStream = StreamHelper.GetMock();
            var calendarPlanStream = StreamHelper.GetMock();
            var energyAndWaterStream = StreamHelper.GetMock();
            var viewModel = new ProjectViewModel
            {
                CalculationFiles = new FormFileCollection
                {
                    FormFileHelper.GetMock(durationByLCStream, "Продолжительность по трудозатратам.docx").Object,
                    FormFileHelper.GetMock(calendarPlanStream, "Календарный план.docx").Object,
                    FormFileHelper.GetMock(energyAndWaterStream, "Энергия и вода.docx").Object,
                },
                ObjectCipher = "5.5-20.548",
                ProjectTemplate = ProjectTemplate.CPS,
                ProjectEngineer = Engineer.Kapitan,
                NormalInspectionEngineer = Engineer.Prishep,
                ChiefEngineer = Engineer.Selivanova,
                ChiefProjectEngineer = Engineer.Saiko,
            };
            var constructionStartDate = new DateTime(2022, 8, 1);
            var employeesNeed = new Fixture().Create<EmployeesNeed>();
            var durationByLC = new Fixture().Create<DurationByLC>();

            var durationByLCDocument = MyWordDocumentHelper.GetMock();

            var preparatoryTable = MyTableHelper.GetMock();
            var mainTable = MyTableHelper.GetMock();

            var energyAndWaterTable = MyTableHelper.GetMock();

            var calculationDictionary = new Dictionary<Mock<Stream>, Mock<IMyWordDocument>>
            {
                {durationByLCStream, durationByLCDocument},
                {calendarPlanStream, MyWordDocumentHelper.GetMock(MySectionHelper.GetMock(new List<Mock<IMyTable>> { preparatoryTable, mainTable }))},
                {energyAndWaterStream, MyWordDocumentHelper.GetMock(MySectionHelper.GetMock(new List<Mock<IMyTable>> { energyAndWaterTable }))},
            };

            var projectDocument = MyWordDocumentHelper.GetMock();

            var documentFactory = MyWordDocumentFactoryHelper.GetMock(@"root\Infrastructure\Templates\ProjectTemplates\CPSProjectTemplate.doc", projectDocument, calculationDictionary);
            var webHostEnvironment = WebHostEnvironmentHelper.GetMock("root");
            var projectReplacer = ProjectReplacerHelper.GetMock();
            var durationByLCReplacer = DurationByLCReplacerHelper.GetMock();
            var calendarPlanReplacer = CalendarPlanReplacerHelper.GetMock();
            var energyAndWaterReplacer = EnergyAndWaterReplacerHelper.GetMock();
            var engineerReplacer = EngineerReplacerHelper.GetMock();
            var employeesNeedReplacer = EmployeesNeedReplacerHelper.GetMock();
            var technicalAndEconomicalIndicatorsReplacer = TechnicalAndEconomicalIndicatorsReplacerHelper.GetMock();
            var sut = new ProjectWriter(documentFactory.Object, projectReplacer.Object, durationByLCReplacer.Object,
                calendarPlanReplacer.Object, energyAndWaterReplacer.Object, engineerReplacer.Object, employeesNeedReplacer.Object,
                technicalAndEconomicalIndicatorsReplacer.Object, webHostEnvironment.Object);

            // act

            await sut.GetProjectStream(viewModel, constructionStartDate, employeesNeed, durationByLC);

            // assert

            projectReplacer.Verify(x => x.ReplaceObjectCipher(projectDocument.Object, viewModel.ObjectCipher), Times.Once);
            projectReplacer.Verify(x => x.ReplaceCurrentDate(projectDocument.Object), Times.Once);
            projectReplacer.Verify(x => x.ReplaceConstructionStartDate(projectDocument.Object, constructionStartDate.ToString(Constants.DateTimeMonthStrAndYearFormat)), Times.Once);
            projectReplacer.Verify(x => x.ReplaceConstructionYear(projectDocument.Object, constructionStartDate.Year.ToString()), Times.Once);

            engineerReplacer.Verify(x => x.ReplaceSecondNameAndSignature(projectDocument.Object, viewModel.ProjectEngineer, TypeOfEngineer.ProjectEngineer), Times.Once);
            engineerReplacer.Verify(x => x.ReplaceSecondNameAndSignature(projectDocument.Object, viewModel.NormalInspectionEngineer, TypeOfEngineer.NormalInspectionProjectEngineer), Times.Once);
            engineerReplacer.Verify(x => x.ReplaceSecondNameAndSignature(projectDocument.Object, viewModel.NormalInspectionEngineer, TypeOfEngineer.NormalInspectionProjectEngineer), Times.Once);
            engineerReplacer.Verify(x => x.ReplaceSecondNameAndSignature(projectDocument.Object, viewModel.ChiefEngineer, TypeOfEngineer.ChiefEngineer), Times.Once);
            engineerReplacer.Verify(x => x.ReplaceSecondNameAndSignature(projectDocument.Object, viewModel.ChiefProjectEngineer, TypeOfEngineer.ChiefProjectEngineer), Times.Once);

            durationByLCReplacer.Verify(x => x.Replace(projectDocument.Object, durationByLCDocument.Object), Times.Once);

            calendarPlanReplacer.Verify(x => x.Replace(projectDocument.Object, preparatoryTable.Object, mainTable.Object), Times.Once);

            energyAndWaterReplacer.Verify(x => x.Replace(projectDocument.Object, energyAndWaterTable.Object), Times.Once);

            employeesNeedReplacer.Verify(x => x.Replace(projectDocument.Object, employeesNeed));

            technicalAndEconomicalIndicatorsReplacer.Verify(x => x.Replace(projectDocument.Object, durationByLC), Times.Once);
        }
    }
}
