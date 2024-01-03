using Newtonsoft.Json;

namespace Api.ViewModels
{
    public class OrderProductDetailViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nomeProduto")]
        public string Name { get; set; }

        [JsonProperty("tamanho")]
        public string Size { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("cor")]
        public string Color { get; set; }

        [JsonProperty("valorLiquido")]
        public decimal NetValue { get; set; }

        [JsonProperty("qtde")]
        public int Quantity { get; set; }

        [JsonProperty("imagem")]
        public string Image { get; set; }
    }
}
