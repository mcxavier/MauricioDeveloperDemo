using System.Collections.Generic;
using Core.SeedWork;
using Core.SharedKernel;
using System;

namespace Core.Models.Core.Catalogs
{
    public class Catalog : EntityWithMetadata<int>, IAggregateRoot, IStoreReferenced
    {
        public string StoreCode { get; set; }
        public int? SellerId { get; set; }
        public string Name { get; set; }
        public IEnumerable<CatalogCustomer> Customers { get; set; }
        public IEnumerable<CatalogProduct> Products { get; set; }
        public DateTime? ExpiresAt { get; set; } = null;
        public DateTime? BeginsAt { get; set; } = DateTime.Now;
        public int? NumOfPieces { get; set; }
        public int? NumOfSales { get; set; }
        public decimal? Revenues { get; set; }
        public string? SentContacts { get; set; }
    }
}