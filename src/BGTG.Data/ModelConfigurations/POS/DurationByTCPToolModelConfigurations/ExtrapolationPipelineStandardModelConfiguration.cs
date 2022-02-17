using BGTG.Data.ModelConfigurations.Base;
using BGTG.Entities.POS.DurationByTCPToolEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGTG.Data.ModelConfigurations.POS.DurationByTCPToolModelConfigurations
{
    public class ExtrapolationPipelineStandardModelConfiguration : IdentityModelConfigurationBase<ExtrapolationPipelineStandardEntity>
    {
        protected override void AddBuilder(EntityTypeBuilder<ExtrapolationPipelineStandardEntity> builder)
        {
            builder.Property(x => x.Duration).HasColumnType("money").IsRequired();
            builder.Property(x => x.PipelineLength).HasColumnType("money").IsRequired();
            builder.Property(x => x.PreparatoryPeriod).HasColumnType("money").IsRequired();
            builder.Property(x => x.ExtrapolationDurationByTCPId).IsRequired();

            builder.HasOne(x => x.ExtrapolationDurationByTCP);
        }

        protected override string TableName()
        {
            return "ExtrapolationPipelineStandards";
        }
    }
}
