using Infra.ExternalServices.Authentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Core.Repositories;
using Infra.EntitityConfigurations.Contexts;
using Microsoft.Extensions.Logging;

namespace Infra.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseIdentityContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IdentityContext>(serviceProvider =>
            {
                var connectionString = Environment.GetEnvironmentVariable("SQLCONNSTR_ConnectionString") ?? configuration["ConnectionString"];

                var smartSales = serviceProvider.GetRequiredService<SmartSalesIdentity>();
                var logger = serviceProvider.GetRequiredService<ILogger<IdentityContext>>();

                logger.LogInformation("-- Using master database");
                var connection = new SqlConnectionStringBuilder(connectionString);
                var options = new DbContextOptionsBuilder<IdentityContext>().UseSqlServer(connection.ConnectionString).Options;

                return new IdentityContext(options, smartSales, logger);
            });

            return services;
        }

        public static IServiceCollection UseCoreContextPerTenant(this IServiceCollection services)
        {

            services.AddScoped<CoreContext>(serviceProvider =>
            {
                var identity = serviceProvider.GetRequiredService<SmartSalesIdentity>();
                var _logger = serviceProvider.GetRequiredService<ILogger<CoreContext>>();
                var _tenantQueries = serviceProvider.GetRequiredService<ITenantRepository>();
                var _storeQueries = serviceProvider.GetRequiredService<IStoreRepository>();

                var currentCompany = identity.CurrentCompany;
                _logger.LogInformation("Using {@currentCompany} Tenant database with locationCode: {@LocationCode}", identity.CurrentCompany, identity.CurrentStorePortal);

                if (identity.IsCustomer)
                {
                    var store = _storeQueries.GetStoreByPortalNameAsync(identity.CurrentStorePortal).Result;

                    identity.CurrentStoreId = store?.Id;
                    identity.CurrentStoreCode = store?.StoreCode;
                    identity.Name = "Visitor";

                    currentCompany = store?.CompanyId ?? Guid.Empty;
                }

                var tenant = _tenantQueries.GetTenantByCompanyIdAsync(currentCompany ?? Guid.Empty).Result;
                if (tenant is null)
                    throw new UnauthorizedAccessException("Tenant not created for the current company.");

                _logger.LogInformation("Conecting Tenant database {conn}", tenant.DataBaseConnectionString);
                var options = new DbContextOptionsBuilder<CoreContext>().UseSqlServer(tenant.DataBaseConnectionString).Options;

                return new CoreContext(options, identity, _logger);
            });

            return services;
        }
    }
}