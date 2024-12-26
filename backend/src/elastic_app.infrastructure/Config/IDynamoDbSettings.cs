namespace elastic_app.infrastructure.Config
{
    public interface IDynamoDbSettings
    {
        Storage Storage { get; set; }
        RetrySetting RetrySetting { get; set; }
    }
}
