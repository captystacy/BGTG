using BGTG.Data.ModelConfigurations.Base;
using BGTG.Entities.POSEntities.DurationByTCPToolEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGTG.Data.ModelConfigurations.POSModelConfigurations.DurationByTCPToolModelConfigurations
{
    public class InterpolationPipelineStandardModelConfiguration : IdentityModelConfigurationBase<InterpolationPipelineStandardEntity>
    {
        protected override void AddBuilder(EntityTypeBuilder<InterpolationPipelineStandardEntity> builder)
        {
            builder.Property(x => x.Duration).HasColumnType("money").IsRequired();
            builder.Property(x => x.PipelineLength).HasColumnType("money").IsRequired();
            builder.Property(x => x.PreparatoryPeriod).HasColumnType("money").IsRequired();
            builder.Property(x => x.InterpolationDurationByTCPId).IsRequired();

            builder.HasOne(x => x.InterpolationDurationByTCP);
        }

        protected override string TableName()
        {
            return "InterpolationPipelineStandards";
        }
    }
}
