using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Core.SharedKernel;
using Newtonsoft.Json;


namespace Infra.Domains.Topics
{
    public class PublishApplicationOrderTreatment : IRequest<Response>
    {
        public DateTime CreatedAt { get; set; }
        public string FulfillmentId { get; set; }
        public List<PublishApplicationOrderTreatment_Item> Items { get; set; }
        public PublishApplicationOrderTreatment_Operador Operador { get; set; }
        public string OrderId { get; set; }
        public bool Processed { get; set; }
        public string TreatmentId { get; set; }
        public string Type { get; set; }  // STOCKOUT: Indica que houve uma quebra e estoque
                                          // DELIVERY_FAILED: Entrega com falha na entrega
                                          // DELIVERY_LOST: Indica que houve a perda do pacote na entrega
                                          // DELIVERY_NOT_COLLECTED: Entrega não coletada(onde a transportadora não foi buscar o pedido na loja ou no CD)
                                          // PICKUP_FAILED: Cliente não retirou o pedido em loja ou CD
                                          // RETURN_REQUEST: Solicitação para iniciar a devolução
                                          // EXCHANGE_REQUEST: Solicitação de troca de produto(s)
                                          // CANCELLATION_REQUEST: Solicitação de cancelamento
                                          // RELOCATION_REQUEST: Solicitação de realocação de um pedido
        public DateTime UpdatedAt { get; set; }
    }


    public class PublishApplicationOrderTreatment_Item
    {
        public Int32 Quantity { get; set; }
        public string Reason { get; set; }
        public string SkuId { get; set; }
        public string StockType { get; set; }
    }


    public class PublishApplicationOrderTreatment_Operador
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

}
