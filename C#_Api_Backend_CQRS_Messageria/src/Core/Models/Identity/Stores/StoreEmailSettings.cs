using System;
using Core.SeedWork;

namespace Core.Models.Identity.Stores
{
    public class StoreEmailSettings : EntityWithMetadata<Guid>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public bool Ssl { get; set; }
        public string UrlOrderSuccessMailTemplate { get; set; }
        public Guid StoreId { get; set; }
        public Store Store { get; set; }
    }
}