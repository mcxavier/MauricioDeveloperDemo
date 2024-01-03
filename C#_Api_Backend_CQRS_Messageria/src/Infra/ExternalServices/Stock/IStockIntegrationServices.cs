using System.Collections.Generic;
using System.Threading.Tasks;
using Infra.ExternalServices.Stock.Dtos;

namespace Infra.ExternalServices.Stock
{
    public interface IStockIntegrationServices
    {
        public Task<IList<UxStockDto>> GetAllStockFromShopCode(string shopCode);
        public Task<IList<UxStockDto>> GetProductStockAsync(string shopCode, params string[] skusCodes);
    }
}