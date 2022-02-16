using BGTG.Data.ModelConfigurations.Base;
using BGTG.Entities.POS.DurationByLCToolEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGTG.Data.ModelConfigurations.POS.DurationByLCToolModelConfigurations
{
    public class DurationByLCModelConfiguration : AuditableModelConfigurationBase<DurationByLCEntity>
    {
        protected override void AddBuilder(EntityTypeBuilder<DurationByLCEntity> builder)
        {
            builder.Property(x => x.Duration).HasColumnType("money").IsRequired();
            builder.Property(x => x.TotalLaborCosts).HasColumnType("money").IsRequired();
            builder.Property(x => x.EstimateLaborCosts).HasColumnType("money").IsRequired();
            builder.Property(x => x.TechnologicalLaborCosts).HasColumnType("money").IsRequired();
            builder.Property(x => x.WorkingDayDuration).HasColumnType("money").IsRequired();
            builder.Property(x => x.Shift).HasColumnType("money").IsRequired();
            builder.Property(x => x.NumberOfWorkingDays).HasColumnType("money").IsRequired();
            builder.Property(x => x.NumberOfEmployees).IsRequired();
            builder.Property(x => x.RoundedDuration).HasColumnType("money").IsRequired();
            builder.Property(x => x.TotalDuration).HasColumnType("money").IsRequired();
            builder.Property(x => x.PreparatoryPeriod).HasColumnType("money").IsRequired();
            builder.Property(x => x.AcceptanceTime).HasColumnType("money").IsRequired();
            builder.Property(x => x.AcceptanceTimeIncluded).IsRequired();
            builder.Property(x => x.RoundingIncluded).IsRequired();
            builder.Property(x => x.POSId).IsRequired();

            builder.HasOne(x => x.POS);
        }

        protected override string TableName()
        {
            return "DurationByLCs";
        }
    }
}
