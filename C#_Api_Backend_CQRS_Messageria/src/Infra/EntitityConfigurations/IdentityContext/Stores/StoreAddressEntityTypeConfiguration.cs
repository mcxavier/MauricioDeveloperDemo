using Core.Models.Identity.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.IdentityContext.Stores
{
    public class StoreAddressEntityTypeConfiguration : IEntityTypeConfiguration<StoreAddress>
    {
        public void Configure(EntityTypeBuilder<StoreAddress> builder)
        {
            builder.ToTable("StoreAddress", "store");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Store).WithMany().HasForeignKey(x => x.StoreId);
        }
    }
}