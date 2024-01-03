using Core.Models.Core.Ordering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Ordering
{
    public class OrderItemDiscountEntityTypeConfiguration : IEntityTypeConfiguration<OrderItemDiscount>
    {
        public void Configure(EntityTypeBuilder<OrderItemDiscount> builder)
        {
            builder.ToTable("ItemDiscounts", "order");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(p => p.DiscountType).WithMany().HasForeignKey(x => x.DiscountTypeId);
            builder.HasOne(p => p.OrderItem).WithMany().HasForeignKey(x => x.OrderItemId);
        }
    }
}
