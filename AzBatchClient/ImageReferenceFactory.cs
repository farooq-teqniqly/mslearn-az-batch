using Microsoft.Azure.Batch;

namespace AzBatchClient
{
    public class ImageReferenceFactory
    {
        public ImageReference CreateImageReference()
        {
            return new ImageReference(
                "WindowsServer",
                "MicrosoftWindowsServer",
                "2012-R2-Datacenter-smalldisk",
                "latest");
        }
    }
}