using Microsoft.Azure.Batch;

namespace AzBatchClient
{
    public class ResourceFileFactory
    {
        public ResourceFile CreateResourceFile(string blobSasUri, string blobName)
        {
            return ResourceFile.FromUrl(blobSasUri, blobName);
        }
    }
}