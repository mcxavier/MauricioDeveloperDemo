using Core.Domains.Log;
using Core.Domains.Insights;
using Core.Domains.Marketing.Models;
using Core.Domains.Ordering.Models;
using Core.Models.Core.Catalogs;
using Core.Models.Core.Customers;
using Core.Models.Core.Geography;
using Core.Models.Core.Ordering;
using Core.Models.Core.Payments;
using Core.Models.Core.Products;
using Infra.EntitityConfigurations.Catalogs;
using Infra.EntitityConfigurations.Core.Products;
using Infra.EntitityConfigurations.CoreContext.Catalogs;
using Infra.EntitityConfigurations.CoreContext.Customers;
using Infra.EntitityConfigurations.CoreContext.Log;
using Infra.EntitityConfigurations.CoreContext.Marketing;
using Infra.EntitityConfigurations.CoreContext.Ordering;
using Infra.EntitityConfigurations.CoreContext.Products;
using Infra.EntitityConfigurations.Customers;
using Infra.EntitityConfigurations.Geography;
using Infra.EntitityConfigurations.Messages;
using Infra.EntitityConfigurations.Ordering;
using Infra.EntitityConfigurations.Payments;
using Infra.ExternalServices.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Infra.EntitityConfigurations.Contexts
{
    public class CoreContext : DataContext
    {
        public CoreContext(DbContextOptions options, SmartSalesIdentity identity, ILogger<CoreContext> logger) : base(options, identity, logger)
        {
        }

        // entity configurations
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomersOrderHistory> OrderHistories { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<CatalogProduct> CatalogProducts { get; set; }
        public DbSet<CatalogCustomer> CatalogCustomers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderType> OrderType { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<OrderShipping> OrderShippings { get; set; }
        public DbSet<OrderShipping> OrderTypeEntityTypeConfiguration { get; set; }
        public DbSet<OrderItemDiscount> OrderItemDiscounts { get; set; }
        public DbSet<OrderItemDiscountType> DiscountTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<PaymentCardBrandType> CardBrandTypes { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }
        public DbSet<PaymentTransaction> PaymentTransaction { get; set; }
        public DbSet<PaymentTransactionStatus> PaymentTransactionStatus { get; set; }
        public DbSet<PaymentTransactionSupplierCodeType> PaymentTransactionSupplierCodeType { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetails> ProductDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductVariation> ProductVariations { get; set; }
        public DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public DbSet<ProductSpecificationType> SpecificationFieldTypes { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<CustomerNotification> CustomerNotifications { get; set; }
        public DbSet<LogGeral> LogsGerais { get; set; }
        public DbSet<InsightData> InsightData { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
            builder.ApplyConfiguration(new AddressEntityTypeConfiguration());
            builder.ApplyConfiguration(new CatalogEntityTypeConfiguration());
            builder.ApplyConfiguration(new CatalogCustomerEntityTypeConfiguration());
            builder.ApplyConfiguration(new CatalogProductEntityTypeConfiguration());
            builder.ApplyConfiguration(new ConversationEntityTypeConfiguration());
            builder.ApplyConfiguration(new ConversationMessageEntityTypeConfiguration());
            builder.ApplyConfiguration(new OrderItemDiscountEntityTypeConfiguration());
            builder.ApplyConfiguration(new OrderDiscountTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            builder.ApplyConfiguration(new OrderTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
            builder.ApplyConfiguration(new OrderShippingEntityTypeConfiguration());
            builder.ApplyConfiguration(new OrderStatusEntityTypeConfiguration());
            builder.ApplyConfiguration(new OrderHistoryEntityTypeConfiguration());
            builder.ApplyConfiguration(new PaymentTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new PaymentEntityTypeConfiguration());
            builder.ApplyConfiguration(new PaymentCardBrandTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new PaymentStatusEntityTypeConfiguration());
            builder.ApplyConfiguration(new PaymentTransactionEntityTypeConfiguration());
            builder.ApplyConfiguration(new PaymentTransactionStatusEntityTypeConfiguration());
            builder.ApplyConfiguration(new PaymentTransactionSupplierCodeTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            builder.ApplyConfiguration(new ProductEntityTypeConfiguration());
            builder.ApplyConfiguration(new ProductDetailsEntityTypeConfiguration());
            builder.ApplyConfiguration(new ProductCategoryEntityTypeConfiguration());
            builder.ApplyConfiguration(new ProductImageEntityTypeConfiguration());
            builder.ApplyConfiguration(new ProductSpecificationEntityTypeConfiguration());
            builder.ApplyConfiguration(new ProductSpecificationTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new ProductSpecificationEntityTypeConfiguration());
            builder.ApplyConfiguration(new ProductVariationEntityTypeConfiguration());
            builder.ApplyConfiguration(new StockEntityTypeConfiguration());
            builder.ApplyConfiguration(new CustomerNotificationEntityTypeConfiguration());
            builder.ApplyConfiguration(new SellerEntityTypeConfiguration());
            builder.ApplyConfiguration(new LogGeralEntityTypeConfiguration());
            builder.ApplyConfiguration(new InsightDataEntityTypeConfiguration());
        }
    }

    public class CoreContextDesignFactory : IDesignTimeDbContextFactory<CoreContext>
    {
        public CoreContext CreateDbContext(string[] args)
        {
            var loggerFactory = (ILoggerFactory)new LoggerFactory();
            var logger = loggerFactory.CreateLogger<CoreContext>();

            var optionsBuilder = new DbContextOptionsBuilder<CoreContext>()
                .UseSqlServer("Server=qa-linxsmartsales.database.windows.net;Database=smartsales-santa-lolla;User Id=user.developer;Password=SmartSales2022@;MultipleActiveResultSets=True;");

            return new CoreContext(optionsBuilder.Options, new SmartSalesIdentity(), logger);
        }
    }
}