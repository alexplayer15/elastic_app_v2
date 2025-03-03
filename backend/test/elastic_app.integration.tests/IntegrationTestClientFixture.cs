using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using elastic_app.integration.tests;
using elastic_app.integration.tests.Hooks;
using Microsoft.Extensions.DependencyInjection;

[CollectionDefinition("IntegrationTestCollection")]
public class IntegrationTestClientFixture : IDisposable, IAsyncLifetime
{
    public IntegrationTestClient _client { get; private set; }
    public IAmazonDynamoDB _dynamoDbClient;
    public IntegrationTestHooks _hooks;
    private readonly HashSet<string> _testUsernames = new();
    public IntegrationTestClientFixture()
    {
        var factory = new CustomWebApplicationFactory<Program>();
        var httpClient = factory.CreateClient();
        _client = new IntegrationTestClient(httpClient);
        _dynamoDbClient = factory.Services.GetRequiredService<IAmazonDynamoDB>();
        _hooks = new IntegrationTestHooks();
    }

    public async Task InitializeAsync()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Integration");

        if (await _hooks.LocalStackContainerExists())
        {
            return;
        }

        await _hooks.SetUpLocalStack();
    }

    public async Task DisposeAsync()
    {
        foreach(var name in  _testUsernames)
        {
            await CleanUpTestUserAsync(name);
        }  
    }

    private async Task CleanUpTestUserAsync(string username)
    {
        var userDetails = await GetUserDetailsFromDynamoDB(username);
        if (userDetails != null && userDetails.ContainsKey("id"))
        {
            var userId = userDetails["id"].S;
            await DeleteTestUserAsync(userId);
        }
    }

    public async Task<Dictionary<string, AttributeValue>> GetUserDetailsFromDynamoDB(string username)
    {
        var request = new ScanRequest
        {
            TableName = "UserData",
            FilterExpression = "username = :u",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":u", new AttributeValue { S = username } }
                }
        };

        var response = await _dynamoDbClient.ScanAsync(request);

        return response.Items.Count > 0 ? response.Items[0] : null;
    }

    private async Task DeleteTestUserAsync(string userId)
    {
        var tableName = "UserData";

        await _dynamoDbClient.DeleteItemAsync(new DeleteItemRequest
        {
            TableName = tableName,
            Key = new Dictionary<string, AttributeValue> { { "id", new AttributeValue { S = userId } } }
        });
    }

    public void AddTestUser(string username)
    {
        _testUsernames.Add(username);
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}

