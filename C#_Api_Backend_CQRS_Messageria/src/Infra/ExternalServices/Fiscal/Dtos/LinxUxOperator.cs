using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos
{
    public class LinxUxOperator
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
