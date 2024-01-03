using Core.Domains.Ordering.Models;
using Core.Models.Core.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Payments
{
    public class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments", "payment");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();            
            builder.HasOne(p => p.PaymentType).WithMany().HasForeignKey(x => x.PaymentTypeId);
            builder.HasOne(p => p.Order).WithOne(c => c.Payment).HasForeignKey<Order>(x => x.PaymentId);            
            builder.HasOne(p => p.Status).WithMany().HasForeignKey(x => x.StatusId);
            builder.HasOne(p => p.CardBrandType).WithMany().HasForeignKey(x => x.CardBrandTypeId);
            builder.HasMany(p => p.Transactions).WithOne(x => x.Payment).HasForeignKey(x => x.PaymentId);
        }
    }
}