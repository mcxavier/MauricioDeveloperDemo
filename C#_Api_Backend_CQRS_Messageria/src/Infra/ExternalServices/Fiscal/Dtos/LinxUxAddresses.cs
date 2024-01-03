using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos
{
    public class LinxUxAddresses
    {
        [JsonProperty("billing")]
        public LinxUxBillingAddress Billing { get; set; }
    }
}
