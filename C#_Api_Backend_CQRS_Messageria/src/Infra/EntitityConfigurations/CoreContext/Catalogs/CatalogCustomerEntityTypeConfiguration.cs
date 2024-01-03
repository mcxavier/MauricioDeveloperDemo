using Core.Models.Core.Catalogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.CoreContext.Catalogs
{
    public class CatalogCustomerEntityTypeConfiguration : IEntityTypeConfiguration<CatalogCustomer>
    {
        public void Configure(EntityTypeBuilder<CatalogCustomer> builder)
        {
            builder.ToTable("CatalogCustomer", "catalog");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(p => p.Catalog).WithMany(b => b.Customers).HasForeignKey(x => x.CatalogId);
            builder.HasOne(p => p.Customer).WithMany().HasForeignKey(x => x.CustomerId);
        }
    }
}