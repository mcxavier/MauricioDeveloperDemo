using System;
using Core.SeedWork;
using Core.SharedKernel;

namespace Core.Models.Core.Products
{
    public class Stock : Entity<int>
    {
        public string StoreCode { get; set; }
        public string StockKeepingUnit { get; set; }
        public int Units { get; set; }
        public DateTime LastSinc { get; set; } = DateTime.Now;
    }
}