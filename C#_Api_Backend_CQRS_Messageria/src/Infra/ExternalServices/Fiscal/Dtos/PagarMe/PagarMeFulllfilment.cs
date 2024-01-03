using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Infra.ExternalServices.Fiscal.Dtos.PagarMe
{
    public class PagarMeFulllfilment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("ownership")]
        public string Ownership { get; set; }

        [JsonProperty("shipment")]
        public PagarMeShipment Shipment { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("billingDeadlineAt")]
        public DateTime BillingDeadlineAt { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("freightCosts")]
        public PagarMeFreightCosts FreightCosts { get; set; }

        [JsonProperty("shippingAmount")]
        public decimal ShippingAmount { get; set; }

        [JsonProperty("reservationId")]
        public string ReservationId { get; set; }

        [JsonProperty("processedAt")]
        public DateTime ProcessedAt { get; set; }

        [JsonProperty("enablePrePicking")]
        public bool EnablePrePicking { get; set; }

        [JsonProperty("prePickingFinished")]
        public bool PrePickingFinished { get; set; }

        [JsonProperty("locationType")]
        public string LocationType { get; set; }

        [JsonProperty("totals")]
        public PagarMeTotals Totals { get; set; }

        [JsonProperty("items")]
        public Dictionary<string, PagarMeItem> Items { get; set; }

        [JsonProperty("presale")]
        public bool Presale { get; set; }

        [JsonProperty("enablePicking")]
        public bool EnablePicking { get; set; }

        [JsonProperty("enableShipping")]
        public bool EnableShipping { get; set; }

        [JsonProperty("enableBilling")]
        public bool EnableBilling { get; set; }

        [JsonProperty("deadlineAt")]
        public DateTime DeadlineAt { get; set; }

        //[JsonProperty("invoices")]
        //public object[] Invoices { get; set; }

        [JsonProperty("volumes")]
        public PagarMeVolumes[] Volumes { get; set; }

        [JsonProperty("exceptions")]
        public object[] Exceptions { get; set; }

        [JsonProperty("treatments")]
        public object[] Treatments { get; set; }

        [JsonProperty("statusHistory")]
        public PagarMeStatusHistory[] StatusHistory { get; set; }

        [JsonProperty("transportationHistory")]
        public object[] TransportationHistory { get; set; }

        [JsonProperty("comments")]
        public object[] Comments { get; set; }
    }
}
