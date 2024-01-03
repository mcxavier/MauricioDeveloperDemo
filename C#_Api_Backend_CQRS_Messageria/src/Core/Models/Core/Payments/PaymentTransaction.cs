using Core.SeedWork;

namespace Core.Models.Core.Payments
{
    public class PaymentTransaction : EntityWithMetadata<int>
    {
        public string Suplier { get; set; }
        public int Installments { get; set; }
        public int Ammount { get; set; }
        public bool IsSuccess { get; set; }
        public string Nsu { get; set; }
        public long? AuthorizationCode { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public int? SuplierReturnCode { get; set; }
        public PaymentTransactionSupplierCodeType SuplierReturn { get; set; }
        public int? StatusId { get; set; }
        public PaymentTransactionStatus Status { get; set; }
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
    }
}