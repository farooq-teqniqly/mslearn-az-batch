using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Batch;
using Microsoft.Azure.Batch.Common;
using Microsoft.Extensions.Logging;

namespace AzBatchClient
{
    public class BatchPoolFactory
    {
        private readonly ImageReferenceFactory imageReferenceFactory;
        private readonly VirtualMachineConfigurationFactory virtualMachineConfigurationFactory;
        private readonly ApplicationPackageReferenceFactory applicationPackageReferenceFactory;
        private readonly BatchClient batchClient;
        private readonly ILogger<BatchPoolFactory> logger;
        private readonly AzureBatchOptions options;

        public BatchPoolFactory(
            AzureBatchOptions options,
            ImageReferenceFactory imageReferenceFactory,
            VirtualMachineConfigurationFactory virtualMachineConfigurationFactory,
            ApplicationPackageReferenceFactory applicationPackageReferenceFactory,
            BatchClient batchClient,
            ILogger<BatchPoolFactory> logger)
        {
            this.imageReferenceFactory = imageReferenceFactory;
            this.virtualMachineConfigurationFactory = virtualMachineConfigurationFactory;
            this.applicationPackageReferenceFactory = applicationPackageReferenceFactory;
            this.batchClient = batchClient;
            this.logger = logger;
            this.options = options;
        }
        public async Task CreateBatchPoolAsync(
            string poolId,
            string virtualMachineSize)
        {
            var imageReference = this.imageReferenceFactory.CreateImageReference();
            
            var virtualMachineConfiguration = this.virtualMachineConfigurationFactory.CreateVirtualMachineConfiguration(
                imageReference);

            try
            {
                var pool = this.batchClient.PoolOperations.CreatePool(
                    poolId,
                    virtualMachineSize,
                    virtualMachineConfiguration,
                    this.options.FFMpegPool.NodeCount);

                pool.ApplicationPackageReferences = new List<ApplicationPackageReference>
                {
                    this.applicationPackageReferenceFactory.CreateApplicationPackageReference()
                };

                await pool.CommitAsync();
            }
            catch (BatchException be)
            {
                if (be.RequestInformation?.BatchError?.Code == BatchErrorCodeStrings.PoolExists)
                {
                    this.logger.LogInformation($"Batch pool {poolId} exists.");
                }
                else
                {
                    throw;
                }
            }

        }
    }
}
