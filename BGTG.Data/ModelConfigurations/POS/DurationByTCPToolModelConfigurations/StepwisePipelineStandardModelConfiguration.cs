using BGTG.Data.ModelConfigurations.Base;
using BGTG.Entities.POS.DurationByTCPToolEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGTG.Data.ModelConfigurations.POS.DurationByTCPToolModelConfigurations
{
    public class StepwisePipelineStandardModelConfiguration : IdentityModelConfigurationBase<StepwisePipelineStandardEntity>
    {
        protected override void AddBuilder(EntityTypeBuilder<StepwisePipelineStandardEntity> builder)
        {
            builder.Property(x => x.Duration).HasColumnType("money").IsRequired();
            builder.Property(x => x.PipelineLength).HasColumnType("money").IsRequired();
            builder.Property(x => x.PreparatoryPeriod).HasColumnType("money").IsRequired();
            builder.Property(x => x.StepwiseExtrapolationDurationByTCPId).IsRequired();

            builder.HasOne(x => x.StepwiseExtrapolationDurationByTCP);
        }

        protected override string TableName()
        {
            return "StepwisePipelineStandards";
        }
    }
}
