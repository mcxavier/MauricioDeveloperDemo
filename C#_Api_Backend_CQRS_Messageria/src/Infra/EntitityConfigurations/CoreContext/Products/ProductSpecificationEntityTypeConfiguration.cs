using Core.Models.Core.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Core.Products
{
    public class ProductSpecificationEntityTypeConfiguration : IEntityTypeConfiguration<ProductSpecification>
    {
        public void Configure(EntityTypeBuilder<ProductSpecification> builder)
        {
            builder.ToTable("Specifications", "product");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasIndex(x => x.ProductVariationId).IncludeProperties(p => new { p.TypeId, p.Value });
            builder.HasIndex(x => x.TypeId).IncludeProperties(p => new { p.ProductVariationId });
            builder.Property(x => x.Value).HasMaxLength(25);
            builder.HasIndex(b => b.Value);
            builder.Property(x => x.Description).HasMaxLength(25);
            builder.HasIndex(b => b.Description);
            builder.HasOne(x => x.ProductVariation).WithMany(x => x.Specifications).HasForeignKey(x => x.ProductVariationId);
            builder.HasOne(x => x.Type).WithMany().HasForeignKey(x => x.TypeId);
        }
    }
}