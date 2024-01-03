using System.Collections.Generic;
using System.Threading.Tasks;
using Infra.ExternalServices.Price.Dtos;

namespace Infra.ExternalServices.Price
{
    public interface IPriceIntegrationServices
    {
        public Task<IList<UxPriceDto>> GetAllPricesFromShopCode(string shopCode, params string[] skusCodes);
    }
}