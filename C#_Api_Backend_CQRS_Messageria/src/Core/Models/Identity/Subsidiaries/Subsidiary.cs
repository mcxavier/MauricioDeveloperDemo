using System;
using Core.Models.Identity.Companies;
using Core.Models.Identity.EconomicGroups;
using Core.SeedWork;

namespace Core.Models.Identity.Subsidiaries
{
    public class Subsidiary : Entity<Guid>
    {
        public string Name { get; set; }
        public Guid EconomicGroupId { get; set; }
        public EconomicGroup EconomicGroup { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
    }
}