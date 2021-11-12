using Microsoft.Azure.Batch;

namespace AzBatchClient
{
    public abstract class ApplicationPackageReferenceFactory
    {
        protected string ApplicationId { get; }
        protected string PackageVersion { get; }

        protected ApplicationPackageReferenceFactory(string applicationId, string packageVersion)
        {
            ApplicationId = applicationId;
            PackageVersion = packageVersion;
        }

        public abstract ApplicationPackageReference CreateApplicationPackageReference();
    }
}