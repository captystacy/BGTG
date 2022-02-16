using BGTG.Data.ModelConfigurations.Base;
using BGTG.Entities.POS.CalendarPlanToolEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGTG.Data.ModelConfigurations.POS.CalendarPlanToolModelConfigurations
{
    public class PreparatoryConstructionMonthModelConfiguration : IdentityModelConfigurationBase<PreparatoryConstructionMonthEntity>
    {
        protected override void AddBuilder(EntityTypeBuilder<PreparatoryConstructionMonthEntity> builder)
        {
            builder.Property(x => x.CreationIndex).IsRequired();
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.InvestmentVolume).HasColumnType("money").IsRequired();
            builder.Property(x => x.PercentPart).HasColumnType("money").IsRequired();
            builder.Property(x => x.VolumeCAIW).HasColumnType("money").IsRequired();

            builder.HasOne(x => x.PreparatoryCalendarWork);
        }

        protected override string TableName()
        {
            return "PreparatoryConstructionMonths";
        }
    }
}
