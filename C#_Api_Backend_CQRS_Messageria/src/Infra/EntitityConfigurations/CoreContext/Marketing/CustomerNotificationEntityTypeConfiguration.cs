using Core.Domains.Marketing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.CoreContext.Marketing
{
    public class CustomerNotificationEntityTypeConfiguration : IEntityTypeConfiguration<CustomerNotification>
    {
        public void Configure(EntityTypeBuilder<CustomerNotification> builder)
        {
            builder.ToTable("CustomerNotification", "marketing");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.StockKeepingUnit).HasMaxLength(40);
            builder.Property(x => x.StoreCode).HasMaxLength(25);
            builder.HasIndex(x => x.StoreCode);
        }
    }
}