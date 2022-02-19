using BGTG.Data.ModelConfigurations.Base;
using BGTG.Entities.POSEntities.CalendarPlanToolEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGTG.Data.ModelConfigurations.POSModelConfigurations.CalendarPlanToolModelConfigurations
{
    public class MainCalendarWorkModelConfiguration : IdentityModelConfigurationBase<MainCalendarWorkEntity>
    {
        protected override void AddBuilder(EntityTypeBuilder<MainCalendarWorkEntity> builder)
        {
            builder.Property(x => x.EstimateChapter).IsRequired();
            builder.Property(x => x.TotalCost).HasColumnType("money").IsRequired();
            builder.Property(x => x.TotalCostIncludingCAIW).HasColumnType("money").IsRequired();
            builder.Property(x => x.WorkName).HasMaxLength(128).IsRequired();
            builder.Property(x => x.CalendarPlanId).IsRequired();

            builder.HasOne(x => x.CalendarPlan);
            builder.HasMany(x => x.ConstructionMonths);
        }

        protected override string TableName()
        {
            return "MainCalendarWorks";
        }
    }
}
