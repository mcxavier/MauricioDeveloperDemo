using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Core.Ordering;
using Core.Models.Core.Products;
using Core.Repositories;
using Dapper;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly CoreContext _context;
        private readonly SmartSalesIdentity _identity;
        public StockRepository(CoreContext context, SmartSalesIdentity identity)
        {
            this._context = context;
            this._identity = identity;
        }
        public async Task<Stock> GetProductStockAsync(string skuCode)
        {
            return await _context.Stock.FirstOrDefaultAsync(x => x.StockKeepingUnit == skuCode && x.StoreCode == _identity.CurrentStoreCode);
        }
        public async Task<IList<Stock>> GetProductsStockAsync(string[] skuCodes)
        {
            return await _context.Stock.Where(x => skuCodes.Contains(x.StockKeepingUnit) && x.StoreCode == _identity.CurrentStoreCode).ToListAsync();
        }
        public bool UpdateProductStock(IList<OrderItem> orderItens, bool add = false)
        {

            var parameters = new DynamicParameters(new
            {
                add = add,
                storeCode = this._identity.CurrentStoreCode
            });
            var sqlQuery = @"

                BEGIN TRANSACTION

	                DECLARE @foundSkusCount AS INT;
	                DECLARE @skusCount AS INT;
	                DECLARE @signal AS INT = 1;
	                DECLARE @skus TABLE (Sku VARCHAR(250), Units INT);
	                DECLARE @result TABLE (Sku VARCHAR(250), Units INT);
                    
                    -- Check if we are adding or removing items from stock
	                IF @add = 0 BEGIN
		                SET @signal = -1;
	                END;

	                -- Temporary table insertion for deletion with select
                    INSERT INTO @skus VALUES"
                    +
                    string.Join(',',orderItens.Select(x=>
                    {
                        return " ('" + x.StockKeepingUnit + "', " + Math.Abs(x.Units) + ")";
                    }))
                    +
                    @";
                    
                    -- Checking if which Skus have the correct amount
	                SELECT ST.StockKeepingUnit, SUM(ST.Units) AS Units
	                INTO #foundSkus
	                FROM [product].[Stock] AS ST WITH (UPDLOCK), @skus AS SK
	                WHERE ST.StockKeepingUnit = SK.Sku AND ST.StoreCode = @storeCode AND ST.Units + (SK.Units*@signal) >= 0
	                GROUP BY ST.StockKeepingUnit;

                    -- Counting the valid Skus to be incremented or removed
	                SET @foundSkusCount = (SELECT COUNT(*) FROM #foundSkus);
	                SET @skusCount = (SELECT COUNT(*) FROM @skus);

	                IF (@foundSkusCount = @skusCount) BEGIN
		                UPDATE ST SET ST.Units = ST.Units+(SK.Units*@signal)
                        FROM [product].[Stock] AS ST, @skus AS SK
		                WHERE ST.StockKeepingUnit = SK.Sku;
		                DROP TABLE #foundSkus;
	                END;
	                ELSE BEGIN
		                DROP TABLE #foundSkus;
		                THROW 50000, 'Not all products are available in stock.', 1;
	                END;

                COMMIT TRANSACTION
            ";

            try
            {

                this._context
                    .Database
                    .GetDbConnection()
                    .Query(
                        sqlQuery,
                        param: parameters,
                        commandTimeout: 0
                    );
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}