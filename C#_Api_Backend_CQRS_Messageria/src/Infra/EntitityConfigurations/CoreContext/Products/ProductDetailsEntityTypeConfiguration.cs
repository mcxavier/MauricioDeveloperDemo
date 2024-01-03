using Core.Models.Core.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Core.Products
{
    public class ProductDetailsEntityTypeConfiguration : IEntityTypeConfiguration<ProductDetails>
    {
        public void Configure(EntityTypeBuilder<ProductDetails> builder)
        {
            builder.ToTable("ProductDetails", "product");
            builder.HasKey(x => x.Id);
        }
    }
}