using System;
using Core.SeedWork;

namespace Core.Models.Identity.Stores
{
    public class StoreGatewaySettings : EntityWithMetadata<Guid>
    {
        public string ApiKey { get; set; }
        public string ClientId { get; set; }
        public bool IsSandBox { get; set; }
        public Guid StoreId { get; set; }
        public Store Store { get; set; }
    }
}