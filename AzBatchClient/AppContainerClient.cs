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

        public virtual string ResourceType { get; protected set; } = "b";

        protected AppContainerClient(BlobServiceClient blobServiceClient, string containerName)
        {
            this.blobServiceClient = blobServiceClient;
            this.containerName = containerName;
            this.Container = blobServiceClient.GetBlobContainerClient(containerName);
        }

        protected abstract void ConfigureSasPermissions(BlobSasBuilder sasBuilder);
        protected abstract Uri GenerateSasUri(
            BlobServiceClient blobServiceClient,
            BlobSasBuilder sasBuilder,
            string containerName,
            string blobName);

        public string GetSasUri(string blobName)
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = this.containerName,
                BlobName = blobName,
                Resource = this.ResourceType,
                ExpiresOn = DateTimeOffset.Now.AddHours(2)
            };

            this.ConfigureSasPermissions(sasBuilder);

            return this.GenerateSasUri(
                this.blobServiceClient,
                sasBuilder,
                this.containerName,
                blobName)
                .ToString();
        }

    }
}