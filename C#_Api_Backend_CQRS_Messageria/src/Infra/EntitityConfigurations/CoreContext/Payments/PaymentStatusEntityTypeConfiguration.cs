using Core.Models.Core.Payments;
using Core.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Payments
{

    public class PaymentStatusEntityTypeConfiguration : IEntityTypeConfiguration<PaymentStatus>
    {
        public void Configure(EntityTypeBuilder<PaymentStatus> builder)
        {
            builder.ToTable("PaymentStatus", "payment");
            builder.HasKey(x => x.Id);
            builder.HasData(Enumeration.GetAll<PaymentStatus>());
        }
    }
}