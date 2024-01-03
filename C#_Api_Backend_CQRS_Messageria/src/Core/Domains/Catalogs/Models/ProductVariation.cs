using System.Collections.Generic;
using Core.SeedWork;

namespace Core.Models.Core.Products
{
    public class ProductVariation : EntityWithMetadata<int>
    {
        public string StockKeepingUnit { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string CompleteName { get; set; }
        public decimal? BasePrice { get; set; }
        public decimal? ListPrice { get; set; }
        public decimal? CostPrice { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
        public string OriginId { get; set; }
        public string Origin { get; set; }
        public IList<ProductImage> Images { get; set; }
        public IList<ProductSpecification> Specifications { get; set; }
    }
}