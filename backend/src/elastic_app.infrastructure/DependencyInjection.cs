using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using elastic_app.infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace elastic_app.infrastructure
{
    public static class DependencyInjection
    {

        private static IServiceCollection RegisterDynamoDb(this IServiceCollection services, IConfiguration configuration)
        {
            var dynamoDbConfig = new DynamoDbSettings();
            configuration.GetSection(DynamoDbConstants.DynamoDbConfigurationSection).Bind(dynamoDbConfig);
            _ = services.AddSingleton<IDynamoDbSettings>(_ => dynamoDbConfig);

            var clientConfig = new AmazonDynamoDBConfig
            {
                Timeout = dynamoDbConfig.Timeout,
                MaxErrorRetry = 1,
                RegionEndpoint = RegionEndpoint.EUWest2
            };

            _ = dynamoDbConfig.Storage.LocalMode
                ? services.AddSingleton<IAmazonDynamoDB>(t =>
                {
                    clientConfig.ServiceURL = dynamoDbConfig.Storage.LocalServiceUrl;
                    return new AmazonDynamoDBClient(new BasicAWSCredentials("DUMMYACCESSKEY", "DUMMYSECRETKEY"),
                        clientConfig);
                })
                : services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(clientConfig));

            _ = services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
            return services;
        }
    }
}
