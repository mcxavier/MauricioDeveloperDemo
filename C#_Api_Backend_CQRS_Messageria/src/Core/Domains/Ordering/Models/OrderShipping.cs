using Core.Domains.Ordering.Models;
using Core.Models.Core.Geography;
using Core.SeedWork;
using System;

namespace Core.Models.Core.Ordering
{
    public class OrderShipping : Entity<int>
    {
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ShippingId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Guid? StoreIdPickup { get; set; }
        public int TypeId { get; set; }
        public int DaysToDelivery { get; set; }
    }
}