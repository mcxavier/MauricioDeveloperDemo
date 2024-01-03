using Core.Models.Core.Catalogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Catalogs
{
    public class CatalogProductEntityTypeConfiguration : IEntityTypeConfiguration<CatalogProduct>
    {
        public void Configure(EntityTypeBuilder<CatalogProduct> builder)
        {
            builder.ToTable("CatalogProduct", "catalog");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(p => p.Catalog).WithMany().HasForeignKey(x => x.CatalogId);
            builder.HasOne(p => p.Product).WithMany().HasForeignKey(x => x.ProductId);
        }
    }
}