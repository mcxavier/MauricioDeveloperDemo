using Core.Models.Core.Products;
using Infra.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Core.Products
{
    public class ProductVariationEntityTypeConfiguration : IEntityTypeConfiguration<ProductVariation>
    {
        public void Configure(EntityTypeBuilder<ProductVariation> builder)
        {
            builder.ToTable("Variations", "product");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasIndex(x => x.ProductId).IncludeProperties(p => new { p.BasePrice, p.IsActive });
            builder.HasIndex(x => x.IsActive).IncludeProperties(p => new { p.StockKeepingUnit, p.BasePrice, p.ProductId });
            builder.Property(x => x.BasePrice).HasPrecision(10, 2);
            builder.HasIndex(x => x.BasePrice);
            builder.Property(x => x.CostPrice).HasPrecision(10, 2);
            builder.Property(x => x.ListPrice).HasPrecision(10, 2);
            builder.HasOne(x => x.Product).WithMany(x => x.Variations).HasForeignKey(x => x.ProductId);
            builder.HasMany(x => x.Images).WithOne(x => x.ProductVariation);
            builder.HasMany(x => x.Specifications).WithOne(x => x.ProductVariation);
        }
    }
}