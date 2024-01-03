using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Core.Customers;
using Core.Repositories;
using Hangfire.Server;
using Infra.BackgroundServices.Configurations;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Infra.ExternalServices.Customers;
using Infra.ExternalServices.Customers.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SqlBulkTools;
using SqlBulkTools.Enumeration;
using Utils.Extensions;

namespace Infra.BackgroundServices
{

    public class CustomersSyncWorker
    {
        private readonly ICustomersIntegrationServices _customerIntegrationServices;
        private readonly IServiceProvider _services;
        private ILogger _logger { get; set; }

        public CustomersSyncWorker(ICustomersIntegrationServices stockIntegrationServices, IServiceProvider services)
        {

            this._customerIntegrationServices = stockIntegrationServices;
            this._services = services;
        }

        public void TaskMethod(PerformContext context = null)
        {
            this._logger = context.CreateLoggerForPerformContext<CustomersSyncWorker>();

            this._logger.Information("Importador de clientes executando ...");

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
                        this._logger.Fatal("<br/> erro em importador de clientes as {time} \n {conn} \n {@exception}", DateTimeOffset.Now, conn, exception);

                        throw;
                    }
                }
            }

            this._logger.Information("importador de estoque esperando 3600s");
        }

        protected async Task StartWorkAsync(CoreContext context, string storeCode)
        {
            var uxCustomers = await this._customerIntegrationServices.GetAllCustomersAsync(storeCode);
            var uxOrderHistory = await this._customerIntegrationServices.GetOrderHistoryAsync(storeCode);

            await MergeCustomers(uxCustomers, context, storeCode);
            await MergeOrderHistory(uxOrderHistory, context, storeCode);
        }

        private async Task MergeCustomers(IList<UxCustomerDto> collection, CoreContext context, string storeCode)
        {

            var watch = new Stopwatch();
            watch.Start();

            var bulk = new BulkOperations();

            var entityCustomers = collection.Select(cus => MappingCustomer(cus, storeCode)).ToList();

            this._logger.Information("Importador inserindo/atualizando {ct} clientes da loja {code}", entityCustomers.Count, storeCode);
            await using (var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString))
            {

                await connection.OpenAsync();

                bulk.Setup<Customer>()
                    .ForCollection(entityCustomers)
                    .WithTable("customer.Customers")
                    .AddAllColumns()
                    .RemoveColumn(x => x.CreatedAt)
                    .RemoveColumn(x => x.CreatedBy)
                    .BulkInsertOrUpdate()
                    .SetIdentityColumn(x => x.Id, ColumnDirectionType.InputOutput)
                    .MatchTargetOn(x => x.Cpf)
                    .MatchTargetOn(x => x.StoreCode)
                    .Commit(connection);
            }
            _logger.Information("finalizando inserção em {ellapsed} {ct} clientes da loja {code}", watch.Elapsed, entityCustomers.Count, storeCode);
            watch.Reset();
        }

        private async Task MergeOrderHistory(IList<UxOrderHistoryDto> collection, CoreContext context, string storeCode)
        {
            var watch = new Stopwatch();
            watch.Start();

            var bulk = new BulkOperations();

            var customers = context.Customers.Where(x => x.StoreCode == storeCode).Select(x => new Customer
            {
                Id = x.Id,
                Cpf = x.Cpf
            }).ToList();

            var entityCustomers = collection.Select(hist =>
            {
                var customer = customers.FirstOrDefault(x => x.Cpf == hist.CustomerDocument);

                return MappingHistory(hist, customer, storeCode);

            })
            .Where(x => x.CustomerId != null)
            .ToList();

            this._logger.Information("Importador inserindo/atualizando {ct} historicos dos clintes {code}", entityCustomers.Count, storeCode);
            await using (var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString))
            {

                await connection.OpenAsync();

                bulk.Setup<CustomersOrderHistory>()
                    .ForCollection(entityCustomers)
                    .WithTable("customer.OrderHistory")
                    .AddAllColumns()
                    .BulkInsertOrUpdate()
                    .SetIdentityColumn(x => x.Id, ColumnDirectionType.InputOutput)
                        .MatchTargetOn(h => h.StoreCode)
                        .MatchTargetOn(h => h.CustomerId)
                        .MatchTargetOn(h => h.StockKeepingUnit)
                        .MatchTargetOn(h => h.GrossValue)
                        .MatchTargetOn(h => h.Discount)
                        .MatchTargetOn(h => h.NetValue)
                        .MatchTargetOn(h => h.Units)
                        .MatchTargetOn(h => h.OrderDate)
                        .MatchTargetOn(h => h.SellerName)
                        .MatchTargetOn(h => h.OrderOrigin)
                    .Commit(connection);
            }

            _logger.Information("finalizando inserção em {ellapsed} {ct} historicos da loja {code}", watch.Elapsed, entityCustomers.Count, storeCode);
            watch.Reset();

        }

        public static Customer MappingCustomer(UxCustomerDto cus, string storeCode)
        {
            var customer = new Customer
            {
                StoreCode = storeCode,
                Email = cus.Email?.ToLower(),
                Name = $"{cus.FirstName?.ToLower()} {cus.LastName?.ToLower()}",
                Phone = !cus.ContactNumber.IsNullOrEmpty() ? $"55{cus.ContactNumber}" : "",
                BirthDay = cus.BirthDay,
                Rg = cus.Rg,
                Cpf = cus.Document,
                IsActive = true,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                CreatedBy = "System",
                ModifiedBy = "System"
            };

            return customer;
        }

        public static CustomersOrderHistory MappingHistory(UxOrderHistoryDto history, Customer customer, string storeCode)
        {
            var orderHistory = new CustomersOrderHistory
            {
                StoreCode = storeCode,
                Discount = history?.Discount,
                Units = history?.Units ?? 0,
                GrossValue = history?.GrossValue,
                OrderOrigin = "LinxUX",
                StockKeepingUnit = history?.StockKeepingUnit,
                NetValue = history?.NetValue,
                OrderDate = history?.OrderDate,
                SellerName = (history?.SellerName ?? "").ToLower(),
                CustomerId = customer?.Id,
                IsActive = true,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                CreatedBy = "System",
                ModifiedBy = "System"
            };

            return orderHistory;
        }
    }
}