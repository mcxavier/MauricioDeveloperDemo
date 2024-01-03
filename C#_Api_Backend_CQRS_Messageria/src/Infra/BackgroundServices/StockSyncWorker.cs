using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SqlBulkTools;
using SqlBulkTools.Enumeration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Core.Products;
using Core.Repositories;
using Hangfire.Server;
using Infra.BackgroundServices.Configurations;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Infra.ExternalServices.Stock;
using Infra.ExternalServices.Stock.Dtos;
using Serilog;

namespace Api.BackgroundServices
{

    public class StockSyncWorker
    {
        private readonly IStockIntegrationServices _stockIntegrationServices;
        private readonly IServiceProvider _services;
        private ILogger _logger { get; set; }

        public StockSyncWorker(IStockIntegrationServices stockIntegrationServices, IServiceProvider services)
        {

            this._stockIntegrationServices = stockIntegrationServices;
            this._services = services;
        }

        public void TaskMethod(PerformContext context = null)
        {
            this._logger = context.CreateLoggerForPerformContext<StockSyncWorker>();
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

                    var logger = scope.ServiceProvider.GetService<Microsoft.Extensions.Logging.ILogger<CoreContext>>();
                    var coreContext = scope.ServiceProvider.GetService<CoreContext>();
                    var storeQueries = scope.ServiceProvider.GetService<IStoreRepository>();

                    try
                    {
                        var stores = storeQueries.GetAllStoresAsync(tenant.CompanyId ?? Guid.Empty).Result;
                        foreach (var store in stores)
                        {
                            StartWorkAsync(coreContext, store.StoreCode).Wait();
                        }
                    }
                    catch (Exception exception)
                    {
                        var conn = coreContext.Database.GetDbConnection();

                        this._logger.Fatal("<br/> erro em importador de estoque as {time} \n {conn} \n {@exception}", DateTimeOffset.Now, conn, exception);

                        throw;
                    }
                }
            }

            this._logger.Information("importador de estoque esperando 3600s");
        }

        protected async Task StartWorkAsync(CoreContext context, string storeCode)
        {
            var uxStocks = await this._stockIntegrationServices.GetAllStockFromShopCode(storeCode);

            await MergeStocks(uxStocks, context, storeCode);
        }

        private async Task MergeStocks(IList<UxStockDto> collection, CoreContext context, string storeCode)
        {
            var bulk = new BulkOperations();

            var entityStocks = collection.Select(x => new Stock
            {
                StoreCode = storeCode,
                Units = x.Units,
                StockKeepingUnit = x.StockKeepingUnitCode,
                LastSinc = DateTime.Now,
            }).ToList();

            this._logger.Information("Importador inserindo/atualizando {ct} estoques na loja {code}", entityStocks.Count, storeCode);
            await using (var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString))
            {

                await connection.OpenAsync();

                bulk.Setup<Stock>()
                    .ForCollection(entityStocks)
                    .WithTable("product.Stock")
                    .AddAllColumns()
                    .BulkInsertOrUpdate()
                    .SetIdentityColumn(x => x.Id, ColumnDirectionType.InputOutput)
                    .MatchTargetOn(x => x.StockKeepingUnit)
                    .MatchTargetOn(x => x.StoreCode)
                    .Commit(connection);
            }
        }
    }
}