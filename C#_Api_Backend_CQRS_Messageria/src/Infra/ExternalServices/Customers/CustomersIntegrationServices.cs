using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Infra.ExternalServices.Customers.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infra.ExternalServices.Customers
{
    public class CustomersIntegrationServices : ICustomersIntegrationServices
    {
        private readonly ILogger<CustomersIntegrationServices> _logger;
        private readonly IConfiguration _configuration;

        public CustomersIntegrationServices(IConfiguration configuration, ILogger<CustomersIntegrationServices> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
        }

        public async Task<IList<UxCustomerDto>> GetAllCustomersAsync(string storeCode)
        {
            var connectionString = Environment.GetEnvironmentVariable("SQLCONNSTR_ConnectionStringAramisUx") ?? _configuration["ConnectionStringUx"];

            if (string.IsNullOrEmpty(connectionString))
                throw new NotSupportedException("Váriavel de ambiente ConnectionStringUX não foi inicialiazada.");

            try
            {
                IEnumerable<UxCustomerDto> result;
                await using (var connection = new SqlConnection(connectionString))
                {
                    result = await connection.QueryAsync<UxCustomerDto>(
                        CustomersIntegrationSql.GetCustomersFromUx,
                        new { storeCode },
                        commandTimeout: 160
                    );
                }
                return result.ToList();
            }
            catch (Exception ex)
            {
                this._logger.LogCritical("Problemas na consulta de clientes do UX, {conn} \n {exception} \n {query}", connectionString, ex, CustomersIntegrationSql.GetCustomersFromUx);

                throw;
            }

        }

        public async Task<IList<UxOrderHistoryDto>> GetOrderHistoryAsync(string storeCode)
        {
            var connectionString = Environment.GetEnvironmentVariable("SQLCONNSTR_ConnectionStringAramisUx") ?? _configuration["ConnectionStringUx"];

            if (string.IsNullOrEmpty(connectionString))
                throw new NotSupportedException("Váriavel de ambiente ConnectionStringUX não foi inicialiazada.");

            try
            {
                IEnumerable<UxOrderHistoryDto> result;
                await using (var connection = new SqlConnection(connectionString))
                {
                    result = await connection.QueryAsync<UxOrderHistoryDto>(
                        CustomersIntegrationSql.GetOrderHistoryFromUx,
                        new { storeCode },
                        commandTimeout: 160
                    );
                }
                return result.ToList();
            }
            catch (Exception ex)
            {
                this._logger.LogCritical("Problemas na consulta de historico de vendas do UX, {conn} \n {exception} \n {query}", connectionString, ex, CustomersIntegrationSql.GetOrderHistoryFromUx);

                throw;
            }
        }
    }
}
