using Core.Models.Identity.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.IdentityContext.Stores
{
    public class StoreGatewaySettingsEntityTypeConfiguration : IEntityTypeConfiguration<StoreGatewaySettings>
    {
        public void Configure(EntityTypeBuilder<StoreGatewaySettings> builder)
        {
            builder.ToTable("GatewaySettings", "store");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Store).WithMany().HasForeignKey(x => x.StoreId);
        }
    }
}