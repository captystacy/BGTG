using BGTG.Data.ModelConfigurations.Base;
using BGTG.Entities.POSEntities.EnergyAndWaterToolEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGTG.Data.ModelConfigurations.POSModelConfigurations.EnergyAndWaterToolModelConfigurations
{
    public class EnergyAndWaterModelConfiguration : AuditableModelConfigurationBase<EnergyAndWaterEntity>
    {
        protected override void AddBuilder(EntityTypeBuilder<EnergyAndWaterEntity> builder)
        {
            builder.Property(x => x.ConstructionYear).IsRequired();
            builder.Property(x => x.VolumeCAIW).HasColumnType("money").IsRequired();
            builder.Property(x => x.Energy).HasColumnType("money").IsRequired();
            builder.Property(x => x.Water).HasColumnType("money").IsRequired();
            builder.Property(x => x.CompressedAir).HasColumnType("money").IsRequired();
            builder.Property(x => x.Oxygen).HasColumnType("money").IsRequired();
            builder.Property(x => x.POSId).IsRequired();

            builder.HasOne(x => x.POS);
        }

        protected override string TableName()
        {
            return "EnergyAndWaters";
        }
    }
}
