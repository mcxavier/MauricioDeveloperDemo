using Core.SharedKernel;
using MediatR;
using System;
using System.Collections.Generic;

namespace Infra.Domains.Topics
{
    public class PublishApplicationFulfillmentStatus : IRequest<Response>
    {
        public List<PublishApplicationFulfillment_Attributes> attributes { get; set; }
        public DateTime deliveredAt { get; set; }
        public string fulfillmentId { get; set; }
        public PublishApplicationFulfillmentGiftMessage giftMessage { get; set; }
        public List<PublishApplicationOrder_FullfimentInvoice> invoices { get; set; }
        public string locationId { get; set; }
        public string orderId { get; set; }
        public PublishApplicationOrder_FullfimentPickup pickup { get; set; }
        public DateTime scheduledDeliveryDate { get; set; }
        public PublishApplicationOrder_ScheduledDeliveryHours scheduledDeliveryHours { get; set; }
        public string scheduledDeliveryPeriod { get; set; }
        public PublishApplicationFulfillmentStatus_Shipment shipment { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class PublishApplicationFulfillmentStatus_Shipment
    {
        public PublishApplicationOrder_Address address { get; set; }
        public decimal amount { get; set; }
        public string carrier { get; set; }
        public decimal cost { get; set; }
        public int daysToDelivery { get; set; }
        public string deliveryDate { get; set; }
        public int discount { get; set; }
        public string method { get; set; }
        public string trackingCode { get; set; }
        public string trackingUrl { get; set; }
        public string type { get; set; }   // SHIPMENT_BY_CARRIER: Envio feito por uma transportadora;
                                           // SHIPMENT_BY_STORE: Envio feito diretemente pela loja.Ex.: Motoboy;
                                           // SHIPMENT_BY_MARKETPLACE: Envio realizado pelo Marketplace
        public List<PublishApplicationFulfillment_Volume> volumes { get; set; }

    }

    public class PublishApplicationFulfillment_Volume
    {
        public PublishApplicationOrder_Height height { get; set; }
        public List<PublishApplicationFulfillment_VolumeItem> items { get; set; }
        public PublishApplicationOrder_Length length { get; set; }
        public int totalQuantity { get; set; }
        public PublishApplicationOrder_Volume volume { get; set; }
        public PublishApplicationOrder_Height weight { get; set; }
        public PublishApplicationOrder_Width width { get; set; }
    }

    public class PublishApplicationFulfillment_VolumeItem
    {
        public int quantity { get; set; }
        public string skuId { get; set; }
    }

    public class PublishApplicationFulfillment_Attributes
    {
        public PublishApplicationFulfillmentAttributesData data { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }

    public class PublishApplicationFulfillmentAttributesData
    {
        public string id { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }

    public class PublishApplicationFulfillmentGiftMessage
    {
        public string format { get; set; }
        public string value { get; set; }
    }
}
