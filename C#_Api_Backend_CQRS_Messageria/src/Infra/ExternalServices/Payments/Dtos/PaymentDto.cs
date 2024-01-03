using System.Collections.Generic;

namespace Infra.ExternalServices.Payments.Dtos
{
    public class PaymentDto
    {
        public string Pan { get; set; }
        public long Amount { get; set; }
        public long Installments { get; set; }
        public string Currency { get; set; }
        public string GatewayProvider { get; set; }
        public string Status { get; set; }
        public int PaymentTypeId { get; set; }
        public IList<TransactionDto> Transactions { get; set; }
    }

    public class TransactionDto
    {
        public string Suplier { get; set; }
        public int Installments { get; set; }
        public int Ammount { get; set; }
        public bool IsSuccess { get; set; }
        public string Nsu { get; set; }
        public long AuthorizationCode { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public int? SuplierReturnCode { get; set; }
        public string Status { get; set; }
    }
}