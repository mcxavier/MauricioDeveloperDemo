using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos.PagarMe
{

    public class PagarMeTotals
    {

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonProperty("shippingAmount")]
        public decimal ShippingAmount { get; set; }

        [JsonProperty("priceDiscounted")]
        public decimal PriceDiscounted { get; set; }

        [JsonProperty("giftPackagePrice")]
        public decimal GiftPackagePrice { get; set; }

        [JsonProperty("priceShippingAmount")]
        public decimal PriceShippingAmount { get; set; }

        [JsonProperty("proportionalDiscount")]
        public decimal ProportionalDiscount { get; set; }

    }

}