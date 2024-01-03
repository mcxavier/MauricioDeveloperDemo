using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Core.SharedKernel;
using Newtonsoft.Json;

namespace Infra.Domains.Topics
{
    public class PublishApplicationOrderPaymentStatus : IRequest<Response>
    {
        public string OrderId { get; set; }
        public List<PublishApplicationPayment_Payments> Payments { get; set; }
        public string Status { get; set; } // PARTIAL: o pedido possui transações pagas, mas ainda existem transações pendentes
                                           // COMPLETE: o pedido possui todas transações de pagamento aprovadas.Esse estágio
                                           //           significa que o pedido poderá prosseguir para faturamento e expedição
                                           //           (por convenção o Ecommerce deve enviar esse status)
                                           // CANCELED: o pedido possui todas transações de pagamento canceladas.Esse estágio
                                           //           significa que o pedido deve ser cancelado e suas reservas de estoques
                                           //           devem ser desfeitas.

        public DateTime UpdatedAt { get; set; }
    }

    public class PublishApplicationPayment_Payments
    {
        public PublishApplicationOrder_Balance Balance { get; set; }
        public string Currency { get; set; }
        public PublishApplicationOrder_Details Details { get; set; }
        public PublishApplicationPayment_PaymentsFraud Fraud { get; set; }
        public int? PaymentId { get; set; }
        public string Status { get; set; }
    }


    public class PublishApplicationPayment_PaymentsFraud
    {
        public string AnalysisDate { get; set; }
        public decimal? Score { get; set; }
        public string Status { get; set; } // InAnalisys: Indica que o pagamento ainda está em análise de risco
                                           // Approved: Indica que o pagamento passou na análise de fraude
                                           // Refused: Indica que o pagamento foi recusado na análise
        public string Vendor { get; set; }
        public string VendorStatus { get; set; }
    }




}