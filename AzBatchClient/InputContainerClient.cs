using System;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace AzBatchClient
{
    public class InputContainerClient : AppContainerClient
    {
        public InputContainerClient(BlobServiceClient blobServiceClient) 
            : base(blobServiceClient, "input")
        {
        }

        protected override void ConfigureSasPermissions(BlobSasBuilder sasBuilder)
        {
            sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
        }

        protected override Uri GenerateSasUri(
            BlobServiceClient blobServiceClient, 
            BlobSasBuilder sasBuilder,
            string containerName, 
            string blobName)
        {
            return blobServiceClient
                .GetBlobContainerClient(containerName)
                .GetBlobClient(blobName)
                .GenerateSasUri(sasBuilder);
        }
    }
}
