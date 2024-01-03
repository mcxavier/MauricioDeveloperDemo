using MediatR;
using System;
using System.Collections.Generic;
using Core.SharedKernel;
using Newtonsoft.Json;

namespace Infra.QueryCommands.Commands.Topics
{
    public class PublishApplicationOrder : IRequest<Response>
    {
        public string brandId { get; set; }
        public string channelType { get; set; }
        public string companyId { get; set; }
        public string device { get; set; }
        public decimal discount { get; set; }
        public string orderId { get; set; }
        public string paymentStatus { get; set; }
        public string placedAt { get; set; }
        public string pointOfSaleId { get; set; }
        public string status { get; set; }
        public decimal total { get; set; }
        public List<PublishApplicationOrder_Attributes> attributes { get; set; }
        public PublishApplicationOrder_BillingAddress billingAddress { get; set; }
        public List<PublishApplicationOrder_Payment> payments { get; set; }
        public List<PublishApplicationOrder_Fulfillment> fulfillments { get; set; }
        public List<PublishApplicationOrder_Item> items { get; set; }
        public PublishApplicationOrder_Customer customer { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PublishApplicationOrder_SalesAgent salesAgent { get; set; }

        // public PublishApplicationOrder_Details details { get; set; }
    }
}

public class PublishApplicationOrder_Details
{
    public string group { get; set; }
    public string name { get; set; }
    public string referenceOrderId { get; set; }
}

public class PublishApplicationOrder_BillingAddress
{
    public string addressId { get; set; }
    public string addressLine1 { get; set; }
    public string addressLine2 { get; set; }
    public string addressName { get; set; }
    public string city { get; set; }
    public string contactName { get; set; }
    public string contactPhone { get; set; }
    public string country { get; set; }
    public bool @default { get; set; }
    public int latitude { get; set; }
    public int longitude { get; set; }
    public string neighbourhood { get; set; }
    public string notes { get; set; }
    public string number { get; set; }
    public string state { get; set; }
    public string zipCode { get; set; }
}

public class PublishApplicationOrder_DocumentNumber
{
    public string cpf { get; set; }
    public string rg { get; set; }
}

public class PublishApplicationOrder_Customer
{
    public bool acceptsMarketing { get; set; }
    public string createdAt { get; set; }
    //public List<PublishApplicationOrder_CustomerAttributes> attributes { get; set; }
    public string customerId { get; set; }
    public string dateOfBirth { get; set; }
    public PublishApplicationOrder_DocumentNumber documentNumber { get; set; }
    public string email { get; set; }
    public string firstName { get; set; }
    public string gender { get; set; } // [ MALE, FEMALE, NOT INFORMED ]
    public string lastName { get; set; }
    public string mobilePhone { get; set; }
    public string notes { get; set; }
    public string type { get; set; } //  PERSON: Pessoa física;  COMPANY:Pessoa Jurídica/Empresa;
    public string updatedAt { get; set; }
}

public class PublishApplicationOrder_SalesAgent
{
    public decimal amount { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public PublishApplicationOrder_DocumentNumber documentNumber { get; set; }
}

public class PublishApplicationOrder_Attributes
{
    public PublishApplicationOrder_CustomerAttributesData data { get; set; }
    public string id { get; set; }
    public string name { get; set; }
}

public class PublishApplicationOrder_CustomerAttributesData
{
    public string id { get; set; }
    public string type { get; set; }
    public string value { get; set; }
}

public class PublishApplicationOrder_ScheduledDeliveryHours
{
    public string from { get; set; }
    public string to { get; set; }
}

public class PublishApplicationOrder_FullItem
{
    public string name { get; set; }
    public int quantity { get; set; }
    public string skuId { get; set; }
}

public class PublishApplicationOrder_Item
{
    public decimal discount { get; set; }
    public bool giftPackage { get; set; }
    public bool giftProduct { get; set; }
    public decimal price { get; set; }
    public int quantity { get; set; }
    public PublishApplicationOrder_Sku sku { get; set; }
    public decimal subTotal { get; set; }
}

public class PublishApplicationOrder_Address
{
    public string addressId { get; set; }
    public string addressLine1 { get; set; }
    public string addressLine2 { get; set; }
    public string addressName { get; set; }
    public string city { get; set; }
    public string contactName { get; set; }
    public string contactPhone { get; set; }
    public string country { get; set; }
    public bool @default { get; set; }
    public int latitude { get; set; }
    public int longitude { get; set; }
    public string neighbourhood { get; set; }
    public string notes { get; set; }
    public string number { get; set; }
    public string state { get; set; }
    public string zipCode { get; set; }
}

public class PublishApplicationOrder_Shipment
{
    public PublishApplicationOrder_Address address { get; set; }
    public decimal amount { get; set; }
    public string carrier { get; set; }
    public decimal cost { get; set; }
    public int daysToDelivery { get; set; }
    public string deliveryDate { get; set; }
    public int discount { get; set; }
    public string method { get; set; }
    //public string type { get; set; }   // SHIPMENT_BY_CARRIER: Envio feito por uma transportadora;
    //                                   // SHIPMENT_BY_STORE: Envio feito diretemente pela loja.Ex.: Motoboy;
    //                                   // SHIPMENT_BY_MARKETPLACE: Envio realizado pelo Marketplace
}

public class PublishApplicationOrder_FulfillmentGiftMessage
{
    public string format { get; set; }
    public string value { get; set; }

}

public class PublishApplicationOrder_FulfillmentInvoice
{
    public DateTime createdAt { get; set; }
    public string id { get; set; }
    public string issuedAt { get; set; }
    public PublishApplicationOrder_FulfillmentInvoiceNfe nfe { get; set; }
    public string number { get; set; }
    public DateTime processedAt { get; set; }
    public string type { get; set; }
    public DateTime updatedAt { get; set; }
}

public class PublishApplicationOrder_FulfillmentInvoiceNfe
{
    public string authorizationProtocol { get; set; }
    public string cfop { get; set; }
    public string eletronicKey { get; set; }
    public string invoicePdf { get; set; }
    public string invoiceXml { get; set; }
    public string invoiceXmlContent { get; set; }
    public string observation { get; set; }
    public string operation { get; set; }
    public string serialNumber { get; set; }
    public string taxPdf { get; set; }
}

public class PublishApplicationOrder_Fulfillment
{
    public string fulfillmentId { get; set; }
    public List<PublishApplicationOrder_FullItem> items { get; set; }
    public string locationId { get; set; }
    //public PublishApplicationOrder_FullfimentPickup pickup { get; set; }
    public string scheduledDeliveryDate { get; set; }
    public PublishApplicationOrder_ScheduledDeliveryHours scheduledDeliveryHours { get; set; }
    public string scheduledDeliveryPeriod { get; set; }
    //public PublishApplicationOrder_FullfimentSellerBroker sellerBroker { get; set; }
    public PublishApplicationOrder_Shipment shipment { get; set; }
    public string status { get; set; } // PENDING: Aguardando pagamento
                                       // WAITING: Aguardando início da separação(pedido já pago)
                                       // PICKING: Entrega em separação
                                       // CHECKED: Entrega conferida
                                       // BILLING: Liberado para faturamento
                                       // BILLING_CONFIRMATION: Confirmado no PDV.Pronto para faturamento
                                       // INVOICED: Faturado
                                       // SHIPPED: Enviado
                                       // PICKUP_READY: Pronto para retirada
                                       // DELIVERED: Entregue
                                       // CANCELED: Cancelado
    public string type { get; set; } // PICKUP: Entrega que será retirado em loja ou CD;
                                     // SHIPMENT: Envio feito por transportadora
}

public class PublishApplicationOrder_FullfimentSellerBroker
{
    public string documentNumber { get; set; }
    public string sellerId { get; set; }
}

public class PublishApplicationOrder_FullfimentPickup
{
    public decimal amount { get; set; }
    public int daysToPickup { get; set; }
    public PublishApplicationOrder_FullfimentPickupPeson personToPickup { get; set; }
    public string type { get; set; } // SHIPTO_INLOCKER: Retirada na loja na qual depende do faturamento e entrega do(s) produto(s) por outra filial;
                                     // STANDARD: Retirada na Loja com faturamento pela própria filial onde será feita a retirada.
}

public class PublishApplicationOrder_FullfimentPickupPeson
{
    public string documentNumber { get; set; }
    public string documentType { get; set; }
    public string name { get; set; }
}

public class PublishApplicationOrder_FullfimentInvoice
{
    public string id { get; set; }
    public string issuedAt { get; set; }
    public PublishApplicationOrder_Fullfimentnfe nfe { get; set; }
    public string number { get; set; }
    public DateTime processedAt { get; set; }
    public string type { get; set; }
    public DateTime updatedAt { get; set; }
    public DateTime createdAt { get; set; }
}

public class PublishApplicationOrder_ExceptionItem
{
    public Decimal price { get; set; }
    public Int32 quantity { get; set; }
    public Decimal shippingPrice { get; set; }
    public string skuId { get; set; }
}

public class PublishApplicationOrder_ExceptionRefund
{
    public Decimal freightRefundValue { get; set; }
    public PublishApplicationOrder_ExceptionRefundGiftCard giftCard { get; set; }
    public Decimal giftPackageRefundValue { get; set; }
    public Decimal itemsRefundValue { get; set; }
    public List<string> refundItemStatus { get; set; }
    public string refundType { get; set; } // GIFTCARD: Vale presente;
                                           // CUSTOMERCREDIT: Vale cliente;
                                           // PAYMENT_VOID: Estorno do pagamento;
                                           // OFFLINE_REFUND: Devolução do dinheiro.
}

public class PublishApplicationOrder_ExceptionRefundGiftCard
{
    public string code { get; set; }
    public DateTime createdAt { get; set; }
    public PublishApplicationOrder_DocumentNumber documentNumber { get; set; }
    public DateTime expiresAt { get; set; }
    public string source { get; set; }
}

public class PublishApplicationOrder_Fullfimentnfe
{
    public string authorizationProtocol { get; set; }
    public string cfop { get; set; }
    public string eletronicKey { get; set; }
    public string invoicePdf { get; set; }
    public string invoiceXml { get; set; }
    public string invoiceXmlContent { get; set; }
    public string observation { get; set; }
    public string operation { get; set; }
    public string serialNumber { get; set; }
    public string taxPdf { get; set; }
}


public class PublishApplicationOrder_Volume
{
    public string unit { get; set; }
    public int value { get; set; }
}

public class PublishApplicationOrder_Height
{
    public string unit { get; set; }
    public int value { get; set; }
}

public class PublishApplicationOrder_Length
{
    public string unit { get; set; }
    public int value { get; set; }
}

public class PublishApplicationOrder_Weight
{
    public string unit { get; set; }
    public int value { get; set; }
}

public class PublishApplicationOrder_Width
{
    public string unit { get; set; }
    public int value { get; set; }
}

public class PublishApplicationOrder_Sku
{
    public PublishApplicationOrder_Height height { get; set; }
    public PublishApplicationOrder_Length length { get; set; }
    public bool requiresShipping { get; set; }
    public string sku { get; set; }
    public string skuId { get; set; }
    public string title { get; set; }
    public string updatedAt { get; set; }
    public PublishApplicationOrder_Weight weight { get; set; }
    public PublishApplicationOrder_Width width { get; set; }
}

public class PublishApplicationOrder_Due
{
    public decimal amount { get; set; }
    public string applicationId { get; set; }
}

public class PublishApplicationOrder_Paid
{
    public decimal amount { get; set; }
    public string applicationId { get; set; }
    public string date { get; set; }
}

public class PublishApplicationOrder_Canceled
{
    public decimal amount { get; set; }
    public string applicationId { get; set; }
    public string date { get; set; }
    public string reason { get; set; }
}

public class PublishApplicationOrder_Balance
{
    public PublishApplicationOrder_Due due { get; set; }
    public PublishApplicationOrder_Paid paid { get; set; }
    //public PublishApplicationOrder_Paid? canceled { get; set; }
}

public class PublishApplicationOrder_Issuer
{
    //public string description { get; set; }
    public string standard { get; set; }
    //public string custom { get; set; }
}

public class PublishApplicationOrder_CreditCard
{
    public string acquirer { get; set; }
    public String authorizationDate { get; set; }
    public string authorizationNumber { get; set; }
    public string capturedDate { get; set; }
    public decimal installmentAmount { get; set; }
    public int installments { get; set; }
    public decimal interestTotal { get; set; }
    public PublishApplicationOrder_Issuer issuer { get; set; }
    public string nsu { get; set; }
    public string numberBin { get; set; }
}

public class PublishApplicationOrder_Trasfer
{
    public string account { get; set; }
    public string acquirer { get; set; }
    public string agency { get; set; }
    public string bankCode { get; set; }
}

public class PublishApplicationOrder_PaymentDetails
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public PublishApplicationOrder_CreditCard creditCard { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public PublishApplicationOrder_Custom custom { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public PublishApplicationOrder_CustomerCredit customerCredit { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public PublishApplicationOrder_DebitCard debitCard { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public PublishApplicationOrder_Deposit deposit { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public PublishApplicationOrder_GiftCard giftCard { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public PublishApplicationOrder_Pix pix { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public PublishApplicationOrder_Slip slip { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public PublishApplicationOrder_Trasfer transfer { get; set; }

    public string provider { get; set; } // CIELO, MOIP, MERCADOPAGO, AMEX, REDE, REDEKOMERCI, PAYU, BB, BRADESCO, ITAU, STELO, GOPAGUE, PAGARME, PAGSEGURO, BRASPAG, UNIBANCO, BANRISUL, MUNDIPAGG, BCASH, PAYPAL, REDEPAY, VIAVAREJO, SAFE2PAY, PAYHUB 
    public string type { get; set; }
}

public class PublishApplicationOrder_Transfer
{
    public string account { get; set; }
    public string acquirer { get; set; }
    public string agency { get; set; }
    public string bankCode { get; set; }
}

public class PublishApplicationOrder_Slip
{
    public string acquirer { get; set; }
    public string expirationDate { get; set; }
    public string reconciliationNumber { get; set; }
}

public class PublishApplicationOrder_Pix
{
    public string acquirer { get; set; }
    public string bankCode { get; set; }
    public string transactionId { get; set; }
}

public class PublishApplicationOrder_GiftCard
{
    public string code { get; set; }
}

public class PublishApplicationOrder_Deposit
{
    public string acquirer { get; set; }
    public string number { get; set; }
}

public class PublishApplicationOrder_DebitCard
{
    public string acquirer { get; set; }
    public string authorizationNumber { get; set; }
    public PublishApplicationOrder_Issuer issuer { get; set; }
    public string nsu { get; set; }
}

public class PublishApplicationOrder_CustomerCredit
{
    public string acquirer { get; set; }
    public decimal billingPlan { get; set; }
    public string code { get; set; }
}

public class PublishApplicationOrder_Custom
{
    public string acquirer { get; set; }
    public string extensionAttributes { get; set; }
    public int installments { get; set; }
    public PublishApplicationOrder_Issuer issuer { get; set; }
    public string nsu { get; set; }
    public string numberBin { get; set; }
}

public class PublishApplicationOrder_Payment
{
    public PublishApplicationOrder_Balance balance { get; set; }
    public string currency { get; set; }
    public PublishApplicationOrder_PaymentDetails details { get; set; }
    public string paymentId { get; set; }
    public string status { get; set; }
}