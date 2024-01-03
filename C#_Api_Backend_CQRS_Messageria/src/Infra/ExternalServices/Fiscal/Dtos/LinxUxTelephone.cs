using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos
{
    public  class LinxUxTelephone
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
