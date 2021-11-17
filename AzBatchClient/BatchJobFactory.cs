using System.Threading.Tasks;
using Microsoft.Azure.Batch;

namespace AzBatchClient
{
    public class BatchJobFactory
    {
        private readonly BatchClient batchClient;

        public BatchJobFactory(BatchClient batchClient)
        {
            this.batchClient = batchClient;
        }

        public async Task CreateJobAsync(string jobId, string poolId)
        {
            var job = this.batchClient.JobOperations.CreateJob(
                jobId,
                new PoolInformation {PoolId = poolId});

            await job.CommitAsync();
        }
    }
}
