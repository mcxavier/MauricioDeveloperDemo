using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models.Core.Catalogs;
using Core.Models.Core.Customers;
using Core.Models.Core.Ordering;
using Core.Models.Core.Payments;
using Core.SeedWork;
using Core.SharedKernel;

namespace Core.Domains.Ordering.Models
{
    public class Order : EntityWithMetadata<int>, IAggregateRoot, IStoreReferenced
    {
        public Order()
        {
            this.OrderedAt = DateTime.Now;
        }

        public string StoreCode { get; set; }
        public string OrderCode { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public DateTime OrderedAt { get; set; }
        public decimal ShippingAmmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal Value { get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? SellerId { get; set; }
        public Seller? Seller { get; set; }
        public int? CatalogId { get; set; }
        public Catalog? Catalog { get; set; }
        public int? ShippingId { get; set; }
        public OrderShipping? Shipping { get; set; }
        public int StatusId { get; set; }
        public OrderStatus? Status { get; set; }
        public int OrderTypeId { get; set; }
        public OrderType? OrderType { get; set; }
        public IList<OrderItem>? Items { get; set; }
        public int? PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public bool SendedToUx { get; set; }

        public decimal GetItemsNetValues()
        {
            if (!Items.Any())
            {
                throw new Exception("Items needs be most then 0");
            }

            return (decimal)Items?.Aggregate(0m, (total, current) => total += current.GetNetValue());
        }

        public decimal GetItemsGrossValues()
        {
            if (!Items.Any())
            {
                throw new Exception("Items needs be most then 0");
            }

            return (decimal)Items?.Aggregate(0m, (total, current) => total += current.GetGrossValue());
        }

        public decimal GetDiscounts()
        {
            if (!Items.Any())
            {
                throw new Exception("Items needs be most then 0");
            }

            return (decimal)Items?.Aggregate(0m, (total, current) => total += current.Units * current.UnitDiscount ?? 0);
        }

        public decimal GetNetTotal()
        {
            return this.GetItemsNetValues() + this.ShippingAmmount;
        }

        public decimal GetGrossTotal()
        {
            return this.GetItemsGrossValues();
        }

        public decimal GetUnits()
        {
            return this.Items.Aggregate(0, (i, item) => i += item.Units);
        }

        public string GetReference()
        {
            return $"P{((int)this.OrderTypeId).ToString()}SMSL{this.OrderedAt:yyyyMMddHHmmss}";
        }
    }
}