namespace Infra.ExternalServices.Payments.Dtos
{
    public class PaymentCardDto
    {
        public string Holder { get; set; }
        public string Number { get; set; }
        public string SecurityCode { get; set; }
        public byte Installments { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Brand { get; set; }
    }
}