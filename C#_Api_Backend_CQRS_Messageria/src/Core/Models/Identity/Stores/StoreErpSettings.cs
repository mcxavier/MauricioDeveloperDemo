using System;
using Core.SeedWork;

namespace Core.Models.Identity.Stores
{
    public class StoreErpSettings : EntityWithMetadata<Guid>
    {
        public string AppHost { get; set; }
        public string ServiceHost { get; set; }
        public string Environment { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public Guid StoreId { get; set; }
        public Store Store { get; set; }
    }
}