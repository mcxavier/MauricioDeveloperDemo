using Core.Models.Core.Products;
using Core.SeedWork;

namespace Core.Models.Core.Catalogs
{
    public class CatalogProduct : Entity<int>
    {
        public int CatalogId { get; set; }
        public Catalog Catalog { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}