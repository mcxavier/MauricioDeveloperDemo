using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Infra.ExternalServices.Price.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infra.ExternalServices.Price
{
    public class PriceIntegrationServices : IPriceIntegrationServices
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PriceIntegrationServices> _logger;

        public PriceIntegrationServices(IConfiguration configuration, ILogger<PriceIntegrationServices> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
        }

        public async Task<IList<UxPriceDto>> GetAllPricesFromShopCode(string shopCode, params string[] skusCodes)
        {
            var connectionString = Environment.GetEnvironmentVariable("SQLCONNSTR_ConnectionStringAramisUx") ?? _configuration["ConnectionStringUx"];

            if (string.IsNullOrEmpty(connectionString))
                throw new NotSupportedException("Váriavel de ambiente ConnectionStringUX não foi inicialiazada.");

            try
            {

                IEnumerable<UxPriceDto> result;
                await using (var connection = new SqlConnection(connectionString))
                {
                    result = await connection.QueryAsync<UxPriceDto>(
                       PriceIntegrationSql.GetPricesFromUx,
                       new { skusCodes, shopCode },
                       commandTimeout: 160
                    );
                }

                return result.ToList();
            }
            catch (Exception ex)
            {
                this._logger.LogCritical("Problemas na consulta de preços do UX, {conn} \n {exception} \n {query}", connectionString, ex, PriceIntegrationSql.GetPricesFromUx);
                throw;
            }
        }
    }
}