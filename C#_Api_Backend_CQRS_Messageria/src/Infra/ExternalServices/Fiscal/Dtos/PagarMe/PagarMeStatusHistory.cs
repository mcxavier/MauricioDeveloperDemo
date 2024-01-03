using System;

using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos.PagarMe
{
    public class PagarMeStatusHistory
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("processedAt")]
        public DateTime? ProcessedAt { get; set; }

        [JsonProperty("timeInStatus", NullValueHandling = NullValueHandling.Ignore)]
        public long? TimeInStatus { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("oldData")]
        public PagarMeOldDataUnion PagarMeOldData { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("operator", NullValueHandling = NullValueHandling.Ignore)]
        public PagarMeOperator PagarMeOperator { get; set; }
    }
}
