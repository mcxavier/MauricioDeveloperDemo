using Core.Models.Core.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Payments
{
    public class PaymentTransactionEntityTypeConfiguration : IEntityTypeConfiguration<PaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
        {
            builder.ToTable("Transactions", "payment");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(p => p.Status).WithMany().HasForeignKey(x => x.StatusId);
            builder.HasOne(p => p.Payment).WithMany(x => x.Transactions).HasForeignKey(x => x.PaymentId);
        }
    }
}