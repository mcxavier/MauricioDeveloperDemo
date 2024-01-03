using System;
using Core.Models.Identity.Companies;
using Core.Models.Identity.Subsidiaries;
using Core.SeedWork;

namespace Core.Models.Identity.Stores
{
    public class Store : EntityWithMetadata<Guid>, IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string StoreCode { get; set; }
        public string PortalUrl { get; set; }
        public string Cnpj { get; set; }
        public string OriginId { get; set; }
        public Guid? CompanyId { get; set; }
        public Company Company { get; set; }
        public Guid? SubsidiaryId { get; set; }
        public Subsidiary Subsidiary { get; set; }
    }
}