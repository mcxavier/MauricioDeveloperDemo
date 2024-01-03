using System;

using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos.PagarMe
{
    public class PagarMeVolumes
    {
        [JsonProperty("volume")]
        public int Volume { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("weight")]
        public long Weight { get; set; }

        [JsonProperty("items")]
        public PagarMeItem[] Items { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
