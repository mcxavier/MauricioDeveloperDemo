namespace Infra.ExternalServices.Azure.Configurations
{
    public interface IAzureStorageConfig
    {
        string BaseUrl { get; set; }
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
    }

}