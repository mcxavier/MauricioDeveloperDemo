using System.Collections.Generic;
using Core.Domains.Ordering.Models;
using Core.Models.Core.Products;
using Core.SeedWork;

namespace Core.Models.Core.Ordering
{
    public class OrderItem : Entity<int>
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string StockKeepingUnit { get; set; }
        public string Reference { get; set; }
        public string UrlPicture { get; set; }
        public int Units { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? UnitDiscount { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ProductVariationId { get; set; }
        public ProductVariation ProductVariation { get; set; }
        public List<OrderItemDiscount> DiscountItems { get; set; }

        public decimal GetUnitNetValue()
        {
            return (this.UnitPrice - (this.UnitDiscount ?? 0));
        }

        public decimal GetNetValue()
        {
            return this.GetUnitNetValue() * this.Units;
        }

        public decimal GetGrossValue()
        {
            return this.UnitPrice * this.Units;
        }
    }
}