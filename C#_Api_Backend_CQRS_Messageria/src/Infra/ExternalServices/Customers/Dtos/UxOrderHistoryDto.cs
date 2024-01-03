using System;

namespace Infra.ExternalServices.Customers.Dtos
{

    public class UxOrderHistoryDto
    {

        public string StoreCode { get; set; }

        public string StockKeepingUnit { get; set; }

        public decimal? GrossValue { get; set; }

        public decimal? Discount { get; set; }

        public decimal? NetValue { get; set; }

        public int Units { get; set; }

        public DateTime? OrderDate { get; set; }

        public string CustomerDocument { get; set; }

        public string SellerName { get; set; }

    }

}