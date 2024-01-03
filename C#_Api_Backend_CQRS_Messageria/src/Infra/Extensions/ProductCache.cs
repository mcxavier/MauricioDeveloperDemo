using System.Collections.Generic;
using System.Linq;
using Core.Models.Core.Products;
using Infra.EntitityConfigurations.Contexts;

namespace Infra.Extensions
{
    public class ProductCache : IProductCache
    {
        private Dictionary<int, string> _productCache;
        public bool useMemory = false; // Flag that enables the cache in flash memory
        public bool useDB = false; // Flag that enables the cache in DB
        public ProductCache()
        {
            this._productCache = new Dictionary<int, string>();
        }
        public Product? GetProductById(CoreContext context, int id)
        {
            // Check if available in memory
            if (useMemory && _productCache.ContainsKey(id))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(_productCache[id]);
            }

            string productDetails = null;
            if (useDB)
            {
                // Check if available in ProductDetails (cache information in database)
                productDetails = context.ProductDetails.Where(x => x.Id == id)
                                                                .Select(x => x.Details)
                                                                .FirstOrDefault();
            }

            if (productDetails == null)
            {
                return null;
            }
            else
            {
                _productCache[id] = productDetails;
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(productDetails);
            }
        }
        public void UpdateProductById(CoreContext context, int id, Product product)
        {
            if (useMemory || useDB)
            {
                string productDetails = Newtonsoft.Json.JsonConvert.SerializeObject(product);
                if (useMemory)
                {
                    _productCache[id] = productDetails;
                }

                if (useDB)
                {
                    // Check if available in ProductDetails (cache information in database)
                    ProductDetails productDetailsEntity = context.ProductDetails.Where(x => x.Id == id).FirstOrDefault();
                    if (productDetailsEntity == null)
                    {
                        // Insert new entry on database
                        productDetailsEntity = new ProductDetails();
                        productDetailsEntity.Id = id;
                        productDetailsEntity.Details = productDetails;
                        context.Add(productDetailsEntity);
                    }
                    else
                    {
                        // Update existing entry
                        productDetailsEntity.Id = id;
                        productDetailsEntity.Details = productDetails;
                        context.Update(productDetailsEntity);
                    }
                    context.SaveChanges();
                }
            }
        }
        public bool RemoveProductById(CoreContext context, int id, bool fromDB)
        {
            if (useDB && fromDB)
            {
                // Check if we have cache in database and then remove it (invalidate)
                // to further refreshing it when asked again
                ProductDetails productDetailsEntity = context.ProductDetails.Where(x => x.Id == id).FirstOrDefault();
            
                if (productDetailsEntity != null)
                {
                    context.ProductDetails.Remove(productDetailsEntity);
                    context.SaveChanges();
                }
            }

            if (useMemory && _productCache.ContainsKey(id))
            {
                _productCache.Remove(id);
                return true;
            }
            else return false;
        }
    }
}