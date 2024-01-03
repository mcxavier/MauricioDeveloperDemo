using System.IO;
using System.Threading.Tasks;

using CoreService.IntegrationsViewModels;
using Infra.ExternalServices.Azure.Configurations;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace Infra.ExternalServices.Azure
{

    public interface IAzureStorageIntegrationServices
    {

        AzureStorageConfig GetStorageConfig();
        public Task RenameFolder(string containerName, string folderName, string newFolderName);
        public CloudBlockBlob SendFileToBlob(string containerName, string blobName, string filePath, string contentType = null);
        public CloudBlockBlob GetFileFromBlob(string containerName, string blobName, string filePath, string contentType = null);
        public CloudBlockBlob SendStreamToBlob(Stream stream, string containerName, string blobName, string contentType = null);
        public CloudBlockBlob GetStreamFromBlob(Stream stream, string containerName, string blobName, string contentType = null);
    }

}