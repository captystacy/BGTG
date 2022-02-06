using BGTG.Data.ModelConfigurations.Base;
using BGTG.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BGTG.Data.ModelConfigurations
{
    public class ConstructionObjectModelConfiguration : AuditableModelConfigurationBase<ConstructionObjectEntity>
    {
        protected override void AddBuilder(EntityTypeBuilder<ConstructionObjectEntity> builder)
        {
            builder.Property(x => x.Cipher).HasMaxLength(32).IsRequired();
            builder.HasOne(x => x.POS);
        }

        protected override string TableName()
        {
            return "ConstructionObjects";
        }
    }
}
