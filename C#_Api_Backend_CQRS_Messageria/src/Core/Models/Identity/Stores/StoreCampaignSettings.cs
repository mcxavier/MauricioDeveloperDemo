using System;
using Core.SeedWork;

namespace Core.Models.Identity.Stores
{
    public class StoreCampaignSettings : EntityWithMetadata<Guid>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ServiceUrl { get; set; }
        public Guid StoreId { get; set; }
        public Store Store { get; set; }
    }
}