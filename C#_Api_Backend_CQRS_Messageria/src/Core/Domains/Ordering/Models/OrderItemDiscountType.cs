using Core.SeedWork;

namespace Core.Models.Core.Ordering
{
    public class OrderItemDiscountType : Enumeration
    {
        public OrderItemDiscountType(int id, string name) : base(id, name) { }

        public static OrderItemDiscountType Campaign = new OrderItemDiscountType(1, "Campaign");

        public static OrderItemDiscountType Discount = new OrderItemDiscountType(2, "Discount");
    }

    public enum OrderItemDiscountTypeEnum
    {
        Campaign = 1,
        OrderItemDiscount = 2,
    }
}