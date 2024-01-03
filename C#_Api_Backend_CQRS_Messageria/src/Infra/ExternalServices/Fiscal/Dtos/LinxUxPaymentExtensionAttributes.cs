﻿using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos
{

    public class LinxUxPaymentExtensionAttributes
    {

        [JsonProperty("paymentId")]
        public string PaymentId { get; set; }

        [JsonProperty("lastDigits")]
        public int LastDigits { get; set; }

        [JsonProperty("firstDigits")]
        public int FirstDigits { get; set; }

        [JsonProperty("fulfillmentId")]
        public string[] FulfillmentId { get; set; }

        [JsonProperty("paymentSystem")]
        public int PaymentSystem { get; set; }

    }

}