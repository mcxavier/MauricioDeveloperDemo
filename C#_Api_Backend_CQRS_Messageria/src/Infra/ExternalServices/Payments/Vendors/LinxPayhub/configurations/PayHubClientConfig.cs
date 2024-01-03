namespace Infra.ExternalServices.Payments.Vendors.LinxPayhub.configurations
{
    public class PayHubClientConfig : IPayHubClientConfig
    {
        public string ApiKey { get; set; }
        public string ClientId { get; set; }
        public bool IsSandBox { get; set; }
    }
}