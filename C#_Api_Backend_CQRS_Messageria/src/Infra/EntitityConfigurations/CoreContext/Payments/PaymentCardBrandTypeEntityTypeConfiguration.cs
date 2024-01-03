using Core.Models.Core.Payments;
using Core.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Payments
{
    public class PaymentCardBrandTypeEntityTypeConfiguration : IEntityTypeConfiguration<PaymentCardBrandType>
    {
        public void Configure(EntityTypeBuilder<PaymentCardBrandType> builder)
        {
            builder.ToTable("CardBrandTypes", "payment");
            builder.HasKey(x => x.Id);
            builder.HasData(Enumeration.GetAll<PaymentCardBrandType>());
        }
    }
}