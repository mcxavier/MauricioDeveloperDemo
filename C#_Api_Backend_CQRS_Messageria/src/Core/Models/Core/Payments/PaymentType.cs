using Core.SeedWork;

namespace Core.Models.Core.Payments
{
    public class PaymentType : Enumeration
    {
        public bool IsActive { get; set; }
        public PaymentType(int id, string name, bool isActive) : base(id, name) { this.IsActive = isActive; }

        public static readonly PaymentType CreditCard = new PaymentType(1, "Cartão de crédito", true);

        public static readonly PaymentType DebitCard = new PaymentType(2, "Cartão de Débito", false);

        public static readonly PaymentType PaymentSlip = new PaymentType(3, "Boleto", false);

        public static readonly PaymentType Pix = new PaymentType(4, "Pix", true);
    }

    public enum PaymentTypeEnum
    {
        CreditCard = 1,
        DebitCard = 2,
        PaymentSlip = 3,
        Pix = 4
    }
}