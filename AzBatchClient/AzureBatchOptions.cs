namespace AzBatchClient
{
    public class AzureBatchOptions
    {
        public string Endpoint { get; set; }
        public string AccountName { get; set; }
        public string Key { get; set; }

        public Pool FFMpegPool { get; set; }
    }
}
