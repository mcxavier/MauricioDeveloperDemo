using Core.Models.Core.Catalogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Catalogs
{
    public class CatalogEntityTypeConfiguration : IEntityTypeConfiguration<Catalog>
    {
        public void Configure(EntityTypeBuilder<Catalog> builder)
        {
            builder.ToTable("Catalogs", "catalog");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasMany(p => p.Customers).WithOne(b => b.Catalog);
            builder.HasMany(p => p.Products).WithOne(b => b.Catalog);
        }
    }
}