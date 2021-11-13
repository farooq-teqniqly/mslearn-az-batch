using System;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace AzBatchClient
{
    public class OutputContainerClient : AppContainerClient
    {
        public OutputContainerClient(BlobServiceClient blobServiceClient) 
            : base(blobServiceClient, "output")
        {
            this.ResourceType = "c";
        }

        protected override void ConfigureSasPermissions(BlobSasBuilder sasBuilder)
        {
            sasBuilder.SetPermissions(BlobContainerSasPermissions.Write);
        }

        protected override Uri GenerateSasUri(
            BlobServiceClient blobServiceClient, 
            BlobSasBuilder sasBuilder, 
            string containerName,
            string blobName)
        {
            return blobServiceClient
                .GetBlobContainerClient(containerName)
                .GenerateSasUri(sasBuilder);
        }
    }
}