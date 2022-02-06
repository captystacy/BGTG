using BGTG.Data.ModelConfigurations.Base;
using BGTG.Entities.POSEntities.DurationByTCPToolEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGTG.Data.ModelConfigurations.POSModelConfigurations.DurationByTCPToolModelConfigurations
{
    public class StepwiseExtrapolationPipelineStandardModelConfiguration : IdentityModelConfigurationBase<StepwiseExtrapolationPipelineStandardEntity>
    {
        protected override void AddBuilder(EntityTypeBuilder<StepwiseExtrapolationPipelineStandardEntity> builder)
        {
            builder.Property(x => x.Duration).HasColumnType("money").IsRequired();
            builder.Property(x => x.PipelineLength).HasColumnType("money").IsRequired();
            builder.Property(x => x.PreparatoryPeriod).HasColumnType("money").IsRequired();
            builder.Property(x => x.StepwiseExtrapolationDurationByTCPId).IsRequired();

            builder.HasOne(x => x.StepwiseExtrapolationDurationByTCP);
        }

        protected override string TableName()
        {
            return "StepwiseExtrapolationPipelineStandards";
        }
    }
}
