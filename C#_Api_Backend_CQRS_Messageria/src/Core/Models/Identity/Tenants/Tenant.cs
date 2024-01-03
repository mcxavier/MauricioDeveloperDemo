using System;
using Core.Models.Identity.Companies;
using Core.SeedWork;

namespace Core.Models.Identity.Tenants
{
    public class Tenant : Entity<Guid>
    {
        public string Name { get; set; }
        public string DataBaseConnectionString { get; set; }
        public Guid? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}