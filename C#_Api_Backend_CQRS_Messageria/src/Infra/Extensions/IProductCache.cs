using Core.Models.Core.Products;
using Infra.EntitityConfigurations.Contexts;

namespace Infra.Extensions
{
    public interface IProductCache
    {
        public Product? GetProductById(CoreContext context, int id);
        public void UpdateProductById(CoreContext context, int id, Product product);
        public bool RemoveProductById(CoreContext context, int id, bool fromDB = false);
    }
}