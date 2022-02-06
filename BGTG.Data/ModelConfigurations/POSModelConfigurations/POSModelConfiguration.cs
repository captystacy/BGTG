using BGTG.Data.ModelConfigurations.Base;
using BGTG.Entities.POSEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGTG.Data.ModelConfigurations.POSModelConfigurations
{
    public class POSModelConfiguration : IdentityModelConfigurationBase<POSEntity>
    {
        protected override void AddBuilder(EntityTypeBuilder<POSEntity> builder)
        {
            builder.Property(x => x.ConstructionObjectId).IsRequired();

            builder.HasOne(x => x.ConstructionObject);
            builder.HasOne(x => x.CalendarPlan);
            builder.HasOne(x => x.EnergyAndWater);
            builder.HasOne(x => x.DurationByLC);
            builder.HasOne(x => x.InterpolationDurationByTCP);
            builder.HasOne(x => x.ExtrapolationDurationByTCP);
            builder.HasOne(x => x.StepwiseExtrapolationDurationByTCP);
        }

        protected override string TableName()
        {
            return "POSes";
        }
    }
}
