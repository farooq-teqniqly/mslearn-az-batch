using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Logging;

namespace AzBatchClient
{
    public class FileUploader
    {
        private readonly InputContainerClient containerClient;
        private readonly ILogger<FileUploader> logger;

        public FileUploader(InputContainerClient containerClient, ILogger<FileUploader> logger)
        {
            this.containerClient = containerClient;
            this.logger = logger;
        }

        public async Task<UploadFileResult> UploadFileAsync(string localFilePath)
        {
            var blobName = Path.GetFileName(localFilePath);

            this.logger.LogInformation($"Uploading {localFilePath} to {blobName} in input container...");
            
            await this
                .containerClient
                .Container
                .UploadBlobAsync(
                    Path.GetFileName(localFilePath), 
                    File.OpenRead(localFilePath));
            
            this.logger.LogInformation($"{localFilePath} uploaded to {blobName} in input container.");

            return new UploadFileResult {BlobName = blobName};
        }
    }
}
