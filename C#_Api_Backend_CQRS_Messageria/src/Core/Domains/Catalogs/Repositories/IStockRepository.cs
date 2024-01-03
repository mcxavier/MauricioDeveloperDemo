using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models.Core.Ordering;
using Core.Models.Core.Products;

namespace Core.Repositories
{
    public interface IStockRepository
    {
        Task<Stock> GetProductStockAsync(string skuCode);
        Task<IList<Stock>> GetProductsStockAsync(params string[] skusCodes);
        bool UpdateProductStock(IList<OrderItem> orderItens, bool add = false);
    }
}