using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.BGTG;
using BGTG.Entities.POS;
using BGTG.Entities.POS.CalendarPlanToolEntities;
using BGTG.Entities.POS.DurationByLCToolEntities;
using BGTG.Entities.POS.EnergyAndWaterToolEntities;
using BGTG.POS;
using BGTG.POS.CalendarPlanTool;
using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.POS.EnergyAndWaterTool;
using BGTG.POS.ProjectTool;
using BGTG.POS.ProjectTool.Base;
using BGTG.Web.Infrastructure.Helpers;
using BGTG.Web.Infrastructure.Services.POSServices;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels;
using Calabonga.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace BGTG.Web.Tests.Infrastructure.Services.POS;

public class ProjectServiceTests
{
    private ProjectService _projectService = null!;
    private Mock<IECPProjectWriter> _ecpProjectWriterMock = null!;
    private Mock<IDurationByLCService> _durationByLCServiceMock = null!;
    private Mock<ICalendarPlanService> _calendarPlanServiceMock = null!;
    private Mock<IEnergyAndWaterService> _energyAndWaterServiceMock = null!;
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private Mock<IMapper> _mapperMock = null!;
    private Mock<IWebHostEnvironment> _webHostEnvironmentMock = null!;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock = null!;

    [SetUp]
    public void SetUp()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        IdentityHelper.Instance.Configure(_httpContextAccessorMock.Object);

        _ecpProjectWriterMock = new Mock<IECPProjectWriter>();
        _durationByLCServiceMock = new Mock<IDurationByLCService>();
        _calendarPlanServiceMock = new Mock<ICalendarPlanService>();
        _energyAndWaterServiceMock = new Mock<IEnergyAndWaterService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _projectService = new ProjectService(_ecpProjectWriterMock.Object, _durationByLCServiceMock.Object,
            _calendarPlanServiceMock.Object, _energyAndWaterServiceMock.Object,
            _unitOfWorkMock.Object, _mapperMock.Object, _webHostEnvironmentMock.Object);
    }

    [Test]
    public async Task Write()
    {
        var viewModel = new ProjectViewModel
        {
            ObjectCipher = "5.5-20.548",
            ChiefProjectEngineer = ChiefProjectEngineer.Saiko,
            ProjectTemplate = ProjectTemplate.ECP,
            HouseholdTownIncluded = true,
        };

        var templatePath = @"root\AppData\Templates\POSTemplates\ProjectTemplates\ECP\Saiko\Unknown\Employees4\HouseholdTown+.docx";
        var savePath = @"root\AppData\UserFiles\POSFiles\ProjectFiles\BGTGkss.docx";

        var constructionObject = new ConstructionObjectEntity
        {
            POS = new POSEntity
            {
                DurationByLC = new DurationByLCEntity { NumberOfEmployees = 4 },
                CalendarPlan = new CalendarPlanEntity { ConstructionStartDate = new DateTime(DateTime.Now.Ticks) },
                EnergyAndWater = new EnergyAndWaterEntity(),
            }
        };

        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWorkMock.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);

        repositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
                null,
                It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
                true,
                false)
            .Result).Returns(constructionObject);

        var durationByLC = new DurationByLC(default, default, default, default, default, default, default, 4, default, default, default, default, default, default);
        _mapperMock.Setup(x => x.Map<DurationByLC>(constructionObject.POS.DurationByLC)).Returns(durationByLC);

        var calendarPlan = new CalendarPlan(default!, default!, default, default, default);
        _mapperMock.Setup(x => x.Map<CalendarPlan>(constructionObject.POS.CalendarPlan)).Returns(calendarPlan);

        var energyAndWater = new EnergyAndWater(default, default, default, default, default, default);
        _mapperMock.Setup(x => x.Map<EnergyAndWater>(constructionObject.POS.EnergyAndWater)).Returns(energyAndWater);

        var durationByLCPath = @"root\AppData\UserFiles\POSFiles\DurationByLCFiles\DurationByLCBGTGkss.docx";
        _durationByLCServiceMock.Setup(x => x.GetSavePath()).Returns(durationByLCPath);
        var calendarPlanPath = @"root\AppData\UserFiles\POSFiles\CalendarPlanFiles\CalendarPlanBGTGkss.docx";
        _calendarPlanServiceMock.Setup(x => x.GetSavePath()).Returns(calendarPlanPath);
        var energyAndWaterPath = @"root\AppData\UserFiles\POSFiles\EnergyAndWaterFiles\EnergyAndWaterBGTGkss.docx";
        _energyAndWaterServiceMock.Setup(x => x.GetSavePath()).Returns(energyAndWaterPath);

        await _projectService.Write(viewModel);

        _unitOfWorkMock.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false), Times.Once);

        _mapperMock.Verify(x => x.Map<DurationByLC>(constructionObject.POS.DurationByLC), Times.Once);
        _mapperMock.Verify(x => x.Map<CalendarPlan>(constructionObject.POS.CalendarPlan), Times.Once);
        _mapperMock.Verify(x => x.Map<EnergyAndWater>(constructionObject.POS.EnergyAndWater), Times.Once);

        _durationByLCServiceMock.Verify(x => x.Write(durationByLC), Times.Once);
        _calendarPlanServiceMock.Verify(x => x.Write(calendarPlan), Times.Once);
        _energyAndWaterServiceMock.Verify(x => x.Write(energyAndWater), Times.Once);

        _durationByLCServiceMock.Verify(x => x.GetSavePath(), Times.Once);
        _calendarPlanServiceMock.Verify(x => x.GetSavePath(), Times.Once);
        _energyAndWaterServiceMock.Verify(x => x.GetSavePath(), Times.Once);

        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Exactly(3));

        _ecpProjectWriterMock.Verify(
            x => x.Write(viewModel.ObjectCipher, durationByLC, calendarPlan.ConstructionStartDate, durationByLCPath,
                calendarPlanPath, energyAndWaterPath, templatePath, savePath), Times.Once);
    }

    [Test]
    public void GetSavePath()
    {
        _webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns("root");
        IdentityFake.Setup(_httpContextAccessorMock, "BGTG\\kss");

        var savePath = _projectService.GetSavePath();

        _webHostEnvironmentMock.VerifyGet(x => x.ContentRootPath, Times.Once);
        Assert.AreEqual(@"root\AppData\UserFiles\POSFiles\ProjectFiles\BGTGkss.docx", savePath);
    }
}