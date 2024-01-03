using System;
using System.Collections.Generic;


namespace Infra.Responses
{
    public class CatalogDetailsResponse
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? BeginsAt { get; set; } = null;
        public DateTime? ExpiresAt { get; set; } = null;
        public decimal? Revenues { get; set; }
        public IList<CatalogDetailsResponse_Product> Products { get; set; }
        public CatalogDetailsResponse_Seller Seller { get; set; }
        public IList<CatalogDetailsResponse_Customer> Customers { get; set; }
    }

    public class CatalogDetailsResponse_Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public string ImageUrl { get; set; }
        public string Reference { get; set; }
    }

    public class CatalogDetailsResponse_Seller
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

        public class CatalogDetailsResponse_Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool Received { get; set; }
    }
}