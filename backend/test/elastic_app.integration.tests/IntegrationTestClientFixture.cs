using Amazon.DynamoDBv2;
using elastic_app.integration.tests;
using Microsoft.Extensions.DependencyInjection;

[CollectionDefinition("IntegrationTestCollection")]
public class IntegrationTestClientFixture : IDisposable
{
    public IntegrationTestClient _client { get; private set; }
    public IAmazonDynamoDB _dynamoDbClient;

    public IntegrationTestClientFixture()
    {
        var factory = new CustomWebApplicationFactory<Program>();
        var httpClient = factory.CreateClient();
        _client = new IntegrationTestClient(httpClient);
        _dynamoDbClient = factory.Services.GetRequiredService<IAmazonDynamoDB>();
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}

