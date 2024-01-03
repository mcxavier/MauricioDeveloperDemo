using Core.SeedWork;

namespace Core.Models.Core.Ordering
{
    public class OrderType : Enumeration
    {
        public OrderType(int id, string name) : base(id, name) { }
        public static OrderType Shipment = new OrderType(1, "Shipment");
        public static OrderType Pickup = new OrderType(2, "Pickup");
    }

    public enum OrderTypeEnum
    {
        Shipping = 1,
        Pickup = 2
    }
}