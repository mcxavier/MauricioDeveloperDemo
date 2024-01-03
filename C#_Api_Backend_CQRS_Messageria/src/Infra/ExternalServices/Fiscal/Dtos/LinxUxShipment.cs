using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos
{
    public class LinxUxShipment
    {
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("address")]
        public LinxUxBillingAddress Address { get; set; }

        [JsonProperty("carrierId")]
        public int? CarrierId { get; set; } = null;

        [JsonProperty("shippingTime")]
        public int ShippingTime { get; set; }

        [JsonProperty("shippingPrice")]
        public long ShippingPrice { get; set; }
    }
}
