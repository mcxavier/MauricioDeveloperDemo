using Core.Models.Core.Ordering;
using Core.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Ordering
{

    public class OrderTypeEntityTypeConfiguration : IEntityTypeConfiguration<OrderType>
    {

        public void Configure(EntityTypeBuilder<OrderType> builder)
        {
            builder.ToTable("OrderType", "order");
            builder.HasKey(x => x.Id);        
            builder.HasData(Enumeration.GetAll<OrderType>());
        }
    }
}