using System;
using System.Text.Json.Serialization;

namespace Api.ViewModels
{
    public class OrderViewModel
    {
        [JsonPropertyName("idPedido")]
        public int Id { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("nomeCliente")]
        public string CustomerName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("endereco")]
        public string Address { get; set; }

        [JsonPropertyName("cep")]
        public string CEP { get; set; }

        [JsonPropertyName("cidade")]
        public string City { get; set; }

        [JsonPropertyName("bairro")]
        public string District { get; set; }

        [JsonPropertyName("estado")]
        public string State { get; set; }

        [JsonPropertyName("data")]
        public string Date { get; set; }

        [JsonPropertyName("valorLiquido")]
        public decimal NetValue { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("statusId")]
        public int StatusId { get; set; }

        [JsonPropertyName("telefone")]
        public string Phone { get; set; }

        [JsonPropertyName("cpf")]
        public string CPF { get; set; }

        [JsonPropertyName("rg")]
        public string RG { get; set; }

        [JsonPropertyName("units")]
        public int Units { get; set; }

        [JsonPropertyName("orderType")]
        public int OrderType { get; set; }

        [JsonPropertyName("birthday")]
        public DateTime? BirthDay { get; set; }

        [JsonPropertyName("cardBrand")]
        public int? CardBrand { get; set; }
    }
}
