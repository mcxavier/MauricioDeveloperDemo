using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos
{
    public class LinxUxItem
    {
        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("gtin")]
        public string Gtin { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("price")]
        public decimal? Price { get; set; }

        [JsonProperty("width")]
        public int? Width { get; set; }

        [JsonProperty("height")]
        public int? Height { get; set; }

        [JsonProperty("length")]
        public int? Length { get; set; }

        [JsonProperty("weight")]
        public int? Weight { get; set; }

        [JsonProperty("discount")]
        public decimal? Discount { get; set; }

        [JsonProperty("quantity")]
        public int? Quantity { get; set; }

        [JsonProperty("basePrice")]
        public decimal? BasePrice { get; set; }

        [JsonProperty("categories")]
        public string[] Categories { get; set; }

        [JsonProperty("acquisitionType")]
        public string AcquisitionType { get; set; }

        [JsonProperty("totals")]
        public LinxUxTotals LinxUxTotals { get; set; }

        [JsonProperty("stockType")]
        public string StockType { get; set; }

        [JsonProperty("orderedQuantity")]
        public int? OrderedQuantity { get; set; }

        [JsonProperty("returnedQuantity")]
        public int? ReturnedQuantity { get; set; }

        [JsonProperty("canceledQuantity")]
        public int? CanceledQuantity { get; set; }

        [JsonProperty("shippingPrice")]
        public int? ShippingPrice { get; set; }

        [JsonProperty("itemType")]
        public string ItemType { get; set; }
    }
}
