using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.Batch;
using Microsoft.Extensions.Logging;

namespace AzBatchClient
{
    public class App
    {
        private readonly AppOptions appOptions;
        private readonly BatchPoolFactory poolFactory;
        private readonly FileUploader fileUploader;
        private readonly ResourceFileFactory resourceFileFactory;
        private readonly BatchJobFactory batchJobFactory;
        private readonly InputContainerClient inputContainerClient;
        private readonly OutputContainerClient outputContainerClient;
        private readonly BatchTaskService batchTaskService;
        private readonly ApplicationPackageReferenceFactory applicationPackageReferenceFactory;
        private readonly ILogger<App> logger;

        public App(
            AppOptions appOptions,
            BatchPoolFactory poolFactory,
            FileUploader fileUploader,
            ResourceFileFactory resourceFileFactory,
            BatchJobFactory batchJobFactory,
            InputContainerClient inputContainerClient,
            OutputContainerClient outputContainerClient,
            BatchTaskService batchTaskService,
            ApplicationPackageReferenceFactory applicationPackageReferenceFactory,
            ILogger<App> logger)
        {
            this.appOptions = appOptions;
            this.poolFactory = poolFactory;
            this.fileUploader = fileUploader;
            this.resourceFileFactory = resourceFileFactory;
            this.batchJobFactory = batchJobFactory;
            this.inputContainerClient = inputContainerClient;
            this.outputContainerClient = outputContainerClient;
            this.batchTaskService = batchTaskService;
            this.applicationPackageReferenceFactory = applicationPackageReferenceFactory;
            this.logger = logger;
        }

        public async Task Run()
        {
            var poolId = "WinFFmpegPool";
            var jobId = "WinFFmpegJob";

            this.logger.LogInformation("Creating batch pool...");

            await this.poolFactory.CreateBatchPoolAsync(
                poolId,
                "STANDARD_A1_v2");

            this.logger.LogInformation("Batch pool created.");

            this.logger.LogInformation("Uploading files...");

            var files = Directory.GetFiles(
                Path.Combine(
                    Assembly.GetExecutingAssembly().Location,
                    this.appOptions.InputFolder));

            var resourceFiles = new List<ResourceFile>();

            foreach (var file in files)
            {
                var uploadFileResult = await this.fileUploader.UploadFileAsync(file);
                var blobSasUri = this.inputContainerClient.GetSasUri(uploadFileResult.BlobName);
                var resourceFile = this.resourceFileFactory.CreateResourceFile(blobSasUri, uploadFileResult.BlobName);

                resourceFiles.Add(resourceFile);
            }

            this.logger.LogInformation("Files uploaded...");
            this.logger.LogInformation($"Creating job {jobId} on pool {poolId}");

            await this.batchJobFactory.CreateJobAsync(jobId, poolId);

            this.logger.LogInformation("Scheduling tasks...");

            await this.batchTaskService.ScheduleTasksAsync(
                jobId,
                resourceFiles,
                outputContainerClient.GetSasUri(String.Empty),
                applicationPackageReferenceFactory.CreateApplicationPackageReference());

        }
    }
}
