using Core.Domains.Ordering.Models;
using Core.Models.Core.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.CoreContext.Ordering
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders", "order");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(p => p.Customer).WithMany().HasForeignKey(x => x.CustomerId).IsRequired(false);
            builder.HasOne(p => p.Catalog).WithMany().HasForeignKey(x => x.CatalogId).IsRequired(false);
            builder.HasOne(p => p.Seller).WithMany().HasForeignKey(x => x.SellerId).IsRequired(false);
            builder.HasOne(p => p.Shipping).WithMany().HasForeignKey(x => x.ShippingId).IsRequired(false);
            builder.HasOne(p => p.Status).WithMany().HasForeignKey(x => x.StatusId);
            builder.HasMany(p => p.Items).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);
            builder.HasOne(p => p.Payment).WithOne(c => c.Order).HasForeignKey<Payment>(x => x.OrderId);
            builder.HasOne(p => p.OrderType).WithMany().HasForeignKey(x => x.OrderTypeId);
        }
    }
}