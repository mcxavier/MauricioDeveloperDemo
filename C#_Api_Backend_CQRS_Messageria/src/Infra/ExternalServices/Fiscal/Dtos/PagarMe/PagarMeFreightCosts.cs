using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos.PagarMe
{

    public class PagarMeFreightCosts
    {

        [JsonProperty("totalTime")]
        public int TotalTime { get; set; }

        [JsonProperty("totalPrice")]
        public int TotalPrice { get; set; }

        [JsonProperty("handlingTime")]
        public int HandlingTime { get; set; }

        [JsonProperty("handlingPrice")]
        public int HandlingPrice { get; set; }

    }

}