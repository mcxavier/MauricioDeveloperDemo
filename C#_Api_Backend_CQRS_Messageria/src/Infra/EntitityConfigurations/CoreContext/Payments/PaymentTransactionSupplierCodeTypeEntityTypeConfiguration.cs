using Core.Models.Core.Payments;
using Core.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Payments
{
    public class PaymentTransactionSupplierCodeTypeEntityTypeConfiguration : IEntityTypeConfiguration<PaymentTransactionSupplierCodeType>
    {
        public void Configure(EntityTypeBuilder<PaymentTransactionSupplierCodeType> builder)
        {
            builder.ToTable("SupplierCodeTypes", "payment");
            builder.HasKey(x => x.Id);
            builder.HasData(Enumeration.GetAll<PaymentTransactionSupplierCodeType>());
        }
    }
}