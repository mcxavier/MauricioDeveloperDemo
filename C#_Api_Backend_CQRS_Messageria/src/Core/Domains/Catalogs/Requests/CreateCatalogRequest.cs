using System;
using System.Collections.Generic;

namespace Core.Domains.Catalogs.Requests
{
    public class CreateCatalogRequest
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? SellerId { get; set; }
        public IList<int> ProductsIds { get; set; }
        public IList<int> CustomersIds { get; set; }
        public DateTime? ExpiresAt { get; set; } = null;
        public DateTime? BeginsAt { get; set; } = null;
    }
}