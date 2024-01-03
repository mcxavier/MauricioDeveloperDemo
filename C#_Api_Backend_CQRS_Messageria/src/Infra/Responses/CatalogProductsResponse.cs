using System;
using System.Collections.Generic;
using Core.Models.Core.Products;


namespace Infra.Responses
{
    public class CatalogProductsResponse
    {
        public string Name { get; set; }
        public IList<Product> Products { get; set; }

    }
}