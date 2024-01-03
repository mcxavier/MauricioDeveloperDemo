using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Core.SharedKernel;
using Newtonsoft.Json;

namespace Infra.Domains.Topics
{
    public class PublishApplicationOrderException : IRequest<Response>
    {
        public string CancelReason { get; set; }
        public string ExceptionId { get; set; }
        public string FulfillmentId { get; set; }
        public bool HasTreatment { get; set; }
        public PublishApplicationOrder_FullfimentInvoice Invoice {get; set;}
        public List<PublishApplicationOrder_ExceptionItem> Items {get; set;}
        public PublishApplicationOrder_ExceptionRefund Refund {get; set;}
        public string orderId { get; set; }
        public bool RefundProcessed { get; set; }
        public bool ReturnProcessed { get; set; }
        public string TreatmentId { get; set; }
        public string Type { get; set; } // CANCELLATION: Cancelamento total ou parcial de um fulfillment;
                                         // EXCHANGE: Troca de itens de um fulfillment;
                                         // RETURN: Devolução de um ou mais itens de um fulfillment.
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }




}
