using CoreService.Infrastructure.Services;
using FluentValidation;
using Infra.Extensions;
using Infra.ExternalServices.Authentication;
using Infra.ExternalServices.Catalog;
using Infra.ExternalServices.Fiscal;
using Infra.ExternalServices.MailSender;
using Infra.ExternalServices.MailSender.Configurations;
using Infra.ExternalServices.Payments;
using Infra.ExternalServices.Payments.Contracts;
using Infra.ExternalServices.Payments.Vendors.PagarMe;
using Infra.ExternalServices.Payments.Vendors.PagarMe.configurations;
using Infra.ExternalServices.Reshop;
using Infra.ExternalServices.Azure;
using Infra.ExternalServices.Azure.Configurations;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Core.Domains.Catalogs.Repositories;
using Core.Domains.Customers.Repositories;
using Core.Domains.Marketing.Repositories;
using Core.Domains.Ordering.DomainServices;
using Core.Domains.Ordering.Repositories;
using Core.Repositories;
using Infra.BackgroundServices;
using Infra.DomainInfra.DomainServicesImplementation;
using Infra.ExternalServices.Customers;
using Infra.ExternalServices.Price;
using Infra.ExternalServices.Stock;
using Infra.QueryCommands._Kernel.Behaviors;
using Infra.QueryCommands.Queries.Customers;
using Infra.Repositories;
using Infra.Domains.Catalogs;

namespace Infra.Module
{

    public static class InfraModule
    {

        public static void AddInfraModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataServices(configuration);
            services.AddProductsIntegrationsConfigurations(configuration);
            services.AddMailSenderConfigurations(configuration);
            services.AddMediatrConfigurations();
            services.AddPaymentConfigurations();
            services.AddRepositories();
            services.AddLinxUxConfigurations();
            services.AddAzureConfigurations(configuration);
            services.AddCaching();
            services.AddAuthenticateServices();
            services.AddOrderDomainServices();
            services.AddCatalogDomainServices();
        }

        public static void AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<SmartSalesIdentity>();
            services.AddEntityFrameworkSqlServer();
            services.UseIdentityContext(configuration);
            services.UseCoreContextPerTenant();
        }

        public static void AddMediatrConfigurations(this IServiceCollection services)
        {
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains(nameof(Infra)) || x.FullName.Contains(nameof(Core))).ToList();

            foreach (var assembly in allAssemblies)
            {
                AssemblyScanner
                    .FindValidatorsInAssembly(assembly)
                    .ForEach(result =>
                    {
                        services.AddScoped(result.InterfaceType, result.ValidatorType);
                    });

                services.AddMediatR(assembly);
            }

            // services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }

        public static void AddPaymentConfigurations(this IServiceCollection services)
        {
            services.AddTransient<IPaymentGatewayContext, PaymentGatewayContext>();
            services.AddTransient<IPaymentGatewayServices, PagarMeGatewayServices>();
            services.AddTransient<IPagarMeOrderIntegrationService, PagarMeOrderIntegrationService>();
        }

        public static void AddLinxUxConfigurations(this IServiceCollection services)
        {
            //services.AddTransient<IPriceIntegrationServices, PriceIntegrationServices>();
            //services.AddTransient<IStockIntegrationServices, StockIntegrationServices>();
            //services.AddTransient<ILinxUxOrderIntegrationService, LinxUxOrderIntegrationService>();
            //services.AddTransient<ICustomersIntegrationServices, CustomersIntegrationServices>();
            //services.AddTransient<IPagarMeOrderIntegrationService, PagarMeOrderIntegrationService>();
        }

        public static void AddAzureConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureStorageConfig>(configuration.GetSection(nameof(AzureStorageConfig)));
            services.AddSingleton<IAzureStorageConfig>(x => x.GetRequiredService<IOptions<AzureStorageConfig>>().Value);
            services.AddTransient<IAzureStorageIntegrationServices, AzureStorageIntegrationServices>();
        }

        public static void AddProductsIntegrationsConfigurations(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHttpClient<IProductIntegrationService, ProductService>();

            //Reshop
            services.AddHttpClient<IReshopIntegrationService, ReshopIntegrationService>();
            services.AddHttpClient<ILgpdIntegrationService, LgpdIntegrationService>();
            services.AddHttpClient<ICampaignIntegrationService, CampaignService>();
        }

        public static void AddMailSenderConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEmailServices, EmailServices>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ISettingsRepository, SettingsRepository>();
            services.AddTransient<IStockRepository, StockRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IProductsRepository, CachedProductsRepository>();
            services.AddTransient<ICatalogRepository, CatalogRepository>();
            services.AddTransient<ICustomersRepository, CustomersRepository>();
            services.AddTransient<IStoreRepository, StoreRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IMarketingRepository, MarketingRepository>();
        }

        public static void AddCaching(this IServiceCollection services)
        {
            services.AddSingleton<ICatalogCache>(x => x.GetRequiredService<IOptions<CatalogCache>>().Value);
            services.AddSingleton<IProductCache>(x => x.GetRequiredService<IOptions<ProductCache>>().Value);
        }

        public static void AddAuthenticateServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationApi, AuthenticationApi>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
        }

        public static void AddOrderDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IOrderEmailBuilder, OrderSuccessEmailBuilder>();
            services.AddTransient<ISendEmailToCustomerWhenProductArriveBackgroundJob, SendEmailToCustomerWhenProductArriveBackgroundJob>();
        }

        public static void AddCatalogDomainServices(this IServiceCollection services)
        {
            services.AddTransient<ICatalogService, CatalogService>();
        }

    }
}
