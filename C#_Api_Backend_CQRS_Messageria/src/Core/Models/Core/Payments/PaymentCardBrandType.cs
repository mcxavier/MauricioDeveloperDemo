using Core.SeedWork;

namespace Core.Models.Core.Payments
{
    public class PaymentCardBrandType : Enumeration
    {
        public PaymentCardBrandType(int id, string name) : base(id, name) { }

        public static readonly PaymentCardBrandType Visa = new PaymentCardBrandType(1, "Visa");
        public static readonly PaymentCardBrandType Mastercard = new PaymentCardBrandType(2, "Mastercard");
        public static readonly PaymentCardBrandType AmericanExpress = new PaymentCardBrandType(3, "AmericanExpress");
        public static readonly PaymentCardBrandType Jcb = new PaymentCardBrandType(4, "Jcb");
        public static readonly PaymentCardBrandType Discover = new PaymentCardBrandType(5, "Discover");
        public static readonly PaymentCardBrandType Uknown = new PaymentCardBrandType(6, "Uknown");
    }

    public enum CardBrandEnum
    {
        Visa = 1,
        Mastercard = 2,
        AmericanExpress = 3,
        Jcb = 4,
        Discover = 5,
        Uknown = 6
    }
}