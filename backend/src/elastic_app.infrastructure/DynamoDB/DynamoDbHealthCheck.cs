using Amazon.DynamoDBv2;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace elastic_app.infrastructure.DynamoDB
{
    public class DynamoDbHealthCheck : IHealthCheck
    {
        private readonly IAmazonDynamoDB _dynamoDb;

        public DynamoDbHealthCheck(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _dynamoDb.ListTablesAsync(cancellationToken);

                return response.HttpStatusCode == System.Net.HttpStatusCode.OK
                    ? HealthCheckResult.Healthy("DynamoDB is reachable.")
                    : HealthCheckResult.Unhealthy("DynamoDB returned non-OK status.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("DynamoDB check failed.", ex);
            }
        }
    }
}
