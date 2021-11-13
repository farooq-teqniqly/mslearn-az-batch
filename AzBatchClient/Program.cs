using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Batch;
using Microsoft.Azure.Batch.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AzBatchClient
{
    class Program
    {
        private static IConfigurationRoot configuration;

        static async Task Main(string[] args)
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", true)
                .AddEnvironmentVariables()
                .Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var app = serviceProvider.GetRequiredService<App>();
            await app.Run();

        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure
                .AddConsole()
                .AddApplicationInsights(configuration["AppInsightsKey"]));

            services.AddSingleton<App>();

            services.AddSingleton<TelemetryClient>(provider =>
            {
                var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
                telemetryConfiguration.InstrumentationKey = configuration["AppInsightsKey"];
                telemetryConfiguration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
                telemetryConfiguration.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());

                var module = new DependencyTrackingTelemetryModule
                {
                    EnableSqlCommandTextInstrumentation = true,
                    EnableAzureSdkTelemetryListener = true
                };

                module.Initialize(telemetryConfiguration);

                return new TelemetryClient(telemetryConfiguration);
            });

            ConfigureAzureStorage(services);
            ConfigureAzureBatch(services);
        }

        private static void ConfigureAzureBatch(IServiceCollection services)
        {
            var batchOptions = configuration.GetSection("AzBatch").Get<AzureBatchOptions>();
            services.AddSingleton(batchOptions);
            
            var sharedKeyCredentials = new BatchSharedKeyCredentials(
                batchOptions.Endpoint, 
                batchOptions.AccountName, 
                batchOptions.Key);

            var batchClient = BatchClient.Open(sharedKeyCredentials);

            services.AddSingleton(batchClient);

            services.AddSingleton<BatchPoolFactory>();
            services.AddSingleton<ImageReferenceFactory>();
            services.AddSingleton<VirtualMachineConfigurationFactory>();

            services.AddSingleton<ApplicationPackageReferenceFactory>(
                provider => new FFMpegApplicationPackageReferenceFactory());

            services.AddSingleton<ResourceFileFactory>();
        }

        private static void ConfigureAzureStorage(IServiceCollection services)
        {
            var storageOptions = configuration.GetSection("AzStorage").Get<AzureStorageOptions>();
            services.AddSingleton(storageOptions);

            var blobServiceClient = new BlobServiceClient(storageOptions.ConnectionString);
            services.AddSingleton(blobServiceClient);
            services.AddSingleton<InputContainerClient>();
            services.AddSingleton<OutputContainerClient>();
            services.AddSingleton<FileUploader>();
        }
    }
}
