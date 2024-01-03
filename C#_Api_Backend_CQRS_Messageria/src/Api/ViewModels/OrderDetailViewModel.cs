using System.Collections.Generic;

namespace Api.ViewModels
{
    public class OrderDetailViewModel
    {
        public List<OrderProductDetailViewModel> OrderItens { get; set; }
        public string PaymentType { get; set; }
        public string CardNumber { get; set; }
        public decimal OrderNetValue { get; set; }
        public decimal OrderGrossValue { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingAmmount { get; set; }
        public int Installments { get; set; }
        public int SellerId { get; set; }
        public string SellerName { get; set; }
    }
}
