using Core.Models.Core.Ordering;
using Core.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Ordering
{
    public class OrderDiscountTypeEntityTypeConfiguration : IEntityTypeConfiguration<OrderItemDiscountType>
    {
        public void Configure(EntityTypeBuilder<OrderItemDiscountType> builder)
        {
            builder.ToTable("ItemDiscountTypes", "order");            
            builder.HasKey(x => x.Id);
            builder.Property(o => o.Name).HasMaxLength(200).IsRequired();
            builder.HasData(Enumeration.GetAll<OrderItemDiscountType>());
        }
    }
}
