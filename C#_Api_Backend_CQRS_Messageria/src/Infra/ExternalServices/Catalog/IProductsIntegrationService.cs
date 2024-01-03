using System.Collections.Generic;
using System.Threading.Tasks;

using CoreService.IntegrationsViewModels;

namespace Infra.ExternalServices.Catalog
{

    public interface IProductIntegrationService
    {

        Task<VtexProduct> GetById(int id);

        Task<VtexProductsIds> GetProductIds(int currentPage = 1, int pageSize = 50);

        Task<VtexProductVariation> GetProductVariationsById(int id);

        Task<VtexBasePrices> GetProductVariationPrices(int id);

        Task<List<VtexImageVariation>> GetImagetVariationsById(string id, string sku);

    }

}