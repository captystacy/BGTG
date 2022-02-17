using BGTG.Data.ModelConfigurations.Base;
using BGTG.Entities.BGTG;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGTG.Data.ModelConfigurations.BGTG
{
    public class ConstructionObjectModelConfiguration : AuditableModelConfigurationBase<ConstructionObjectEntity>
    {
        protected override void AddBuilder(EntityTypeBuilder<ConstructionObjectEntity> builder)
        {
            builder.Property(x => x.Cipher).HasMaxLength(32).IsRequired();

            builder.HasIndex(x => x.Cipher);

            builder.HasOne(x => x.POS);
        }

        protected override string TableName()
        {
            return "ConstructionObjects";
        }
    }
}
