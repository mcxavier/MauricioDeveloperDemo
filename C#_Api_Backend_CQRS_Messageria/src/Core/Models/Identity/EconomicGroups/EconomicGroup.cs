using System;
using Core.Models.Identity.Companies;
using Core.SeedWork;

namespace Core.Models.Identity.EconomicGroups
{
    public class EconomicGroup : EntityWithMetadata<Guid>
    {
        public string Name { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
    }
}