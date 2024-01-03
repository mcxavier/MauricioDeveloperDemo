using System;
using System.Collections.Generic;


namespace Core.Domains.Catalogs.Dtos
{
    public class CatalogResumeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? BeginsAt { get; set; } = null;
        public DateTime? ExpiresAt { get; set; } = null;
        public int? NumOfPieces { get; set; }
        public int? NumOfSales { get; set; }
        public decimal? Revenues { get; set; }
        public string? SentContacts { get; set; }
        public string Status { get; set; }
    }

}