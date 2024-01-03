using System;
using Core.SeedWork;

namespace Core.Models.Core.Customers
{
    public class Seller : EntityWithMetadata<int>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDay { get; set; }
        public int OriginId { get; set; }
        public string Document { get; set; }
    }
}