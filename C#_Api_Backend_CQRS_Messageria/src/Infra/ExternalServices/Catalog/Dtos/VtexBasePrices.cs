using System;
using System.Collections.Generic;

namespace CoreService.IntegrationsViewModels
{

    public class VtexBasePrices
    {

        public string ItemId { get; set; }

        public decimal? ListPrice { get; set; }

        public decimal? CostPrice { get; set; }

        public decimal? Markup { get; set; }

        public decimal? BasePrice { get; set; }

        // TODO: asdasdada
        // public IList<VtexBasePrices_FixedPrice> FixedPrices { get; set; }

    }

    public class VtexBasePrices_FixedPrice
    {

        public string TradePolicyId { get; set; }

        public int? Value { get; set; }

        public int? ListPrice { get; set; }

        public int? MinQuantity { get; set; }

        public VtexBasePrices_DateRange DateRange { get; set; }

    }

    public class VtexBasePrices_DateRange
    {

        public DateTime From { get; set; }

        public DateTime To { get; set; }

    }

}