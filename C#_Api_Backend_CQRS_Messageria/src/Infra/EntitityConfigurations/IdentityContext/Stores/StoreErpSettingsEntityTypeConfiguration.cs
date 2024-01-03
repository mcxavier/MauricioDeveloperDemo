using Core.Models.Identity.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.IdentityContext.Stores
{
    public class StoreErpSettingsEntityTypeConfiguration : IEntityTypeConfiguration<StoreErpSettings>
    {
        public void Configure(EntityTypeBuilder<StoreErpSettings> builder)
        {
            builder.ToTable("ErpSettings", "store");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Store).WithMany().HasForeignKey(x => x.StoreId);
        }
    }
}