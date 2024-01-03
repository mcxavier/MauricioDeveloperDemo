using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos
{

    public class LinxUxDocument
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

    }

}