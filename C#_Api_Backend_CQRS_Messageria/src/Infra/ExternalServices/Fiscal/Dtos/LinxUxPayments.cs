using System;

using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos
{
    public class LinxUxPayments
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("isCaptured")]
        public bool IsCaptured { get; set; }

        [JsonProperty("billingDate")]
        public DateTime BillingDate { get; set; }

        [JsonProperty("installments")]
        public int Installments { get; set; }

        [JsonProperty("paymentNumber")]
        public int PaymentNumber { get; set; }

        [JsonProperty("transactionNumber")]
        public string TransactionNumber { get; set; }

        //[JsonProperty("extensionAttributes")]
        //public PaymentExtensionAttributes ExtensionAttributes { get; set; }
    }
}
