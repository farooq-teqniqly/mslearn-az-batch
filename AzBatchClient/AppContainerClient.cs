using System;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace AzBatchClient
{
    public abstract class AppContainerClient
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly string containerName;
        public BlobContainerClient Container { get; }
        protected AppContainerClient(BlobServiceClient blobServiceClient, string containerName)
        {
            this.blobServiceClient = blobServiceClient;
            this.containerName = containerName;
            this.Container = blobServiceClient.GetBlobContainerClient(containerName);
        }

        protected abstract void ConfigureSasPermissions(BlobSasBuilder sasBuilder);

        public string GetSasUri(string blobName)
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = this.containerName,
                BlobName = blobName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.Now.AddHours(2)
            };

            this.ConfigureSasPermissions(sasBuilder);

            return this
                .blobServiceClient
                .GetBlobContainerClient(this.containerName)
                .GenerateSasUri(sasBuilder)
                .ToString();
        }
    }
}