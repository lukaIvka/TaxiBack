using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureInterface
{
    public class AzureBlobCRUD : Contracts.Blob.IBlob
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly BlobContainerClient blobContainerClient;
        private readonly string containerName;

        public AzureBlobCRUD(string storageUri, string containerName)
        {
            blobServiceClient = new BlobServiceClient(storageUri);
            this.containerName = containerName;
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            blobContainerClient.CreateIfNotExists();
        }

        public async Task<string> GenerateSasUri(string blobName)
        {
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            var dto = new DateTimeOffset(DateTime.Now.AddHours(1));
            var sasUri = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, dto);
            return sasUri?.AbsoluteUri; 
        }

        public async Task<string> UploadBlob(string blobName, Stream blobContent)
        {
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            var res = await blobClient.UploadAsync(blobContent, true);
            return res != null ? $"{blobClient.Uri}/{blobName}" : null;
        }
    }
}
