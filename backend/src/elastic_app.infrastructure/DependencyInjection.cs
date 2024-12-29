using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon;
using elastic_app.infrastructure.Config;
using elastic_app.infrastructure.Repositories;
using elastic_app.domain.Abstractions;

namespace elastic_app.infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.AddTransient<IUserRepository, UserRepository>();
            _ = services.RegisterDynamoDb(configuration);
    
            return services;
        }
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
