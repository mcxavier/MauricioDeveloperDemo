namespace Infra.Domains.Topics
{
    public class PublishApplicationAck
    {
        public string entityId { get; set; }  // carrier: carrierId
                                              // carrier-order-notify: orderId
                                              // customer: customerId
                                              // dump-request: topicId
                                              // fulfillment-status: orderId
                                              // location: locationId
                                              // order-cancellation: orderId
                                              // order-payment-status: orderId
                                              // order: orderId
                                              // price: skuId
                                              // product: productId
                                              // sku: skuId
                                              // stock: skuId
                                              // order-treatment: orderId
                                              // order-exception: orderId

        public FeedbackContent feedbackContent { get; set; }
        public string receiptId { get; set; }
        public string referenceMessageId { get; set; }
        public string type { get; set; }  // DISCARD: Indica que a mensagem não foi processada pelo sistema consumidor.
                                          // SUCCESS: Indica que a mensagem foi processada com sucesso pelo sistema consumidor.
                                          // ERROR: Indica que a mensagem foi processada mas algum erro ocorreu no qual não é possível retentar seu processamento.
                                          //        Neste casos recomenda-se além da mensagem de erro incluir na propriedade "additionalInfo" detalhes mais técnicos do
                                          //        problema ocorrido.

    }


    public class FeedbackContent
    {
        public AdditionalInfo additionalInfo { get; set; }
        public string message { get; set; }
    }

    public class AdditionalInfo
    {
        public string description { get; set; }
    }

    public enum AckType
    {
        SUCCESS = 1,
        ERROR = 2,
        DISCARD = 3
    }
}
