namespace elastic_app.infrastructure.Settings
{
    public interface IDynamoDbSettings
    {
        Storage Storage { get; set; }
        RetrySetting RetrySetting { get; set; }
    }
}
