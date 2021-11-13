using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AzBatchClient
{
    public class App
    {
        private readonly BatchPoolFactory poolFactory;
        private readonly ILogger<App> logger;

        public App(
            BatchPoolFactory poolFactory,
            ILogger<App> logger)
        {
            this.poolFactory = poolFactory;
            this.logger = logger;
        }

        public async Task Run()
        {
            this.logger.LogInformation("Creating batch pool...");

            await this.poolFactory.CreateBatchPoolAsync(
                "WinFFmpegPool",
                "STANDARD_A1_v2");

            this.logger.LogInformation("Batch pool created.");
        }
    }
}
