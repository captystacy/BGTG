using BGTG.Data.ModelConfigurations.Base;
using BGTG.Entities.POSEntities.CalendarPlanToolEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGTG.Data.ModelConfigurations.POSModelConfigurations.CalendarPlanToolModelConfigurations
{
    public class CalendarPlanModelConfiguration : AuditableModelConfigurationBase<CalendarPlanEntity>
    {
        protected override void AddBuilder(EntityTypeBuilder<CalendarPlanEntity> builder)
        {
            builder.Property(x => x.ConstructionDuration).HasColumnType("money").IsRequired();
            builder.Property(x => x.ConstructionDurationCeiling).IsRequired();
            builder.Property(x => x.ConstructionStartDate).IsRequired();
            builder.Property(x => x.POSId).IsRequired();

            builder.HasMany(x => x.MainCalendarWorks);
            builder.HasMany(x => x.PreparatoryCalendarWorks);
            builder.HasOne(x => x.POS);
        }

        protected override string TableName()
        {
            return "CalendarPlans";
        }
    }
}
