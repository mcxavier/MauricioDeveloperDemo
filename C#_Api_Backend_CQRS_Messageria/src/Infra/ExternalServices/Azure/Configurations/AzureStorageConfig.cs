

namespace Infra.ExternalServices.Azure.Configurations
{
    public class AzureStorageConfig : IAzureStorageConfig
    {
        public string BaseUrl { get; set; }
        public string BaseBlobUrl { get; set; }
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
    }
}