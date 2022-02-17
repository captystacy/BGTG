using BGTG.Data.ModelConfigurations.Base;
using BGTG.Entities.POS.DurationByTCPToolEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGTG.Data.ModelConfigurations.POS.DurationByTCPToolModelConfigurations
{
    public class InterpolationDurationByTCPModelConfiguration : AuditableModelConfigurationBase<InterpolationDurationByTCPEntity>
    {
        protected override void AddBuilder(EntityTypeBuilder<InterpolationDurationByTCPEntity> builder)
        {
            builder.Property(x => x.PipelineMaterial).HasMaxLength(64).IsRequired();
            builder.Property(x => x.PipelineDiameter).IsRequired();
            builder.Property(x => x.PipelineDiameterPresentation).HasMaxLength(16).IsRequired();
            builder.Property(x => x.PipelineLength).HasColumnType("money").IsRequired();
            builder.Property(x => x.DurationCalculationType).IsRequired();
            builder.Property(x => x.Duration).HasColumnType("money").IsRequired();
            builder.Property(x => x.RoundedDuration).HasColumnType("money").IsRequired();
            builder.Property(x => x.PreparatoryPeriod).HasColumnType("money").IsRequired();
            builder.Property(x => x.AppendixKey).IsRequired();
            builder.Property(x => x.AppendixPage).IsRequired();
            builder.Property(x => x.VolumeChange).HasColumnType("money").IsRequired();
            builder.Property(x => x.DurationChange).HasColumnType("money").IsRequired();
            builder.Property(x => x.POSId).IsRequired();

            builder.HasMany(x => x.CalculationPipelineStandards);
            builder.HasOne(x => x.POS);
        }

        protected override string TableName()
        {
            return "InterpolationDurationByTCPs";
        }
    }
}
