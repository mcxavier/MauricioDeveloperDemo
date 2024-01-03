using System;
using System.Collections.Generic;

namespace Core.Domains.Catalogs.Queries
{
    public class FilterCatalogQuery
    {
        public int? Id { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Name { get; set; }
        public int? SellerId { get; set; }
        public int Status {get; set;}
        public DateTime? ExpiresAt { get; set; } = null;
        public DateTime? BeginsAt { get; set; } = null;
        public DateTime? BeginsAtInitialDate { get; set; } = null;
        public DateTime? BeginsAtFinalDate { get; set; } = null;
        public bool OrderByCreatedAt { get; set; } = true;
    }
}