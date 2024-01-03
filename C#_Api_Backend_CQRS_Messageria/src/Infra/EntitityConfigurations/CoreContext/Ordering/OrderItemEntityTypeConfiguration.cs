using Core.Models.Core.Ordering;
using Infra.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Ordering
{
    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("Items", "order");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.UnitPrice).HasPrecision(10, 2);
            builder.Property(p => p.UnitDiscount).HasPrecision(10, 2);
            builder.HasOne(p => p.Order).WithMany(x => x.Items).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(p => p.ProductVariation).WithMany().HasForeignKey(x => x.ProductVariationId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(p => p.DiscountItems).WithOne(x => x.OrderItem).HasForeignKey(x => x.OrderItemId);
        }
    }
}