using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domains.Catalogs.Filters;
using Core.Dtos;
using Core.Models.Core.Products;
using PagedList.Core;

namespace Core.Domains.Catalogs.Repositories
{
    public interface IProductsRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Product GetProductById(int id);
        Task<IPagedList<Product>> GetProductsPaginated(int pageIndex = 1, int pageSize = 10);
        Task<IPagedList<Product>> GetProductsByIds(IList<int> id, int pageIndex = 1, int pageSize = 10);
        Task<IPagedList<ProductDto>> GetProductsFiltered(ProductFilter productFilter);
        Task<IPagedList<ProductDto>> GetOptimizedProductsFiltered(ProductFilter productFilter);
        Task<IPagedList<ProductResumeDto>> GetProductResumeByTerm(ProductFilter productFilter);
        Task<ProductVariation> GetVariationByStockKeepingUnitAsync(string sku);
    }
}