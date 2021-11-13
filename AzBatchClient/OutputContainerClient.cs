using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace AzBatchClient
{
    public class OutputContainerClient : AppContainerClient
    {
        public OutputContainerClient(BlobServiceClient blobServiceClient) 
            : base(blobServiceClient, "output")
        {
        }

        protected override void ConfigureSasPermissions(BlobSasBuilder sasBuilder)
        {
            sasBuilder.SetPermissions(BlobContainerSasPermissions.Write);
        }
    }
}