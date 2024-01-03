using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos.PagarMe
{
    public class PagarMeShipment
    {
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("address")]
        public PagarMeBillingAddress Address { get; set; }

        [JsonProperty("carrierId")]
        public int? CarrierId { get; set; } = null;

        [JsonProperty("shippingTime")]
        public int ShippingTime { get; set; }

        [JsonProperty("shippingPrice")]
        public long ShippingPrice { get; set; }
    }
}
