using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LinxIO.Interfaces;
using Infra.Repositories;
using Core.Repositories;
using Infra.Extensions;
using Infra.Module;
using System.IO;
using Infra.ExternalServices.Authentication;
using LinxIO.Queue.Services;
using Core.Domains.Catalogs.Repositories;
using Core.Domains.Marketing.Repositories;

namespace LinxIO
{
    public class Program
    {
        static async Task Main()
        {
            var builder = new HostBuilder();
            builder.ConfigureAppConfiguration((hostContext, config) =>
            {
                var env = hostContext.HostingEnvironment;
                config.Sources.Clear();
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", false, true);
                config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
                config.AddEnvironmentVariables();
            });
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            builder.ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<IStockRepository, StockRepository>();
                services.AddTransient<IProductsRepository, CachedProductsRepository>();
                services.AddTransient<IProductCache, ProductCache>();
                services.AddTransient<IMarketingRepository, MarketingRepository>();
                services.AddScoped<ICompanyRepository, CompanyRepository>();
                services.AddScoped<IStoreRepository, StoreRepository>();
                services.AddScoped<IQueueService, QueueService>();
                services.AddScoped<SmartSalesIdentity>();
                services.UseIdentityContext(hostContext.Configuration);
                services.AddDataServices(hostContext.Configuration);
                services.AddMediatrConfigurations();
                services.AddPaymentConfigurations();
                services.AddProductsIntegrationsConfigurations(hostContext.Configuration);
            });
            builder.ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices();
                b.AddAzureStorage();
                b.AddTimers();
            });
            builder.ConfigureLogging((context, b) =>
            {
                b.AddConsole();
            });
            builder.UseConsoleLifetime();

            var host = builder.Build();
            using (host)
            {
                try
                {
                    await host.RunAsync();
                }
                catch (Exception) { }
            }
        }
    }
}
