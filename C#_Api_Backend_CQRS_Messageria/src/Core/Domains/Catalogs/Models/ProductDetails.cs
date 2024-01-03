using System.Collections.Generic;
using Core.SeedWork;

namespace Core.Models.Core.Products
{
    public class ProductDetails : EntityWithMetadata<int>, IAggregateRoot
    {
        public string Details { get; set; }
    }
}