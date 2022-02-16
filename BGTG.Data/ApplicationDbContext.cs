using BGTG.Data.Base;
using BGTG.Entities.BGTG;
using BGTG.Entities.POS;
using BGTG.Entities.POS.CalendarPlanToolEntities;
using BGTG.Entities.POS.DurationByLCToolEntities;
using BGTG.Entities.POS.DurationByTCPToolEntities;
using BGTG.Entities.POS.EnergyAndWaterToolEntities;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Data
{
    /// <summary>
    /// Database for application
    /// </summary>
    public class ApplicationDbContext : DbContextBase<ApplicationDbContext>, IApplicationDbContext
    {
        /// <inheritdoc />
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ConstructionObjectEntity> ConstructionObjects { get; set; } = null!;

        public DbSet<POSEntity> POSes { get; set; } = null!;

        #region Calendar plan tool

        public DbSet<CalendarPlanEntity> CalendarPlans { get; set; } = null!;

        public DbSet<PreparatoryCalendarWorkEntity> PreparatoryCalendarWorks { get; set; } = null!;

        public DbSet<MainCalendarWorkEntity> MainCalendarWorks { get; set; } = null!;

        public DbSet<PreparatoryConstructionMonthEntity> PreparatoryConstructionMonths { get; set; } = null!;

        public DbSet<MainConstructionMonthEntity> MainConstructionMonths { get; set; } = null!;

        #endregion

        #region Duration by labor costs tool

        public DbSet<DurationByLCEntity> DurationByLCs { get; set; } = null!;

        #endregion

        #region Duration by TCP tool

        public DbSet<ExtrapolationDurationByTCPEntity> ExtrapolationDurationByTCPs { get; set; } = null!;

        public DbSet<ExtrapolationPipelineStandardEntity> ExtrapolationPipelineStandards { get; set; } = null!;

        public DbSet<InterpolationDurationByTCPEntity> InterpolationDurationByTCPs { get; set; } = null!;

        public DbSet<InterpolationPipelineStandardEntity> InterpolationPipelineStandards { get; set; } = null!;

        public DbSet<StepwiseExtrapolationDurationByTCPEntity> StepwiseExtrapolationDurationByTCPs { get; set; } = null!;

        public DbSet<StepwiseExtrapolationPipelineStandardEntity> StepwiseExtrapolationPipelineStandards { get; set; } = null!;

        public DbSet<StepwisePipelineStandardEntity> StepwisePipelineStandards { get; set; } = null!;

        #endregion

        #region Energy and water tool

        public DbSet<EnergyAndWaterEntity> EnergyAndWaters { get; set; } = null!;

        #endregion
    }
}