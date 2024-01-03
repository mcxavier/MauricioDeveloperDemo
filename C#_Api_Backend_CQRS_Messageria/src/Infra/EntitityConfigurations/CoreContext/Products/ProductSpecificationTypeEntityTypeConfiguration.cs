using Core.Models.Core.Products;
using Core.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Core.Products
{
    public class ProductSpecificationTypeEntityTypeConfiguration : IEntityTypeConfiguration<ProductSpecificationType>
    {
        public void Configure(EntityTypeBuilder<ProductSpecificationType> builder)
        {
            builder.ToTable("SpecificationTypes", "product");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasData(Enumeration.GetAll<ProductSpecificationType>());
        }
    }
}