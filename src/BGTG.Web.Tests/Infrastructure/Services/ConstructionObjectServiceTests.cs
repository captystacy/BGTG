using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BGTG.Entities;
using BGTG.Entities.POSEntities;
using BGTG.Entities.POSEntities.CalendarPlanToolEntities;
using BGTG.Entities.POSEntities.DurationByLCToolEntities;
using BGTG.Entities.POSEntities.DurationByTCPToolEntities;
using BGTG.Entities.POSEntities.EnergyAndWaterToolEntities;
using BGTG.Web.Infrastructure.Services;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;

namespace BGTG.Web.Tests.Infrastructure.Services;

public class ConstructionObjectServiceTests
{
    private ConstructionObjectService _constructionObjectService = null!;
    private Mock<IUnitOfWork> _unitOfWork = null!;

    [SetUp]
    public void SetUp()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _constructionObjectService = new ConstructionObjectService(_unitOfWork.Object);
    }

    [Test]
    public async Task Update_CalendarPlan_ConstructionObjectIsNull()
    {
        var objectCipher = "5.5-20.548";
        var calendarPlan = new CalendarPlanEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);
        var lastSaveChangesResult = new SaveChangesResult();
        _unitOfWork.Setup(x => x.LastSaveChangesResult).Returns(lastSaveChangesResult);

        var operation = await _constructionObjectService.Update(objectCipher, calendarPlan);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.InsertAsync(
            It.Is<ConstructionObjectEntity>(x =>
                x.Cipher == objectCipher
                && x.CreatedAt == calendarPlan.CreatedAt
                && x.CreatedBy == calendarPlan.CreatedBy
                && x.POS!.CalendarPlan == calendarPlan
            ), default(CancellationToken)), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);

        Assert.IsTrue(operation.Ok);
        Assert.AreEqual(operation.Result, calendarPlan);
    }

    [Test]
    public async Task Update_CalendarPlan_POSIsNull()
    {
        var objectCipher = "5.5-20.548";
        var calendarPlan = new CalendarPlanEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);
        var constructionObject = new ConstructionObjectEntity();
        repositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, calendarPlan);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == calendarPlan.CreatedAt
                && x.UpdatedBy == calendarPlan.CreatedBy
                && x.POS!.CalendarPlan == calendarPlan
            )), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_CalendarPlan_POSIsNotNull_OldCalendarPlanIsNull()
    {
        var objectCipher = "5.5-20.548";
        var calendarPlan = new CalendarPlanEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);
        var constructionObject = new ConstructionObjectEntity
        {
            POS = new POSEntity()
        };
        repositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, calendarPlan);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == calendarPlan.CreatedAt
                && x.UpdatedBy == calendarPlan.CreatedBy
                && x.POS!.CalendarPlan == calendarPlan
            )), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_CalendarPlan_POSIsNotNull_OldCalendarPlanIsNotNull()
    {
        var objectCipher = "5.5-20.548";
        var calendarPlan = new CalendarPlanEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var constructionObjectRepositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(constructionObjectRepositoryMock.Object);
        var calendarPlanRepositoryMock = new Mock<IRepository<CalendarPlanEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<CalendarPlanEntity>(false)).Returns(calendarPlanRepositoryMock.Object);
        var oldCalendarPlan = new CalendarPlanEntity();
        var constructionObject = new ConstructionObjectEntity
        {
            POS = new POSEntity
            {
                CalendarPlan = oldCalendarPlan
            }
        };
        constructionObjectRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, calendarPlan);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        constructionObjectRepositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);

        _unitOfWork.Verify(x => x.GetRepository<CalendarPlanEntity>(false), Times.Once);
        calendarPlanRepositoryMock.Verify(x => x.Delete(oldCalendarPlan), Times.Once);

        constructionObjectRepositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == calendarPlan.CreatedAt
                && x.UpdatedBy == calendarPlan.CreatedBy
                && x.POS!.CalendarPlan == calendarPlan
            )), Times.Once);

        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_DurationByLC_ConstructionObjectIsNull()
    {
        var objectCipher = "5.5-20.548";
        var durationByLC = new DurationByLCEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);

        await _constructionObjectService.Update(objectCipher, durationByLC);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.InsertAsync(
            It.Is<ConstructionObjectEntity>(x =>
                x.Cipher == objectCipher
                && x.CreatedAt == durationByLC.CreatedAt
                && x.CreatedBy == durationByLC.CreatedBy
                && x.POS!.DurationByLC == durationByLC
            ), default(CancellationToken)), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_DurationByLC_POSIsNull()
    {
        var objectCipher = "5.5-20.548";
        var durationByLC = new DurationByLCEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);
        var constructionObject = new ConstructionObjectEntity();
        repositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, durationByLC);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == durationByLC.CreatedAt
                && x.UpdatedBy == durationByLC.CreatedBy
                && x.POS!.DurationByLC == durationByLC
            )), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_DurationByLC_POSIsNotNull_OldDurationByLCIsNull()
    {
        var objectCipher = "5.5-20.548";
        var durationByLC = new DurationByLCEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);
        var constructionObject = new ConstructionObjectEntity
        {
            POS = new POSEntity()
        };
        repositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, durationByLC);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == durationByLC.CreatedAt
                && x.UpdatedBy == durationByLC.CreatedBy
                && x.POS!.DurationByLC == durationByLC
            )), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_DurationByLC_POSIsNotNull_OldDurationByLCIsNotNull()
    {
        var objectCipher = "5.5-20.548";
        var durationByLC = new DurationByLCEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var constructionObjectRepositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(constructionObjectRepositoryMock.Object);
        var durationByLCRepositoryMock = new Mock<IRepository<DurationByLCEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<DurationByLCEntity>(false)).Returns(durationByLCRepositoryMock.Object);
        var oldDurationByLC = new DurationByLCEntity();
        var constructionObject = new ConstructionObjectEntity
        {
            POS = new POSEntity
            {
                DurationByLC = oldDurationByLC
            }
        };
        constructionObjectRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, durationByLC);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        constructionObjectRepositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);

        _unitOfWork.Verify(x => x.GetRepository<DurationByLCEntity>(false), Times.Once);
        durationByLCRepositoryMock.Verify(x => x.Delete(oldDurationByLC), Times.Once);

        constructionObjectRepositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == durationByLC.CreatedAt
                && x.UpdatedBy == durationByLC.CreatedBy
                && x.POS!.DurationByLC == durationByLC
            )), Times.Once);

        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_InterpolationDurationByTCP_ConstructionObjectIsNull()
    {
        var objectCipher = "5.5-20.548";
        var interpolationDurationByTCP = new InterpolationDurationByTCPEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);

        await _constructionObjectService.Update(objectCipher, interpolationDurationByTCP);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.InsertAsync(
            It.Is<ConstructionObjectEntity>(x =>
                x.Cipher == objectCipher
                && x.CreatedAt == interpolationDurationByTCP.CreatedAt
                && x.CreatedBy == interpolationDurationByTCP.CreatedBy
                && x.POS!.InterpolationDurationByTCP == interpolationDurationByTCP
            ), default(CancellationToken)), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_InterpolationDurationByTCP_POSIsNull()
    {
        var objectCipher = "5.5-20.548";
        var interpolationDurationByTCP = new InterpolationDurationByTCPEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);
        var constructionObject = new ConstructionObjectEntity();
        repositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, interpolationDurationByTCP);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == interpolationDurationByTCP.CreatedAt
                && x.UpdatedBy == interpolationDurationByTCP.CreatedBy
                && x.POS!.InterpolationDurationByTCP == interpolationDurationByTCP
            )), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_InterpolationDurationByTCP_POSIsNotNull_OldDurationByTCPsIsNull()
    {
        var objectCipher = "5.5-20.548";
        var interpolationDurationByTCP = new InterpolationDurationByTCPEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);
        var constructionObject = new ConstructionObjectEntity
        {
            POS = new POSEntity()
        };
        repositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, interpolationDurationByTCP);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == interpolationDurationByTCP.CreatedAt
                && x.UpdatedBy == interpolationDurationByTCP.CreatedBy
                && x.POS!.InterpolationDurationByTCP == interpolationDurationByTCP
            )), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_InterpolationDurationByTCP_POSIsNotNull_OldDurationByTCPsIsNotNull()
    {
        var objectCipher = "5.5-20.548";
        var interpolationDurationByTCP = new InterpolationDurationByTCPEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var constructionObjectRepositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(constructionObjectRepositoryMock.Object);
        var interpolationDurationByTCPRepositoryMock = new Mock<IRepository<InterpolationDurationByTCPEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<InterpolationDurationByTCPEntity>(false)).Returns(interpolationDurationByTCPRepositoryMock.Object);
        var extrapolationDurationByTCPRepositoryMock = new Mock<IRepository<ExtrapolationDurationByTCPEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ExtrapolationDurationByTCPEntity>(false)).Returns(extrapolationDurationByTCPRepositoryMock.Object);
        var stepwiseExtrapolationDurationByTCPRepositoryMock = new Mock<IRepository<StepwiseExtrapolationDurationByTCPEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<StepwiseExtrapolationDurationByTCPEntity>(false)).Returns(stepwiseExtrapolationDurationByTCPRepositoryMock.Object);

        var oldInterpolationDurationByTCP = new InterpolationDurationByTCPEntity();
        var oldExtrapolationDurationByTCP = new ExtrapolationDurationByTCPEntity();
        var oldStepwiseExtrapolationDurationByTCP = new StepwiseExtrapolationDurationByTCPEntity();
        var constructionObject = new ConstructionObjectEntity
        {
            POS = new POSEntity
            {
                InterpolationDurationByTCP = oldInterpolationDurationByTCP,
                ExtrapolationDurationByTCP = oldExtrapolationDurationByTCP,
                StepwiseExtrapolationDurationByTCP = oldStepwiseExtrapolationDurationByTCP
            }
        };
        constructionObjectRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, interpolationDurationByTCP);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        constructionObjectRepositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);

        _unitOfWork.Verify(x => x.GetRepository<InterpolationDurationByTCPEntity>(false), Times.Once);
        interpolationDurationByTCPRepositoryMock.Verify(x => x.Delete(oldInterpolationDurationByTCP), Times.Once);
        _unitOfWork.Verify(x => x.GetRepository<ExtrapolationDurationByTCPEntity>(false), Times.Once);
        extrapolationDurationByTCPRepositoryMock.Verify(x => x.Delete(oldExtrapolationDurationByTCP), Times.Once);
        _unitOfWork.Verify(x => x.GetRepository<StepwiseExtrapolationDurationByTCPEntity>(false), Times.Once);
        stepwiseExtrapolationDurationByTCPRepositoryMock.Verify(x => x.Delete(oldStepwiseExtrapolationDurationByTCP), Times.Once);

        constructionObjectRepositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == interpolationDurationByTCP.CreatedAt
                && x.UpdatedBy == interpolationDurationByTCP.CreatedBy
                && x.POS!.InterpolationDurationByTCP == interpolationDurationByTCP
            )), Times.Once);

        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_ExtrapolationDurationByTCP_ConstructionObjectIsNull()
    {
        var objectCipher = "5.5-20.548";
        var extrapolationDurationByTCP = new ExtrapolationDurationByTCPEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);

        await _constructionObjectService.Update(objectCipher, extrapolationDurationByTCP);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.InsertAsync(
            It.Is<ConstructionObjectEntity>(x =>
                x.Cipher == objectCipher
                && x.CreatedAt == extrapolationDurationByTCP.CreatedAt
                && x.CreatedBy == extrapolationDurationByTCP.CreatedBy
                && x.POS!.ExtrapolationDurationByTCP == extrapolationDurationByTCP
            ), default(CancellationToken)), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_ExtrapolationDurationByTCP_POSIsNull()
    {
        var objectCipher = "5.5-20.548";
        var extrapolationDurationByTCP = new ExtrapolationDurationByTCPEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);
        var constructionObject = new ConstructionObjectEntity();
        repositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, extrapolationDurationByTCP);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == extrapolationDurationByTCP.CreatedAt
                && x.UpdatedBy == extrapolationDurationByTCP.CreatedBy
                && x.POS!.ExtrapolationDurationByTCP == extrapolationDurationByTCP
            )), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_ExtrapolationDurationByTCP_POSIsNotNull_OldDurationByTCPsIsNull()
    {
        var objectCipher = "5.5-20.548";
        var extrapolationDurationByTCP = new ExtrapolationDurationByTCPEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);
        var constructionObject = new ConstructionObjectEntity
        {
            POS = new POSEntity()
        };
        repositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, extrapolationDurationByTCP);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == extrapolationDurationByTCP.CreatedAt
                && x.UpdatedBy == extrapolationDurationByTCP.CreatedBy
                && x.POS!.ExtrapolationDurationByTCP == extrapolationDurationByTCP
            )), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_ExtrapolationDurationByTCP_POSIsNotNull_OldDurationByTCPsIsNotNull()
    {
        var objectCipher = "5.5-20.548";
        var extrapolationDurationByTCP = new ExtrapolationDurationByTCPEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var constructionObjectRepositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(constructionObjectRepositoryMock.Object);
        var interpolationDurationByTCPRepositoryMock = new Mock<IRepository<InterpolationDurationByTCPEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<InterpolationDurationByTCPEntity>(false)).Returns(interpolationDurationByTCPRepositoryMock.Object);
        var extrapolationDurationByTCPRepositoryMock = new Mock<IRepository<ExtrapolationDurationByTCPEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ExtrapolationDurationByTCPEntity>(false)).Returns(extrapolationDurationByTCPRepositoryMock.Object);
        var stepwiseExtrapolationDurationByTCPRepositoryMock = new Mock<IRepository<StepwiseExtrapolationDurationByTCPEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<StepwiseExtrapolationDurationByTCPEntity>(false)).Returns(stepwiseExtrapolationDurationByTCPRepositoryMock.Object);

        var oldInterpolationDurationByTCP = new InterpolationDurationByTCPEntity();
        var oldExtrapolationDurationByTCP = new ExtrapolationDurationByTCPEntity();
        var oldStepwiseExtrapolationDurationByTCP = new StepwiseExtrapolationDurationByTCPEntity();
        var constructionObject = new ConstructionObjectEntity
        {
            POS = new POSEntity
            {
                InterpolationDurationByTCP = oldInterpolationDurationByTCP,
                ExtrapolationDurationByTCP = oldExtrapolationDurationByTCP,
                StepwiseExtrapolationDurationByTCP = oldStepwiseExtrapolationDurationByTCP
            }
        };
        constructionObjectRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, extrapolationDurationByTCP);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        constructionObjectRepositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);

        _unitOfWork.Verify(x => x.GetRepository<InterpolationDurationByTCPEntity>(false), Times.Once);
        interpolationDurationByTCPRepositoryMock.Verify(x => x.Delete(oldInterpolationDurationByTCP), Times.Once);
        _unitOfWork.Verify(x => x.GetRepository<ExtrapolationDurationByTCPEntity>(false), Times.Once);
        extrapolationDurationByTCPRepositoryMock.Verify(x => x.Delete(oldExtrapolationDurationByTCP), Times.Once);
        _unitOfWork.Verify(x => x.GetRepository<StepwiseExtrapolationDurationByTCPEntity>(false), Times.Once);
        stepwiseExtrapolationDurationByTCPRepositoryMock.Verify(x => x.Delete(oldStepwiseExtrapolationDurationByTCP), Times.Once);

        constructionObjectRepositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == extrapolationDurationByTCP.CreatedAt
                && x.UpdatedBy == extrapolationDurationByTCP.CreatedBy
                && x.POS!.ExtrapolationDurationByTCP == extrapolationDurationByTCP
            )), Times.Once);

        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_StepwiseExtrapolationDurationByTCP_ConstructionObjectIsNull()
    {
        var objectCipher = "5.5-20.548";
        var stepwiseExtrapolationDurationByTCP = new StepwiseExtrapolationDurationByTCPEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);

        await _constructionObjectService.Update(objectCipher, stepwiseExtrapolationDurationByTCP);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.InsertAsync(
            It.Is<ConstructionObjectEntity>(x =>
                x.Cipher == objectCipher
                && x.CreatedAt == stepwiseExtrapolationDurationByTCP.CreatedAt
                && x.CreatedBy == stepwiseExtrapolationDurationByTCP.CreatedBy
                && x.POS!.StepwiseExtrapolationDurationByTCP == stepwiseExtrapolationDurationByTCP
            ), default(CancellationToken)), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_StepwiseExtrapolationDurationByTCP_POSIsNull()
    {
        var objectCipher = "5.5-20.548";
        var stepwiseExtrapolationDurationByTCP = new StepwiseExtrapolationDurationByTCPEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);
        var constructionObject = new ConstructionObjectEntity();
        repositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, stepwiseExtrapolationDurationByTCP);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == stepwiseExtrapolationDurationByTCP.CreatedAt
                && x.UpdatedBy == stepwiseExtrapolationDurationByTCP.CreatedBy
                && x.POS!.StepwiseExtrapolationDurationByTCP == stepwiseExtrapolationDurationByTCP
            )), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_StepwiseExtrapolationDurationByTCP_POSIsNotNull_OldDurationByTCPsIsNull()
    {
        var objectCipher = "5.5-20.548";
        var stepwiseExtrapolationDurationByTCP = new StepwiseExtrapolationDurationByTCPEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);
        var constructionObject = new ConstructionObjectEntity
        {
            POS = new POSEntity()
        };
        repositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, stepwiseExtrapolationDurationByTCP);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == stepwiseExtrapolationDurationByTCP.CreatedAt
                && x.UpdatedBy == stepwiseExtrapolationDurationByTCP.CreatedBy
                && x.POS!.StepwiseExtrapolationDurationByTCP == stepwiseExtrapolationDurationByTCP
            )), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_StepwiseExtrapolationDurationByTCP_POSIsNotNull_OldDurationByTCPsIsNotNull()
    {
        var objectCipher = "5.5-20.548";
        var stepwiseExtrapolationDurationByTCP = new StepwiseExtrapolationDurationByTCPEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var constructionObjectRepositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(constructionObjectRepositoryMock.Object);
        var interpolationDurationByTCPRepositoryMock = new Mock<IRepository<InterpolationDurationByTCPEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<InterpolationDurationByTCPEntity>(false)).Returns(interpolationDurationByTCPRepositoryMock.Object);
        var extrapolationDurationByTCPRepositoryMock = new Mock<IRepository<ExtrapolationDurationByTCPEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ExtrapolationDurationByTCPEntity>(false)).Returns(extrapolationDurationByTCPRepositoryMock.Object);
        var stepwiseExtrapolationDurationByTCPRepositoryMock = new Mock<IRepository<StepwiseExtrapolationDurationByTCPEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<StepwiseExtrapolationDurationByTCPEntity>(false)).Returns(stepwiseExtrapolationDurationByTCPRepositoryMock.Object);

        var oldInterpolationDurationByTCP = new InterpolationDurationByTCPEntity();
        var oldExtrapolationDurationByTCP = new ExtrapolationDurationByTCPEntity();
        var oldStepwiseExtrapolationDurationByTCP = new StepwiseExtrapolationDurationByTCPEntity();
        var constructionObject = new ConstructionObjectEntity
        {
            POS = new POSEntity
            {
                InterpolationDurationByTCP = oldInterpolationDurationByTCP,
                ExtrapolationDurationByTCP = oldExtrapolationDurationByTCP,
                StepwiseExtrapolationDurationByTCP = oldStepwiseExtrapolationDurationByTCP
            }
        };
        constructionObjectRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, stepwiseExtrapolationDurationByTCP);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        constructionObjectRepositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);

        _unitOfWork.Verify(x => x.GetRepository<InterpolationDurationByTCPEntity>(false), Times.Once);
        interpolationDurationByTCPRepositoryMock.Verify(x => x.Delete(oldInterpolationDurationByTCP), Times.Once);
        _unitOfWork.Verify(x => x.GetRepository<ExtrapolationDurationByTCPEntity>(false), Times.Once);
        extrapolationDurationByTCPRepositoryMock.Verify(x => x.Delete(oldExtrapolationDurationByTCP), Times.Once);
        _unitOfWork.Verify(x => x.GetRepository<StepwiseExtrapolationDurationByTCPEntity>(false), Times.Once);
        stepwiseExtrapolationDurationByTCPRepositoryMock.Verify(x => x.Delete(oldStepwiseExtrapolationDurationByTCP), Times.Once);

        constructionObjectRepositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == stepwiseExtrapolationDurationByTCP.CreatedAt
                && x.UpdatedBy == stepwiseExtrapolationDurationByTCP.CreatedBy
                && x.POS!.StepwiseExtrapolationDurationByTCP == stepwiseExtrapolationDurationByTCP
            )), Times.Once);

        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_EnergyAndWater_ConstructionObjectIsNull()
    {
        var objectCipher = "5.5-20.548";
        var energyAndWater = new EnergyAndWaterEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);

        await _constructionObjectService.Update(objectCipher, energyAndWater);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.InsertAsync(
            It.Is<ConstructionObjectEntity>(x =>
                x.Cipher == objectCipher
                && x.CreatedAt == energyAndWater.CreatedAt
                && x.CreatedBy == energyAndWater.CreatedBy
                && x.POS!.EnergyAndWater == energyAndWater
            ), default(CancellationToken)), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_EnergyAndWater_POSIsNull()
    {
        var objectCipher = "5.5-20.548";
        var energyAndWater = new EnergyAndWaterEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);
        var constructionObject = new ConstructionObjectEntity();
        repositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, energyAndWater);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == energyAndWater.CreatedAt
                && x.UpdatedBy == energyAndWater.CreatedBy
                && x.POS!.EnergyAndWater == energyAndWater
            )), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_EnergyAndWater_POSIsNotNull_OldEnergyAndWaterIsNull()
    {
        var objectCipher = "5.5-20.548";
        var energyAndWater = new EnergyAndWaterEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var repositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(repositoryMock.Object);
        var constructionObject = new ConstructionObjectEntity
        {
            POS = new POSEntity()
        };
        repositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, energyAndWater);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        repositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);
        repositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == energyAndWater.CreatedAt
                && x.UpdatedBy == energyAndWater.CreatedBy
                && x.POS!.EnergyAndWater == energyAndWater
            )), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }

    [Test]
    public async Task Update_EnergyAndWater_POSIsNotNull_OldEnergyAndWaterIsNotNull()
    {
        var objectCipher = "5.5-20.548";
        var energyAndWater = new EnergyAndWaterEntity
        {
            CreatedAt = new DateTime(DateTime.Now.Ticks),
            CreatedBy = "BGTG\\kss"
        };

        var constructionObjectRepositoryMock = new Mock<IRepository<ConstructionObjectEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<ConstructionObjectEntity>(false)).Returns(constructionObjectRepositoryMock.Object);
        var energyAndWaterRepositoryMock = new Mock<IRepository<EnergyAndWaterEntity>>();
        _unitOfWork.Setup(x => x.GetRepository<EnergyAndWaterEntity>(false)).Returns(energyAndWaterRepositoryMock.Object);
        var oldEnergyAndWater = new EnergyAndWaterEntity();
        var constructionObject = new ConstructionObjectEntity
        {
            POS = new POSEntity
            {
                EnergyAndWater = oldEnergyAndWater
            }
        };
        constructionObjectRepositoryMock.Setup(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result).Returns(constructionObject);

        await _constructionObjectService.Update(objectCipher, energyAndWater);

        _unitOfWork.Verify(x => x.GetRepository<ConstructionObjectEntity>(false), Times.Once);
        constructionObjectRepositoryMock.Verify(x => x.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<ConstructionObjectEntity, bool>>>(),
            null,
            It.IsAny<Func<IQueryable<ConstructionObjectEntity>, IIncludableQueryable<ConstructionObjectEntity, object>>>(),
            true,
            false).Result, Times.Once);

        _unitOfWork.Verify(x => x.GetRepository<EnergyAndWaterEntity>(false), Times.Once);
        energyAndWaterRepositoryMock.Verify(x => x.Delete(oldEnergyAndWater), Times.Once);

        constructionObjectRepositoryMock.Verify(x => x.Update(
            It.Is<ConstructionObjectEntity>(x =>
                x.UpdatedAt == energyAndWater.CreatedAt
                && x.UpdatedBy == energyAndWater.CreatedBy
                && x.POS!.EnergyAndWater == energyAndWater
            )), Times.Once);

        _unitOfWork.Verify(x => x.SaveChangesAsync(false), Times.Once);
    }
}