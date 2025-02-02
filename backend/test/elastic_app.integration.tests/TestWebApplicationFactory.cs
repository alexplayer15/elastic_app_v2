using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Hosting;

namespace elastic_app.integration.tests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IAmazonDynamoDB>();

                services.AddSingleton<IAmazonDynamoDB>(sp =>
                {
                    var config = new AmazonDynamoDBConfig
                    {
                        ServiceURL = "http://localhost:4566"
                    };
                    return new AmazonDynamoDBClient(config);
                });
            });

            builder.UseEnvironment("Integration");
        }
    }
}

