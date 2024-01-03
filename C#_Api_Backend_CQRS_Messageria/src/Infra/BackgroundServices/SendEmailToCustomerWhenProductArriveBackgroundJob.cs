using System;
using System.Threading.Tasks;
using Api.BackgroundServices;
using Core.Domains.Marketing.Commands.SendEmailToCustomerWhenProductArrive;
using Core.Domains.Marketing.Repositories;
using Core.Repositories;
using Hangfire.Server;
using Infra.BackgroundServices.Configurations;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace Infra.BackgroundServices
{

    public interface ISendEmailToCustomerWhenProductArriveBackgroundJob
    {
        void TaskMethod(PerformContext context);
    }

    public class SendEmailToCustomerWhenProductArriveBackgroundJob : ISendEmailToCustomerWhenProductArriveBackgroundJob
    {
        private readonly IServiceProvider _services;
        private ILogger _logger { get; set; }

        public SendEmailToCustomerWhenProductArriveBackgroundJob(IServiceProvider services)
        {
            this._services = services;
        }

        public void TaskMethod(PerformContext context = null)
        {
            this._logger = context.CreateLoggerForPerformContext<PriceFromUxSyncWorker>();
            this._logger.Information("Importador executando ...");

            using (var scope = _services.CreateScope())
            {
                var tenantQueries = scope.ServiceProvider.GetService<ITenantRepository>();
                var tenants = tenantQueries.GetAllTenants().Result;

                foreach (var tenant in tenants)
                {
                    var identity = scope.ServiceProvider.GetService<SmartSalesIdentity>();
                    identity.Name = "System";
                    identity.CurrentCompany = tenant.CompanyId;

                    var logger = scope.ServiceProvider.GetService<ILogger<CoreContext>>();
                    var coreContext = scope.ServiceProvider.GetService<CoreContext>();
                    var storeQueries = scope.ServiceProvider.GetService<IStoreRepository>();

                    try
                    {
                        var stores = storeQueries.GetAllStoresAsync(tenant.CompanyId ?? Guid.Empty).Result;
                        foreach (var store in stores)
                        {
                            StartWorkAsync(scope, coreContext, store.StoreCode).Wait();
                        }
                    }
                    catch (Exception exception)
                    {
                        var conn = coreContext.Database.GetDbConnection();

                        this._logger.Fatal("<br/> erro em importador de preços as {time} \n {conn} \n {@exception}", DateTimeOffset.Now, conn, exception);
                        throw;
                    }
                }
            }

            this._logger.Information("importador de preços esperando 3600s");
        }

        protected async Task StartWorkAsync(IServiceScope scope, CoreContext context, string storeCode)
        {
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var repository = scope.ServiceProvider.GetService<IMarketingRepository>();
            var unnotifiedCustomers = await repository.GetAllUnnotifiedCustomers(storeCode);

            foreach (var unnotifiedCustomer in unnotifiedCustomers)
            {
                var command = new SendEmailToCustomerWhenProductArriveCommand(stockKeepingUnit: unnotifiedCustomer.StockKeepingUnit, customerName: unnotifiedCustomer.CustomerName, customerEmail: unnotifiedCustomer.CustomerEmail);
                await mediator.Send(command);
            }
        }
    }
}