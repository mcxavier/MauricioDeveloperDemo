using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos
{
    public class LinxUxConnectorResponses
    {
        [JsonProperty("nsu")]
        public string Nsu { get; set; }

        [JsonProperty("tid")]
        public string Tid { get; set; }

        [JsonProperty("acquirer")]
        public string Acquirer { get; set; }

        [JsonProperty("pspReference")]
        public string PspReference { get; set; }

        [JsonProperty("acquirerReference")]
        public string AcquirerReference { get; set; }

        [JsonProperty("authorizationcode")]
        public string Authorizationcode { get; set; }
    }
}
