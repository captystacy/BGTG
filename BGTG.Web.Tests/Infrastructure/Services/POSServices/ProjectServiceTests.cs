using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Data.CustomRepositories.Interfaces;
using BGTG.Entities;
using BGTG.Entities.POSEntities;
using BGTG.Entities.POSEntities.CalendarPlanToolEntities;
using BGTG.Entities.POSEntities.DurationByLCToolEntities;
using BGTG.Entities.POSEntities.EnergyAndWaterToolEntities;
using BGTG.POS;
using BGTG.POS.CalendarPlanTool;
using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.POS.EnergyAndWaterTool;
using BGTG.POS.ProjectTool;
using BGTG.POS.ProjectTool.Interfaces;
using BGTG.Web.Infrastructure.Services.POSServices;
using BGTG.Web.Infrastructure.Services.POSServices.Interfaces;
using BGTG.Web.ViewModels.POSViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace BGTG.Web.Tests.Infrastructure.Services.POSServices
{
    public class ProjectServiceTests
    {
        private ProjectService _projectService;
        private Mock<IECPProjectWriter> _ecpProjectWriterMock;
        private Mock<IDurationByLCService> _durationByLCServiceMock;
        private Mock<ICalendarPlanService> _calendarPlanServiceMock;
        private Mock<IEnergyAndWaterService> _energyAndWaterServiceMock;
        private Mock<IConstructionObjectRepository> _constructionObjectRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;

        [SetUp]
        public void SetUp()
        {
            _ecpProjectWriterMock = new Mock<IECPProjectWriter>();
            _durationByLCServiceMock = new Mock<IDurationByLCService>();
            _calendarPlanServiceMock = new Mock<ICalendarPlanService>();
            _energyAndWaterServiceMock = new Mock<IEnergyAndWaterService>();
            _constructionObjectRepositoryMock = new Mock<IConstructionObjectRepository>();
            _mapperMock = new Mock<IMapper>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _projectService = new ProjectService(_ecpProjectWriterMock.Object, _durationByLCServiceMock.Object,
                _calendarPlanServiceMock.Object, _energyAndWaterServiceMock.Object,
                _constructionObjectRepositoryMock.Object, _mapperMock.Object, _webHostEnvironmentMock.Object);
        }

        [Test]
        public async Task Write()
        {
            var windowsName = "BGTG\\kss";
            var viewModel = new ProjectViewModel
            {
                ObjectCipher = "5.5-20.548",
                ChiefProjectEngineer = ChiefProjectEngineer.Saiko,
                ProjectTemplate = ProjectTemplate.ECP,
                HouseholdTownIncluded = true,
            };

            var templatePath = @"wwwroot\AppData\Templates\ProjectTemplates\ECP\Saiko\Unknown\Employees4\HouseholdTown+.docx";
            var savePath = @"wwwroot\AppData\UserFiles\ProjectFiles\BGTGkss.docx";

            var constructionObject = new ConstructionObjectEntity
            {
                POS = new POSEntity
                {
                    DurationByLC = new DurationByLCEntity() { NumberOfEmployees = 4 },
                    CalendarPlan = new CalendarPlanEntity() { ConstructionStartDate = new DateTime(DateTime.Now.Ticks) },
                    EnergyAndWater = new EnergyAndWaterEntity(),
                }
            };

            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");

            _constructionObjectRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(), null,
                It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(), true, false).Result).Returns(constructionObject);

            var durationByLC = new DurationByLC(default, default, default, default, default, default, default, 4, default, default, default, default, default, default);
            _mapperMock.Setup(x => x.Map<DurationByLC>(constructionObject.POS.DurationByLC)).Returns(durationByLC);

            var calendarPlan = new CalendarPlan(default, default, default, default, default);
            _mapperMock.Setup(x => x.Map<CalendarPlan>(constructionObject.POS.CalendarPlan)).Returns(calendarPlan);

            var energyAndWater = new EnergyAndWater(default, default, default, default, default, default);
            _mapperMock.Setup(x => x.Map<EnergyAndWater>(constructionObject.POS.EnergyAndWater)).Returns(energyAndWater);

            var durationByLCPath = @"wwwroot\AppData\UserFiles\DurationByLCFiles\DurationByLCBGTGkss.docx";
            _durationByLCServiceMock.Setup(x => x.GetSavePath(windowsName)).Returns(durationByLCPath);
            var calendarPlanPath = @"wwwroot\AppData\UserFiles\CalendarPlanFiles\CalendarPlanBGTGkss.docx";
            _calendarPlanServiceMock.Setup(x => x.GetSavePath(windowsName)).Returns(calendarPlanPath);
            var energyAndWaterPath = @"wwwroot\AppData\UserFiles\EnergyAndWaterFiles\EnergyAndWaterBGTGkss.docx";
            _energyAndWaterServiceMock.Setup(x => x.GetSavePath(windowsName)).Returns(energyAndWaterPath);

            await _projectService.Write(viewModel, windowsName);

            _constructionObjectRepositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(), null,
                It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(), true, false), Times.Once);

            _mapperMock.Verify(x => x.Map<DurationByLC>(constructionObject.POS.DurationByLC), Times.Once);
            _mapperMock.Verify(x => x.Map<CalendarPlan>(constructionObject.POS.CalendarPlan), Times.Once);
            _mapperMock.Verify(x => x.Map<EnergyAndWater>(constructionObject.POS.EnergyAndWater), Times.Once);

            _durationByLCServiceMock.Verify(x => x.Write(durationByLC, windowsName), Times.Once);
            _calendarPlanServiceMock.Verify(x => x.Write(calendarPlan, windowsName), Times.Once);
            _energyAndWaterServiceMock.Verify(x => x.Write(energyAndWater, windowsName), Times.Once);

            _durationByLCServiceMock.Verify(x => x.GetSavePath(windowsName), Times.Once);
            _calendarPlanServiceMock.Verify(x => x.GetSavePath(windowsName), Times.Once);
            _energyAndWaterServiceMock.Verify(x => x.GetSavePath(windowsName), Times.Once);

            _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(3));

            _ecpProjectWriterMock.Verify(
                x => x.Write(viewModel.ObjectCipher, durationByLC, calendarPlan.ConstructionStartDate, durationByLCPath,
                    calendarPlanPath, energyAndWaterPath, templatePath, savePath), Times.Once);
        }

        [Test]
        public void GetSavePath()
        {
            _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("wwwroot");
            var windowsName = "BGTG\\kss";

            var savePath = _projectService.GetSavePath(windowsName);

            _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
            Assert.AreEqual(@"wwwroot\AppData\UserFiles\ProjectFiles\BGTGkss.docx", savePath);
        }
    }
}
