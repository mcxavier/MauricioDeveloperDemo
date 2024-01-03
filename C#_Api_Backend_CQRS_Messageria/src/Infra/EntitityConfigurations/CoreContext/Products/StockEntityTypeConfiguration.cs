using Core.Models.Core.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntitityConfigurations.Core.Products
{
    public class StockEntityTypeConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("Stock", "product");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.StockKeepingUnit).HasMaxLength(40);
            builder.HasIndex(x => x.StockKeepingUnit).IncludeProperties(p => p.Units);
            builder.Property(x => x.StoreCode).HasMaxLength(25);
            builder.HasIndex(x => x.StoreCode);
            builder.HasIndex(x => x.StoreCode).IncludeProperties(p => p.StockKeepingUnit);
            builder.HasAlternateKey(x => new { x.StockKeepingUnit, x.StoreCode }).HasName("UniqueStockRegister");
        }
    }
}