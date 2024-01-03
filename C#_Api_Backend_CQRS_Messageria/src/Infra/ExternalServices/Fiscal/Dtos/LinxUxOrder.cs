using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos
{

    public class LinxUxOrder
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("pointOfSaleId")]
        public string PointOfSaleId { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("discount")]
        public decimal Discount { get; set; }

        [JsonProperty("placedAt")]
        public DateTime PlacedAt { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        /*
        [JsonProperty("comments")]
        public object[] Comments { get; set; }
        */

        [JsonProperty("billingAddress")]
        public LinxUxBillingAddress BillingAddress { get; set; }

        [JsonProperty("customer")]
        public LinxUxOrderCustomer Customer { get; set; }

        [JsonProperty("items")]
        public LinxUxItem[] Items { get; set; }

        [JsonProperty("payments")]
        public LinxUxPayments[] Payments { get; set; }

        [JsonProperty("totals")]
        public LinxUxTotals LinxUxTotals { get; set; }

        [JsonProperty("fulfillments")]
        public Dictionary<string, LinxUxFulllfilment> Fulfillments { get; set; }


    }

}