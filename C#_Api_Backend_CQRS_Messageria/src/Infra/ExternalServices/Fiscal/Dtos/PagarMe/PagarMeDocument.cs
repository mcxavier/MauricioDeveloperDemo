using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos.PagarMe
{

    public class PagarMeDocument
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

    }

}