using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SqlBulkTools;
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
using Infra.ExternalServices.Price;
using Infra.ExternalServices.Price.Dtos;
using MoreLinq;
using PagedList.Core;
using Serilog;

namespace Api.BackgroundServices
{

    public class PriceFromUxSyncWorker
    {
        private readonly IPriceIntegrationServices _priceIntegrationServices;
        private readonly IServiceProvider _services;
        private ILogger _logger { get; set; }

        public PriceFromUxSyncWorker(IPriceIntegrationServices priceIntegrationServices, IServiceProvider services)
        {

            this._priceIntegrationServices = priceIntegrationServices;
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

                        this._logger.Fatal("<br/> erro em importador de preços as {time} \n {conn} \n {@exception}", DateTimeOffset.Now, conn, exception);
                        throw;
                    }
                }
            }

            this._logger.Information("importador de preços esperando 3600s");
        }

        protected async Task StartWorkAsync(CoreContext context, string storeCode)
        {
            var (index, perPage) = (1, 2000);

            var list = GetVariations(context, index, perPage);

            for (var i = 1; i <= list.PageCount; i++)
            {

                var skus = list.Select(x => x.StockKeepingUnit).ToArray();

                var uxStocks = await this._priceIntegrationServices.GetAllPricesFromShopCode(storeCode, skus);

                await MergePrices(uxStocks, list.ToList(), context, storeCode);

                list = GetVariations(context, i, perPage);
            }
        }

        private class SkuGroup
        {

            public string StockKeepingUnit { get; set; }

            public string OriginId { get; set; }

        }

        private IPagedList<SkuGroup> GetVariations(CoreContext context, int index, int perPage)
        {

            return context.ProductVariations
                          .Where(x => x.IsActive)
                          .Select(x => new SkuGroup
                          {
                              StockKeepingUnit = x.StockKeepingUnit,
                              OriginId = x.OriginId
                          }).ToPagedList(index, perPage);
        }

        private async Task MergePrices(IList<UxPriceDto> collection, IList<SkuGroup> list, CoreContext context, string storeCode)
        {
            var bulk = new BulkOperations();

            var entityVariations = collection.Select(x => new ProductVariation
            {
                StockKeepingUnit = x.StockKeepingUnitCode,
                BasePrice = x.BasePrice,
                ListPrice = x.ListPrice,
                ModifiedAt = DateTime.Now,
                ModifiedBy = "system price update sync",
                OriginId = list.Where(y => y.StockKeepingUnit == x.StockKeepingUnitCode).Select(y => y.OriginId).FirstOrDefault()
            }).DistinctBy(x => x.OriginId).ToList();

            this._logger.Information("Importador inserindo/atualizando {ct} preços na loja {code}", entityVariations.Count, storeCode);
            await using (var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString))
            {

                await connection.OpenAsync();

                bulk.Setup<ProductVariation>()
                    .ForCollection(entityVariations)
                    .WithTable("product.Variations")
                        .AddColumn(variation => variation.BasePrice)
                        .AddColumn(variation => variation.ListPrice)
                        .AddColumn(variation => variation.ModifiedAt)
                        .AddColumn(variation => variation.ModifiedBy)
                    .BulkUpdate()
                        .MatchTargetOn(x => x.StockKeepingUnit)
                        .MatchTargetOn(x => x.OriginId)
                    .Commit(connection);
            }
        }
    }
}