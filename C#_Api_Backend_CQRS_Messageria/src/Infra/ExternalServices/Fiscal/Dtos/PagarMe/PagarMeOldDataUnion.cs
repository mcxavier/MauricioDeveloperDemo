using System;

using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos.PagarMe
{
    public class PagarMeOldDataUnion
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("processedAt")]
        public DateTime ProcessedAt { get; set; }

        [JsonProperty("operator", NullValueHandling = NullValueHandling.Ignore)]
        public PagarMeOperator PagarMeOperator { get; set; }
    }
}
