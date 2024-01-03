using System;
using Core.Models.Core.Products;
using Core.SeedWork;
using Core.SharedKernel;

namespace Core.Models.Core.Customers
{
    public class CustomersOrderHistory : EntityWithMetadata<int>, IStoreReferenced
    {
        public string StoreCode { get; set; }
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string StockKeepingUnit { get; set; }
        public ProductVariation Variation { get; set; }
        public decimal? GrossValue { get; set; }
        public decimal? Discount { get; set; }
        public decimal? NetValue { get; set; }
        public int Units { get; set; }
        public DateTime? OrderDate { get; set; }
        public string SellerName { get; set; }
        public string OrderOrigin { get; set; }
    }
}