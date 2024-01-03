using System;
using Core.Models.Identity.Tenants;
using Core.SeedWork;

namespace Core.Models.Identity.Companies
{

    public class Company : EntityWithMetadata<Guid>
    {
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Cnpj { get; set; }
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}