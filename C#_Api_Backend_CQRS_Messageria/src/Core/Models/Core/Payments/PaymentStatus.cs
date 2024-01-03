using Core.SeedWork;

namespace Core.Models.Core.Payments
{
    public class PaymentStatus : Enumeration
    {
        public static readonly PaymentStatus Processing = new PaymentStatus(1, "Processing");
        public static readonly PaymentStatus Processed = new PaymentStatus(2, "Processed");
        public static readonly PaymentStatus Failed = new PaymentStatus(3, "Failed");
        public PaymentStatus(int id, string name) : base(id, name) { }
    }

    public enum PaymentStatusEnum
    {
        Processing = 1,
        Processed = 2,
        Failed = 3
    }
}