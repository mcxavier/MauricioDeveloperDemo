using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using CoreService.Infrastructure.Services;
using CoreService.IntegrationsViewModels;


using Microsoft.Extensions.Logging;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Newtonsoft.Json;

using Infra.ExternalServices.Azure.Configurations;
using System.IO;

namespace Infra.ExternalServices.Azure
{

    public class AzureStorageIntegrationServices : IAzureStorageIntegrationServices
    {

        private readonly ILogger<AzureStorageIntegrationServices> _logger;
        private readonly IAzureStorageConfig _config;
        private AzureHelper _azure;

        public AzureStorageIntegrationServices(IAzureStorageConfig config, ILogger<AzureStorageIntegrationServices> logger)
        {
            this._logger = logger;
            this._config = config;
            this._azure = new AzureHelper(config);
        }

        public AzureStorageConfig GetStorageConfig()
        {
            this._logger.LogInformation("Requesting Azure storage config");
            return (AzureStorageConfig) this._config;
        }
        public async Task RenameFolder(string containerName, string folderName, string newFolderName)
        {
            await this._azure.RenameFolder(containerName, folderName, newFolderName);
        }
        public CloudBlockBlob SendFileToBlob(string containerName, string blobName, string filePath, string contentType = null)
        {
            return this._azure.SendFileToBlob(containerName, blobName, filePath, contentType);
        }

        public CloudBlockBlob GetFileFromBlob(string containerName, string blobName, string filePath, string contentType = null)
        {
            return this._azure.GetFileFromBlob(containerName, blobName, filePath, contentType);
        }

        public CloudBlockBlob GetStreamFromBlob(Stream stream, string containerName, string blobName, string contentType = null)
        {
            return this._azure.GetStreamFromBlob(stream, containerName, blobName, contentType);
        }

        public CloudBlockBlob SendStreamToBlob(Stream stream, string containerName, string blobName, string contentType = null)
        {
            return this._azure.SendStreamToBlob(stream, containerName, blobName, contentType);
        }
    }
}