using System;
using Core.SeedWork;

namespace Core.Models.Identity.Companies
{
    public class CompanySettings : EntityWithMetadata<Guid>
    {
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public int? TypeId { get; set; }
        public CompanySettingsType? Type { get; set; }
        public string Value { get; set; }
    }
}