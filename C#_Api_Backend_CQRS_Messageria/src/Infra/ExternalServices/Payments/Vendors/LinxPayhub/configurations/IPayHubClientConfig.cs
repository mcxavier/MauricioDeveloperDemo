namespace Infra.ExternalServices.Payments.Vendors.LinxPayhub.configurations
{
    public interface IPayHubClientConfig
    {
        public string ApiKey { get; set; }
        public string ClientId { get; set; }
        public bool IsSandBox { get; set; }
    }
}