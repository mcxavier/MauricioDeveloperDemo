using Core.Models;
using Core.Models.Core.Payments;
using Core.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Payments
{
    public class PaymentTypeEntityTypeConfiguration : IEntityTypeConfiguration<PaymentType>
    {
        public void Configure(EntityTypeBuilder<PaymentType> builder)
        {
            builder.ToTable("PaymenTypes", "payment");
            builder.HasKey(x => x.Id);
            builder.HasData(Enumeration.GetAll<PaymentType>());
        }
    }
}