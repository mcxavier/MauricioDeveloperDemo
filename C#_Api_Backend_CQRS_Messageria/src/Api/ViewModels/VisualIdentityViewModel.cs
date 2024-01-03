using System.Collections.Generic;
using Newtonsoft.Json;

namespace Api.ViewModels
{
    public class VisualIdentityViewModel
    {
        [JsonProperty("zipCode")]
        public string? ZipCode { get; set; }
        [JsonProperty("productsQuant")]
        public int ProductsQuant { get; set; }
        [JsonProperty("freight")]
        public decimal? Freight { get; set; }
        [JsonProperty("additional")]
        public decimal? Additional { get; set; }
        [JsonProperty("mainColor")]
        public string MainColor { get; set; }
        [JsonProperty("secondaryColor")]
        public string SecondaryColor { get; set; }
        [JsonProperty("range")]
        public int? Range { get; set; }
        [JsonProperty("whatsapp")]
        public string? WhatsApp { get; set; }
        [JsonProperty("footerMessage")]
        public string FooterMessage { get; set; }
    }
}
