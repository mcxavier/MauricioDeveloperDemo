using System.Collections.Generic;
using Core.SeedWork;

namespace Core.Models.Core.Products
{
    public class Product : EntityWithMetadata<int>, IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string BrandName { get; set; }
        public string Reference { get; set; }
        public string CommonReference { get; set; }
        public string Ncm { get; set; }
        public string OriginId { get; set; }
        public string Origin { get; set; }
        public IList<ProductCategory> Categories { get; set; }
        public IList<ProductVariation> Variations { get; set; }
    }
}