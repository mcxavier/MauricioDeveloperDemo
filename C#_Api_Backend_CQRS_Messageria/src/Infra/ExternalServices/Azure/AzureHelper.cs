using System;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

using CoreService.Infrastructure.Services;
using CoreService.IntegrationsViewModels;

using Infra.ExternalServices.Azure.Configurations;
using Microsoft.Azure.Storage.DataMovement;

namespace Infra.ExternalServices.Azure
{

    public class AzureHelper
    {
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _blobClient;
        public string urlBlobStorage;
        private readonly IAzureStorageConfig azureConfig;

        public AzureHelper(IAzureStorageConfig azureStorageConfig)
        {
            this.azureConfig = azureStorageConfig;
            _storageAccount = CloudStorageAccount.Parse(azureConfig.ConnectionString);
            urlBlobStorage = _storageAccount.BlobStorageUri.PrimaryUri.AbsoluteUri;
        }
        public async Task RenameFolder(string containerName, string dirName, string newDirName)
        {
            var container = GetOrAddContainer(containerName);
            await CopyBlob(container, dirName, newDirName);
            DeleteDirectoryIfExists(container, dirName);
        }

        private static async Task CopyBlobDirectoryAsync(CloudBlobDirectory sourceBlob, CloudBlobDirectory destBlob)
        {
            try
            {
                CopyDirectoryOptions options = new CopyDirectoryOptions();
                options.Recursive = true;
                await TransferManager.CopyDirectoryAsync(sourceBlob, destBlob, true, options, null);
                //await TransferManager.CopyAsync(sourceBlob, destBlob, true);
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw e;
            }
            finally
            {
              
            }
        }

        public async Task CopyBlob(CloudBlobContainer container, string sourceName, string destName)
        {
            try
            {
                CloudBlobDirectory sourceBlob = null, destBlob = null;
                sourceBlob = GetBlobDirectory(container, sourceName);
                destBlob = GetBlobDirectory(container, destName);
                await CopyBlobDirectoryAsync(sourceBlob, destBlob);
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw e;
            }
            finally
            {

            }
        }

        public CloudBlobContainer GetOrAddContainer(string containerName)
        {
            if (_blobClient == null)
                _blobClient = _storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
            CreateContainerIfNotExists(container);
            return container;
        }

        public CloudBlockBlob SendFileToBlob(string containerName, string blobName, string filePath, string contentType = null)
        {
            var container = GetOrAddContainer(containerName);
            CloudBlockBlob blockBlob = GetBlockBlob(container, blobName, contentType);
            blockBlob.UploadFromFile(filePath);
            return blockBlob;
        }

        public CloudBlockBlob GetFileFromBlob( string containerName, string blobName, string filePath, string contentType = null)
        {
            var container = GetOrAddContainer(containerName);
            CloudBlockBlob blockBlob = GetBlockBlob(container, blobName, contentType);
            blockBlob.DownloadToFile(filePath,FileMode.OpenOrCreate);
            return blockBlob;
        }
        public CloudBlockBlob GetStreamFromBlob(Stream stream, string containerName, string blobName, string contentType = null)
        {
            var container = GetOrAddContainer(containerName);
            CloudBlockBlob blockBlob = GetBlockBlob(container, blobName, contentType);
            blockBlob.DownloadToStream(stream);
            return blockBlob;
        }
        public CloudBlockBlob SendStreamToBlob(Stream stream, string containerName, string blobName, string contentType = null)
        {
            var container = GetOrAddContainer(containerName);
            CloudBlockBlob blockBlob = GetBlockBlob(container, blobName, contentType);
            blockBlob.UploadFromStream(stream);
            return blockBlob;
        }

        public CloudBlockBlob GetBlockBlob(string containerName, string blobName, string contentType = null)
        {
            var container = GetOrAddContainer(containerName);
            return GetBlockBlob(container, blobName, contentType);
        }
        public CloudBlockBlob GetBlockBlob(CloudBlobContainer container, string blobName, string contentType)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            blockBlob.Properties.ContentType = contentType ?? blockBlob.Properties.ContentType;
            return blockBlob;
        }

        public CloudBlockBlob GetBlockBlob(CloudBlobContainer container, string blobName)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            return blockBlob;
        }
        public CloudBlobDirectory GetBlobDirectory(CloudBlobContainer container, string dirName)
        {
            CloudBlobDirectory dirBlob = container.GetDirectoryReference(dirName);
            return dirBlob;
        }

        public void CreateContainerIfNotExists(CloudBlobContainer cloudBlobContainer)
        {
            if (!cloudBlobContainer.Exists())
                cloudBlobContainer.CreateIfNotExists();
        }

        public bool DeleteFileIfExists(CloudBlobContainer cloudBlobContainer, string blobName)
        {
            try
            {
                CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);
                if (blockBlob != null && blockBlob.Exists())
                {
                    try { blockBlob.DeleteIfExists(); } catch (Exception) { }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteDirectoryIfExists(CloudBlobContainer cloudBlobContainer, string directoryName)
        {
            try
            {
                foreach (IListBlobItem blob in cloudBlobContainer.GetDirectoryReference(directoryName).ListBlobs(true))
                {
                    if (blob.GetType() == typeof(CloudBlob) || blob.GetType().BaseType == typeof(CloudBlob))
                    {
                        ((CloudBlob)blob).DeleteIfExists();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
