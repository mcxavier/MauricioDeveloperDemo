using Core.Models;
using Core.Models.Core.Ordering;
using Core.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Ordering
{
    public class OrderStatusEntityTypeConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.ToTable("Status", "order");
            builder.HasKey(x => x.Id);
            builder.HasData(Enumeration.GetAll<OrderStatus>());
        }
    }
}