using Core.SeedWork;

namespace Core.Models.Core.Payments
{
    public class PaymentTransactionStatus : Enumeration
    {

        public static readonly PaymentTransactionStatus Authorized = new PaymentTransactionStatus(1, "Authorized");
        public static readonly PaymentTransactionStatus PreAuthorized = new PaymentTransactionStatus(2, "PreAuthorized");
        public static readonly PaymentTransactionStatus Captured = new PaymentTransactionStatus(3, "Captured");
        public static readonly PaymentTransactionStatus Canceled = new PaymentTransactionStatus(4, "Canceled");
        public static readonly PaymentTransactionStatus NotAuthorized = new PaymentTransactionStatus(5, "NotAuthorized");
        public static readonly PaymentTransactionStatus Reject = new PaymentTransactionStatus(6, "Reject");
        public static readonly PaymentTransactionStatus UnderInvestigation = new PaymentTransactionStatus(7, "UnderInvestigation");
        public static readonly PaymentTransactionStatus Unknown = new PaymentTransactionStatus(8, "Unknown");
        public static readonly PaymentTransactionStatus Release = new PaymentTransactionStatus(9, "Release");

        public PaymentTransactionStatus(int id, string name) : base(id, name) { }
    }

}