using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Batch;
using Microsoft.Azure.Batch.Common;
using Microsoft.Extensions.Logging;

namespace AzBatchClient
{
    public class BatchTaskService
    {
        private readonly BatchClient batchClient;
        private readonly ILogger<BatchTaskService> logger;

        public BatchTaskService(BatchClient batchClient, ILogger<BatchTaskService> logger)
        {
            this.batchClient = batchClient;
            this.logger = logger;
        }

        public async Task<List<CloudTask>> ScheduleTasksAsync(
            string jobId,
            List<ResourceFile> resourceFiles,
            string outputContainerSasUrl,
            ApplicationPackageReference packageReference)
        {
            var cloudTasks = new List<CloudTask>();

            for (var i = 0; i < resourceFiles.Count; i++)
            {
                var taskId = $"Task={i}";
                var appPath = $"%AZ_BATCH_APP_PACKAGE_{packageReference.ApplicationId}#{packageReference.Version}%";
                var inputFile = resourceFiles[i].FilePath;
                var outputMediaFile = $"${Path.GetFileNameWithoutExtension(inputFile)}.gif";
                var taskCommandLine = $"cmd /c {appPath}\\ffmpeg-3.4-win64-static\\bin\\ffmpeg.exe -i {inputFile} {outputMediaFile}";

                var cloudTask = new CloudTask(taskId, taskCommandLine)
                {
                    ResourceFiles = new List<ResourceFile> {resourceFiles[i]}
                };

                var outputFiles = new List<OutputFile>();
                var outputContainer = new OutputFileBlobContainerDestination(outputContainerSasUrl);

                var outputFile = new OutputFile(
                    outputMediaFile,
                    new OutputFileDestination(outputContainer),
                    new OutputFileUploadOptions(OutputFileUploadCondition.TaskSuccess));

                outputFiles.Add(outputFile);
                cloudTask.OutputFiles = outputFiles;
                cloudTasks.Add(cloudTask);

                this.logger.LogInformation($"Task {taskId} added to job {jobId}.\nTask command line:\n${taskCommandLine}");
            }

            await this.batchClient.JobOperations.AddTaskAsync(jobId, cloudTasks);

            return cloudTasks;
        }
    }
}
