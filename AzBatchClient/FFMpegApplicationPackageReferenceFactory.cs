using Microsoft.Azure.Batch;

namespace AzBatchClient
{
    public class FFMpegApplicationPackageReferenceFactory : ApplicationPackageReferenceFactory
    {
        public FFMpegApplicationPackageReferenceFactory() : base("ffmpeg", "3.4")
        {
        }

        public override ApplicationPackageReference CreateApplicationPackageReference()
        {
            return new ApplicationPackageReference
            {
                ApplicationId = this.ApplicationId,
                Version = this.PackageVersion
            };
        }
    }
}