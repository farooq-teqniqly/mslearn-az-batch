using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Batch;
using Microsoft.Extensions.Logging;

namespace AzBatchClient
{
    public class App
    {
        private readonly BatchClient batchClient;
        private readonly ILogger<App> logger;

        public App(BatchClient batchClient, ILogger<App> logger)
        {
            this.batchClient = batchClient;
            this.logger = logger;
        }

        public async Task Run()
        {
            foreach (var app in batchClient.ApplicationOperations.ListApplicationSummaries())
            {
                this.logger.LogInformation($"Installed app: {app.Id}\t{app.Versions[0]}");
            }
        }
    }
}
