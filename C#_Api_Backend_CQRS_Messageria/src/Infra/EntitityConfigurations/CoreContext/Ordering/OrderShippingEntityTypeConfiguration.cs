using Core.Models.Core.Ordering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Ordering
{
    public class OrderShippingEntityTypeConfiguration : IEntityTypeConfiguration<OrderShipping>
    {
        public void Configure(EntityTypeBuilder<OrderShipping> builder)
        {
            builder.ToTable("Shippings", "order");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Address).WithMany().HasForeignKey(x => x.AddressId);
        }
    }
}