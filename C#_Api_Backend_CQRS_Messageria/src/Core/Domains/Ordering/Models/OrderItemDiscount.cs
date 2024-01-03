using Core.SeedWork;

namespace Core.Models.Core.Ordering
{
    public class OrderItemDiscount : Entity<int>
    {
        public decimal Value { get; set; }
        public int ReferenceId { get; set; }
        public int DiscountTypeId { get; set; }
        public OrderItemDiscountType DiscountType { get; set; }
        public int? OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}
