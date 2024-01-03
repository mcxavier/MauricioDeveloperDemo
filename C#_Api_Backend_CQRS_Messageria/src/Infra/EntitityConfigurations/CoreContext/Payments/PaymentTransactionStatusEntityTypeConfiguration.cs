using Core.Models.Core.Payments;
using Core.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Payments
{
    public class PaymentTransactionStatusEntityTypeConfiguration : IEntityTypeConfiguration<PaymentTransactionStatus>
    {
        public void Configure(EntityTypeBuilder<PaymentTransactionStatus> builder)
        {
            builder.ToTable("TransactionStatus", "payment");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasData(Enumeration.GetAll<PaymentTransactionStatus>());
        }
    }
}