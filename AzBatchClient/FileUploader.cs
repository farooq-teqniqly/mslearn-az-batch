using System.IO;
using System.Threading.Tasks;
using Azure;
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

            try
            {
                await this
                    .containerClient
                    .Container
                    .UploadBlobAsync(
                        Path.GetFileName(localFilePath),
                        File.OpenRead(localFilePath));

                this.logger.LogInformation($"{localFilePath} uploaded to {blobName} in input container.");
            }
            catch (RequestFailedException rfe)
            {
                if (rfe.ErrorCode == "BlobAlreadyExists")
                {
                    this.logger.LogWarning($"{blobName} already exists and will not be overwritten.");
                }
                else
                {
                    throw;
                }
                
            }

            

            return new UploadFileResult {BlobName = blobName};
        }
    }
}
