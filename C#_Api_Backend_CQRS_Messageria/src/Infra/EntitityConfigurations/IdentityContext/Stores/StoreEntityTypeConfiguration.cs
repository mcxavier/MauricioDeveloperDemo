using Core.Models.Identity.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.IdentityContext.Stores
{
    public class StoreEntityTypeConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.ToTable("Stores", "store");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId);
            builder.HasOne(x => x.Subsidiary).WithMany().HasForeignKey(x => x.SubsidiaryId).IsRequired(false);
        }
    }
}