using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Infra.ExternalServices.Stock.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infra.ExternalServices.Stock
{
    public class StockIntegrationServices : IStockIntegrationServices
    {
        private readonly ILogger<StockIntegrationServices> _logger;
        private readonly IConfiguration _configuration;

        public StockIntegrationServices(IConfiguration configuration, ILogger<StockIntegrationServices> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
        }

        public async Task<IList<UxStockDto>> GetAllStockFromShopCode(string shopCode)
        {
            var connectionString = Environment.GetEnvironmentVariable("SQLCONNSTR_ConnectionStringAramisUx") ?? _configuration["ConnectionStringUx"];

            if (string.IsNullOrEmpty(connectionString))
                throw new NotSupportedException("Váriavel de ambiente ConnectionStringUX não foi inicialiazada.");

            try
            {
                string[] skusCodes = null;
                IEnumerable<UxStockDto> result;
                await using (var connection = new SqlConnection(connectionString))
                {
                    result = await connection.QueryAsync<UxStockDto>(
                        StockIntegrationSql.GetStockFromUx,
                        new { skusCodes, shopCode },
                        commandTimeout: 160
                    );
                }

                return result.ToList();
            }
            catch (Exception ex)
            {
                this._logger.LogCritical("Problemas na consulta de estoque do UX, {conn} \n {exception} \n {query}", connectionString, ex, StockIntegrationSql.GetStockFromUx);

                throw;
            }
        }

        public async Task<IList<UxStockDto>> GetProductStockAsync(string shopCode, params string[] skusCodes)
        {
            var connectionString = Environment.GetEnvironmentVariable("SQLCONNSTR_ConnectionStringAramisUx") ?? _configuration["ConnectionStringUx"];

            if (string.IsNullOrEmpty(connectionString))
                throw new NotSupportedException("Váriavel de ambiente ConnectionStringUX não foi inicialiazada.");

            try
            {
                IEnumerable<UxStockDto> result;
                await using (var connection = new SqlConnection(connectionString))
                {
                    result = await connection.QueryAsync<UxStockDto>(
                        StockIntegrationSql.GetStockFromUx,
                        new { skusCodes, shopCode },
                        commandTimeout: 160
                    );
                }

                return result.ToList();

            }
            catch (Exception ex)
            {
                this._logger.LogCritical("Problemas na consulta de estoque do UX, {conn} \n {exception} \n {query}", connectionString, ex, StockIntegrationSql.GetStockFromUx);
                throw;
            }
        }
    }
}
