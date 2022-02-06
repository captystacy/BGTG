using BGTG.Data.Base;
using BGTG.Entities;
using BGTG.Entities.POSEntities;
using BGTG.Entities.POSEntities.CalendarPlanToolEntities;
using BGTG.Entities.POSEntities.DurationByLCToolEntities;
using BGTG.Entities.POSEntities.DurationByTCPToolEntities;
using BGTG.Entities.POSEntities.EnergyAndWaterToolEntities;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Data
{
    public class ApplicationDbContext : DbContextBase<ApplicationDbContext>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ConstructionObjectEntity> ConstructionObjects { get; set; }

        public DbSet<POSEntity> POSes { get; set; }

        #region Calendar plan tool

        public DbSet<CalendarPlanEntity> CalendarPlans { get; set; }

        public DbSet<PreparatoryCalendarWorkEntity> PreparatoryCalendarWorks { get; set; }

        public DbSet<MainCalendarWorkEntity> MainCalendarWorks { get; set; }

        public DbSet<PreparatoryConstructionMonthEntity> PreparatoryConstructionMonths { get; set; }

        public DbSet<MainConstructionMonthEntity> MainConstructionMonths { get; set; }

        #endregion

        #region Duration by labor costs tool

        public DbSet<DurationByLCEntity> DurationByLCs { get; set; }

        #endregion

        #region Duration by TCP tool

        public DbSet<ExtrapolationDurationByTCPEntity> ExtrapolationDurationByTCPs { get; set; }

        public DbSet<ExtrapolationPipelineStandardEntity> ExtrapolationPipelineStandards { get; set; }

        public DbSet<InterpolationDurationByTCPEntity> InterpolationDurationByTCPs { get; set; }

        public DbSet<InterpolationPipelineStandardEntity> InterpolationPipelineStandards { get; set; }

        public DbSet<StepwiseExtrapolationDurationByTCPEntity> StepwiseExtrapolationDurationByTCPs { get; set; }

        public DbSet<StepwiseExtrapolationPipelineStandardEntity> StepwiseExtrapolationPipelineStandards { get; set; }

        public DbSet<StepwisePipelineStandardEntity> StepwisePipelineStandards { get; set; }

        #endregion

        #region Energy and water tool

        public DbSet<EnergyAndWaterEntity> EnergyAndWaters { get; set; }

        #endregion
    }
}