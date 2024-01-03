using System;
using Core.SeedWork;

namespace Core.Models.Identity.Stores
{
    public class StoreSettings : EntityWithMetadata<Guid>
    {
        public Guid StoreId { get; set; }
        public Store Store { get; set; }
        public string Value { get; set; }
        public int? TypeId { get; set; }
        public StoreSettingsType? Type { get; set; }

    }
}
