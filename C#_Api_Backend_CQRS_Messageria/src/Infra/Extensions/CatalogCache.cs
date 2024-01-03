using System.Collections.Generic;
using Infra.Responses;

namespace Infra.Extensions
{
    public class CatalogCache : ICatalogCache
    {
        private Dictionary<int, CatalogDetailsResponse> _catalogDetailResponseCache;
        private Dictionary<int, CatalogProductsResponse> _catalogProductResponseCache;
        //private Dictionary<int, List<ProductListingQueryModel>> _catalogProductsResponseCache;
        public CatalogCache()
        {
            _catalogDetailResponseCache = new Dictionary<int, CatalogDetailsResponse>();
            _catalogProductResponseCache = new Dictionary<int, CatalogProductsResponse>();
        }
        public CatalogDetailsResponse? GetCatalogDetailsById(int id)
        {
            if (_catalogDetailResponseCache.ContainsKey(id))
            {
                return _catalogDetailResponseCache[id];
            } else
            {
                return null;
            }
        }
        public void UpdateCatalogDetailsById(int id, CatalogDetailsResponse catalog)
        {
            _catalogDetailResponseCache[id] = catalog;
        }

        public bool RemoveCatalogDetailsById(int id)
        {
            if (_catalogDetailResponseCache.ContainsKey(id))
            {
                _catalogDetailResponseCache.Remove(id);
                return true;
            }
            else return false;
        }

        public CatalogProductsResponse? GetCatalogProductsById(int id)
        {
            if (_catalogProductResponseCache.ContainsKey(id))
            {
                return _catalogProductResponseCache[id];
            }
            else
            {
                return null;
            }
        }
        //public void UpdateCatalogProductsById(int id, List<ProductListingQueryModel> productList)
        //{
        //    _catalogProductsResponseCache[id] = productList;
        //}

        public void UpdateCatalogProductsById(int id, CatalogProductsResponse catalog)
        {
            _catalogProductResponseCache[id] = catalog;
        }

        public bool RemoveCatalogProductsById(int id)
        {
            if (_catalogProductResponseCache.ContainsKey(id))
            {
                _catalogProductResponseCache.Remove(id);
                return true;
            }
            else return false;
        }

    }
}
