using Microsoft.Azure.Batch;

namespace AzBatchClient
{
    public class VirtualMachineConfigurationFactory
    {
        public VirtualMachineConfiguration CreateVirtualMachineConfiguration(ImageReference imageReference)
        {
            return new VirtualMachineConfiguration(
                imageReference, 
                "batch.node.windows amd64");
        }
    }
}