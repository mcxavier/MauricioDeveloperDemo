using System;

using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos
{
    public class LinxUxOldDataUnion
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("processedAt")]
        public DateTime ProcessedAt { get; set; }

        [JsonProperty("operator", NullValueHandling = NullValueHandling.Ignore)]
        public LinxUxOperator LinxUxOperator { get; set; }
    }
}
