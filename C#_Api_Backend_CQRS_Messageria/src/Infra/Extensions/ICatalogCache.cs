using Infra.Responses;

namespace Infra.Extensions
{
    public interface ICatalogCache
    {
        public CatalogDetailsResponse? GetCatalogDetailsById(int id);
        public CatalogProductsResponse? GetCatalogProductsById(int id);
        public void UpdateCatalogDetailsById(int id, CatalogDetailsResponse? catalog);
        public void UpdateCatalogProductsById(int id, CatalogProductsResponse? catalog);
        public bool RemoveCatalogDetailsById(int id);
        public bool RemoveCatalogProductsById(int id);
    }
}
