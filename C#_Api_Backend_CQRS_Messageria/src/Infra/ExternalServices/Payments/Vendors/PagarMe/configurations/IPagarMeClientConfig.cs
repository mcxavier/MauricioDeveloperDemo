namespace Infra.ExternalServices.Payments.Vendors.PagarMe.configurations
{
    public interface IPagarMeClientConfig
    {
        public string ApiKey { get; set; }
        public string ClientId { get; set; }
        public bool IsSandBox { get; set; }
    }
}