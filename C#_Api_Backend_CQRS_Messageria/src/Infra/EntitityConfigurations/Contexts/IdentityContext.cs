using Core.Models.Identity.Companies;
using Core.Models.Identity.EconomicGroups;
using Core.Models.Identity.Stores;
using Core.Models.Identity.Subsidiaries;
using Core.Models.Identity.Tenants;
using Core.Models.Identity.Users;
using Infra.EntitityConfigurations.IdentityContext;
using Infra.EntitityConfigurations.IdentityContext.Companies;
using Infra.EntitityConfigurations.IdentityContext.EconomicGroups;
using Infra.EntitityConfigurations.IdentityContext.Stores;
using Infra.EntitityConfigurations.IdentityContext.Subsidiaries;
using Infra.EntitityConfigurations.IdentityContext.Tenants;
using Infra.ExternalServices.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Infra.EntitityConfigurations.Contexts
{
    public class IdentityContext : DataContext
    {
        public IdentityContext(DbContextOptions options, SmartSalesIdentity identity, ILogger<IdentityContext> logger) : base(options, identity, logger) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanySettings> CompanySettings { get; set; }
        public DbSet<CompanySettingsType> CompanySettingsType { get; set; }
        public DbSet<EconomicGroup> EconomicGroups { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreAddress> StoreAddresses { get; set; }
        public DbSet<StoreCampaignSettings> StoreCampaignSettings { get; set; }
        public DbSet<StoreEmailSettings> StoreEmailSettings { get; set; }
        public DbSet<StoreGatewaySettings> StoreGatewaySettings { get; set; }
        public DbSet<StoreErpSettings> StoreErpSettings { get; set; }
        public DbSet<Subsidiary> Subsidiaries { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<StoreSettings> StoreSettings { get; set; }
        public DbSet<StoreSettingsType> StoreSettingsType { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CompanyEntityTypeConfiguration());
            builder.ApplyConfiguration(new CompanySettingsEntityTypeConfiguration());
            builder.ApplyConfiguration(new CompanySettingsTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new EconomicGroupEntityTypeConfiguration());
            builder.ApplyConfiguration(new StoreEntityTypeConfiguration());
            builder.ApplyConfiguration(new StoreAddressEntityTypeConfiguration());
            builder.ApplyConfiguration(new StoreCampaignSettingsEntityTypeConfiguration());
            builder.ApplyConfiguration(new StoreEmailSettingsEntityTypeConfiguration());
            builder.ApplyConfiguration(new StoreGatewaySettingsEntityTypeConfiguration());
            builder.ApplyConfiguration(new StoreErpSettingsEntityTypeConfiguration());
            builder.ApplyConfiguration(new SubsidiaryEntityTypeConfiguration());
            builder.ApplyConfiguration(new SubsidiaryEntityTypeConfiguration());
            builder.ApplyConfiguration(new TenantEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserEntityTypeConfiguration());
            builder.ApplyConfiguration(new StoreSettingsEntityTypeConfiguration());
            builder.ApplyConfiguration(new StoreSettingsTypeEntityTypeConfiguration());
        }
    }

    public class IdentityContextDesignFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            var loggerFactory = (ILoggerFactory)new LoggerFactory();
            var logger = loggerFactory.CreateLogger<IdentityContext>();

            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>()
             //   .UseSqlServer("Server=qa-linxsmartsales.database.windows.net;Database=smartsales-master;User Id=user.developer;Password=SmartSales2022@;");
            .UseSqlServer("Server=prd-linxsmartsales.database.windows.net;Database=smartsales-master;User Id=user.smart;Password=Sm@rt#2020@;");

            return new IdentityContext(optionsBuilder.Options, new SmartSalesIdentity(), logger);
        }
    }
}