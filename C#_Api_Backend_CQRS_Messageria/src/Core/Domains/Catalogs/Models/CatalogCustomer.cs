using Core.Models.Core.Customers;
using Core.SeedWork;
using System;

namespace Core.Models.Core.Catalogs
{
    public class CatalogCustomer : Entity<int>
    {
        public int CatalogId { get; set; }
        public Catalog Catalog { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime? SentAt{ get; set; }
    }
}