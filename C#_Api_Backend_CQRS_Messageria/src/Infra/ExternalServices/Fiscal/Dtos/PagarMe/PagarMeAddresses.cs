using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos.PagarMe
{
    public class PagarMeAddresses
    {
        [JsonProperty("billing")]
        public PagarMeBillingAddress Billing { get; set; }
    }
}
