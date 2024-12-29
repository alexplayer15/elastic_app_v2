namespace elastic_app.infrastructure.Config
{
    public class DynamoDbSettings : IDynamoDbSettings
    {
        public Storage Storage { get; set; }
        public RetrySetting RetrySetting { get; set; }
        public TimeSpan Timeout { get; set; }
    }
    public class RetrySetting
    {
        public int NoOfRetry { get; set; }
        public int Exponent { get; set; }
    }
    public class Storage
    {
        public bool LocalMode { get; set; }
        public string? LocalServiceUrl { get; set; }
    }
}
