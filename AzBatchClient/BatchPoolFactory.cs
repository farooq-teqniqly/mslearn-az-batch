using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Batch;

namespace AzBatchClient
{
    public class BatchPoolFactory
    {
        private readonly ImageReferenceFactory imageReferenceFactory;
        private readonly VirtualMachineConfigurationFactory virtualMachineConfigurationFactory;
        private readonly ApplicationPackageReferenceFactory applicationPackageReferenceFactory;
        private readonly BatchClient batchClient;
        private readonly AzureBatchOptions options;

        public BatchPoolFactory(
            AzureBatchOptions options,
            ImageReferenceFactory imageReferenceFactory,
            VirtualMachineConfigurationFactory virtualMachineConfigurationFactory,
            ApplicationPackageReferenceFactory applicationPackageReferenceFactory,
            BatchClient batchClient)
        {
            this.imageReferenceFactory = imageReferenceFactory;
            this.virtualMachineConfigurationFactory = virtualMachineConfigurationFactory;
            this.applicationPackageReferenceFactory = applicationPackageReferenceFactory;
            this.batchClient = batchClient;
            this.options = options;
        }
        public async Task CreateBatchPoolAsync(
            string poolId,
            string virtualMachineSize,
            byte targetDedicatedComputeNodes)
        {
            var imageReference = this.imageReferenceFactory.CreateImageReference();
            
            var virtualMachineConfiguration = this.virtualMachineConfigurationFactory.CreateVirtualMachineConfiguration(
                imageReference);

            var pool = this.batchClient.PoolOperations.CreatePool(
                poolId,
                virtualMachineSize,
                virtualMachineConfiguration,
                targetDedicatedComputeNodes,
                this.options.FFMpegPool.NodeCount);

            pool.ApplicationPackageReferences = new List<ApplicationPackageReference>
            {
                this.applicationPackageReferenceFactory.CreateApplicationPackageReference()
            };

            await pool.CommitAsync();

        }
    }
}
