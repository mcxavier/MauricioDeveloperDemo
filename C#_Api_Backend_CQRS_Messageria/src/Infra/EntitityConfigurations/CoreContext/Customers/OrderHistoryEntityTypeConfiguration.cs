using Core.Models.Core.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.CoreContext.Customers
{
    public class OrderHistoryEntityTypeConfiguration : IEntityTypeConfiguration<CustomersOrderHistory>
    {
        public void Configure(EntityTypeBuilder<CustomersOrderHistory> builder)
        {
            builder.ToTable("OrderHistory", "customer");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Variation)
                   .WithMany()
                   .HasForeignKey(history => history.StockKeepingUnit)
                   .HasPrincipalKey(variation => variation.StockKeepingUnit);
        }
    }
}